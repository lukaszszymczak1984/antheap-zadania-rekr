using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitles
{
    internal class SubtitlesBlock
    {
        /// <summary>
        /// Nr bloku
        /// </summary>
        internal int BlockNumber { get; set; }

        /// <summary>
        /// Tekst napisów
        /// </summary>
        internal List<string> Lines { get; set; } = new List<string>();

        /// <summary>
        /// Czas startu
        /// </summary>
        internal TimeSpan? Start { get; set; }

        /// <summary>
        /// Czas końca
        /// </summary>
        internal TimeSpan? End { get; set; }

        /// <summary>
        /// Błędy
        /// </summary>
        internal List<string> Errors = new List<string>();

        /// <summary>
        /// Czy czas poprawny
        /// </summary>
        private bool IsTimeValid => Start.HasValue && End.HasValue;

        /// <summary>
        /// Czy blok poprawny
        /// </summary>
        internal bool IsValid => !Errors.Any() && IsTimeValid;

        /// <summary>
        /// Czy blok zaczyna się od równych sekund
        /// </summary>
        /// <returns></returns>
        internal bool StartsFromFullSecond => IsTimeValid && Start!.Value.Milliseconds == 0;

        /// <summary>
        /// Czy blok NIE zaczyna się od równych sekund
        /// </summary>
        /// <returns></returns>
        internal bool StartsFromNotFullSecond => IsTimeValid && Start!.Value.Milliseconds != 0;

        /// <summary>
        /// Ustawienie nru bloku
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineIndex"></param>
        internal void SetBlockNumber(string line, int lineIndex)
        {
            // sprawdzenie poprawności
            if (Int32.TryParse(line, out int blockNumber))
            {
                BlockNumber = blockNumber;
            }
            else
            {
                // dodanie błędu
                Errors.Add($"Niepoprawny numer bloku. Linia: {lineIndex}");
            }
        }

        /// <summary>
        /// Ustawienie czasu startu i końca
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineIndex"></param>
        internal void SetTime(string line, int lineIndex)
        {
            string[] timeParts = line.Split("-->");
            // sprawdzenie poprawności
            if (timeParts.Length != 2)
            {
                // dodanie błędu
                Errors.Add($"Niepoprawna linia z czasem. Linia: {lineIndex}");
                return;
            }

            // parsowanie czasu startu
            TimeSpan? start = GetTime(timeParts[0]);
            // sprawdzenie poprawności
            if (start != null)
            {
                Start = start;
            }
            else
            {
                // dodanie błędu
                Errors.Add($"Niepoprawny czas startu. Linia: {lineIndex}");
            }

            // parsowanie czasu końca
            TimeSpan? end = GetTime(timeParts[1]);
            // spradzenie poprawności
            if (end != null)
            {
                End = end;
            }
            else
            {
                // dodanie błędu
                Errors.Add($"Niepoprawny czas końca. Linia: {lineIndex}");
            }
        }

        /// <summary>
        /// Przesunięcie w czasie
        /// </summary>
        /// <param name="offset"></param>
        internal void SetOffset(TimeSpan offset)
        {
            if (Start.HasValue)
            {
                Start = Start.Value.Add(offset);
            }
            if (End.HasValue)
            {
                End = End.Value.Add(offset);
            }
        }

        /// <summary>
        /// Odczyt czasu w formie tekstu
        /// </summary>
        /// <returns></returns>
        internal string GetTime()
        {
            string? start = null;
            string? end = null;
            // sprawdzenie poprawności czasów
            if (IsTimeValid)
            {
                start = $"{Start!.Value.Hours.ToString("00")}:{Start!.Value.Minutes.ToString("00")}:{Start!.Value.Seconds.ToString("00")},{Start!.Value.Milliseconds.ToString("000")}";
                end = $"{End!.Value.Hours.ToString("00")}:{End!.Value.Minutes.ToString("00")}:{End!.Value.Seconds.ToString("00")},{End!.Value.Milliseconds.ToString("000")}";
            }
            return $"{start} --> {end}";
        }

        /// <summary>
        /// Parsowanie czasu z tekstu
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private TimeSpan? GetTime(string text)
        {
            string[] timeParts = text.Trim().Split(':');
            if (timeParts.Length == 3)
            {
                string[] secondsParts = timeParts[2].Split(',');
                if (secondsParts.Length == 2)
                {
                    if (Int32.TryParse(timeParts[0], out int hours)
                        && Int32.TryParse(timeParts[1], out int minutes)
                        && Int32.TryParse(secondsParts[0], out int seconds)
                        && Int32.TryParse(secondsParts[1], out int miliseconds)
                        )
                    {
                        return new TimeSpan(0, hours, minutes, seconds, miliseconds);
                    }
                }
            }
            return null;
        }
    }
}
