namespace GridPuzzleSolver
{
    /// <summary>
    /// Exception class for any exception thrown within the project that
    /// relates to the solving of a Kakuro puzzle.
    /// </summary>
    public class GridPuzzleSolverException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridPuzzleSolverException"/> class.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public GridPuzzleSolverException(string message)
            : base(message)
        {
        }
    }
}
