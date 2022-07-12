using Subtitles;

string _originalFileName = "napisy do filmu.srt";
string _newFileName = "nowe napisy do filmu.srt";
string _errorsFileName = "errors.txt";
TimeSpan _offset = new TimeSpan(0, 0, 0, 5, 880);

SubtitlesTask subtitlesTask = new SubtitlesTask(_originalFileName, _newFileName, _errorsFileName, _offset);
subtitlesTask.Run();
