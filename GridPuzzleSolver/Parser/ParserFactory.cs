using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using GridPuzzleSolver.Puzzles.Sudoku.Parser;

namespace GridPuzzleSolver.Parser
{
    /// <summary>
    /// The ParserFactory is responsible for loading the correct Parser based
    /// on the file that has been provided.
    /// </summary>
    internal static class ParserFactory
    {
        /// <summary>
        /// Get a puzzle parser for the given puzzle file.
        /// </summary>
        /// <param name="puzzleFile">The puzzle file to be parsed.</param>
        /// <returns>An IParser implementation.</returns>
        /// <exception cref="ArgumentException">Thrown if the puzzle file path is invalid.</exception>
        /// <exception cref="ParserException">Thrown if no suitable parser for the puzzle is found.</exception>
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
                throw new ArgumentException(
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
