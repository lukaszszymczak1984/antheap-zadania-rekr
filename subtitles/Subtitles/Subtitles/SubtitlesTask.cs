using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Subtitles
{
    internal class SubtitlesTask
    {
        private readonly string _originalFileName;
        private readonly string _newFileName;
        private readonly string _errorsFileName;
        private readonly TimeSpan _offset;

        internal SubtitlesTask(string originalFileName, string newFileName, string errorsFileName, TimeSpan offset)
        {
            _originalFileName = originalFileName;
            _newFileName = newFileName;
            _offset = offset;
            _errorsFileName = errorsFileName;
        }

        internal void Run()
        {
            // odczyt napisów
            List<SubtitlesBlock> blocks = ReadBlocks(_originalFileName);

            // zapis napisów, które nie zaczynają się o równych sekundach
            WriteBlocks(blocks.Where(e => e.StartsFromNotFullSecond).ToList(), _originalFileName);

            // zapis napisów do innego pliku, które zaczynają się o równych sekudnach
            WriteBlocks(blocks.Where(e => e.StartsFromFullSecond).ToList(), _newFileName);

            // zapis błędów do pliku
            WriteErrors(blocks);
        }

        /// <summary>
        /// Odczyt napisów
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<SubtitlesBlock> ReadBlocks(string fileName)
        {
            List<SubtitlesBlock> result = new List<SubtitlesBlock>();
            if (File.Exists(fileName))
            {
                using (var sr = new StreamReader(fileName))
                {
                    string? line;

                    // indeks linii w pliku
                    int lineIndex = 1;
                    // indeks linii w bloku
                    int blockLineIndex = 0;

                    bool endOfFile = false;
                    SubtitlesBlock block = new SubtitlesBlock();

                    // sprawdzenie końca pliku
                    while (!endOfFile)
                    {
                        // odczyt linii
                        line = sr.ReadLine();

                        // dodanie obiektu do kolekcji
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            result.Add(block);
                            block = new SubtitlesBlock();
                            blockLineIndex = 0;

                            // sprawdzenie końca pliku
                            if (line == null)
                            {
                                endOfFile = true;
                            }
                            continue;
                        }

                        // odczyt numeracji bloku napisów
                        if (blockLineIndex == 0)
                        {
                            block.SetBlockNumber(line, lineIndex);
                        }
                        // odczyt czasu
                        else if (blockLineIndex == 1)
                        {
                            // odczyt czasu
                            block.SetTime(line, lineIndex);
                            // dodanie przesunięcia w czasie
                            block.SetOffset(_offset);
                        }
                        // odczyt napisów
                        else
                        {
                            block.Lines.Add(line);
                        }

                        // zwiększenie indeksu bloku
                        blockLineIndex++;
                        // zwiększenie indeksu linii w pliku
                        lineIndex++;
                    }
                }
            }
            // zwrócenie kolekcji bloków napisów
            return result;
        }

        // zapis napisów do pliku
        private void WriteBlocks(List<SubtitlesBlock> blocks, string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var block in blocks)
                {
                    // zapis bieżącego numeru bloku
                    sw.WriteLine((blocks.IndexOf(block) + 1).ToString());
                    // zapis czasu
                    sw.WriteLine(block.GetTime());
                    // zapis napisów
                    foreach (string textLine in block.Lines)
                    {
                        sw.WriteLine(textLine);
                    }
                    // usunięcie nadmiarowego pustego wiersza
                    if (blocks.IndexOf(block) < blocks.Count - 1)
                    {
                        sw.WriteLine("");
                    }
                }
            }
        }

        /// <summary>
        /// Zapis błędów
        /// </summary>
        /// <param name="blocks"></param>
        private void WriteErrors(List<SubtitlesBlock> blocks)
        {
            using (var sw = new StreamWriter(_errorsFileName))
            {
                // pobieranie błędów z nieprawidłowch bloków
                List<string> errors = blocks.Where(e => !e.IsValid).SelectMany(e => e.Errors).ToList();
                // zapis
                foreach (var error in errors)
                {
                    sw.WriteLine(error);
                }
            }
        }
    }
}
