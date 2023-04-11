namespace GridPuzzleSolver.Parser
{
    internal abstract class BaseParser : IParser
    {
        public abstract Puzzle ParsePuzzle(string puzzleFilePath);

        /// <summary>
        /// Validate that the input file contains a valid puzzle.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <param name="expectedFileExtension">The expected file extension for the puzzle file.</param>
        protected void ValidateInputFile(string puzzleFilePath, string expectedFileExtension)
        {
            if (!File.Exists(puzzleFilePath))
            {
                throw new FileNotFoundException("Unable to find puzzle file.", puzzleFilePath);
            }

            if (Path.GetExtension(puzzleFilePath) != expectedFileExtension)
            {
                throw new ArgumentException($"Invalid file type, expected {expectedFileExtension}.", nameof(puzzleFilePath));
            }

            // Make sure the file actually contains some data.
            if (new FileInfo(puzzleFilePath).Length == 0)
            {
                throw new ArgumentException("Puzzle file is empty.", nameof(puzzleFilePath));
            }
        }
    }
}
