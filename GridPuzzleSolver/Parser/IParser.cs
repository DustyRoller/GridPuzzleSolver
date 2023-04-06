namespace GridPuzzleSolver.Parser
{
    internal interface IParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        Puzzle ParsePuzzle(string puzzleFilePath);
    }
}
