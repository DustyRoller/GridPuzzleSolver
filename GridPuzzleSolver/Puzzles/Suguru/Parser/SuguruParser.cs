using GridPuzzleSolver.Components;
using GridPuzzleSolver.Parser;

namespace GridPuzzleSolver.Puzzles.Suguru.Parser
{
    /// <summary>
    /// Class to parse Suguru puzzles from text files.
    /// </summary>
    internal class SuguruParser : BaseParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public override Puzzle ParsePuzzle(string puzzleFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
