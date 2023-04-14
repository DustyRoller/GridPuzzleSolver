namespace GridPuzzleSolver.Parser
{
    internal interface IParser
    {
        /// <summary>
        /// Gets the file extension of the file that the parser will read.
        /// </summary>
        static string FileExtension { get; } = string.Empty;

        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        Puzzle ParsePuzzle(string puzzleFilePath);
    }
}
