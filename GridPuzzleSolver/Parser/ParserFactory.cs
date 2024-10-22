using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using GridPuzzleSolver.Puzzles.Sudoku.Parser;

namespace GridPuzzleSolver.Parser
{
    /// <summary>
    /// The ParserFactory is responsible for instantiating the correct Parser
    /// based on the given file extension.
    /// </summary>
    internal static class ParserFactory
    {
        /// <summary>
        /// Get a puzzle parser for the given puzzle file extension.
        /// </summary>
        /// <param name="puzzleFileExtension">The extension of the puzzle file.</param>
        /// <returns>An IParser implementation.</returns>
        public static IParser GetParser(string puzzleFileExtension)
        {
            if (string.IsNullOrEmpty(puzzleFileExtension))
            {
                throw new ArgumentException("Puzzle file extension is null or empty.", nameof(puzzleFileExtension));
            }

            IParser parser;

            if (puzzleFileExtension == KakuroParser.FileExtension)
            {
                parser = new KakuroParser();
            }
            else if (puzzleFileExtension == SudokuParser.FileExtension)
            {
                parser = new SudokuParser();
            }
            else
            {
                throw new ParserException($"File extension \'{puzzleFileExtension}\' not recognised.");
            }

            return parser;
        }
    }
}
