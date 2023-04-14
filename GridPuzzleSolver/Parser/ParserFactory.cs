using GridPuzzleSolver.KakuroSolver.Parser;
using GridPuzzleSolver.SudokuSolver.Parser;

namespace GridPuzzleSolver.Parser
{
    /// <summary>
    /// The ParserFactory is responsible for loading the correct Parser based
    /// on the file that has been provided.
    /// </summary>
    internal static class ParserFactory
    {
        public static IParser GetParser(string puzzleFile)
        {
            if (string.IsNullOrEmpty(puzzleFile))
            {
                throw new ArgumentException("Puzzle file is null or empty.", nameof(puzzleFile));
            }
            // Get the file extension from the file name.
            var fileExtension = Path.GetExtension(puzzleFile);

            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ParserException(
                    $"Failed to get file extension from puzzle file - {puzzleFile}.");
            }

            IParser parser;

            if (fileExtension == KakuroParser.FileExtension)
            {
                parser = new KakuroParser();
            }
            else if (fileExtension == SudokuParser.FileExtension)
            {
                parser = new SudokuParser();
            }
            else
            {
                throw new ParserException($"File extension \'{fileExtension}\' not recognised.");
            }

            return parser;
        }
    }
}
