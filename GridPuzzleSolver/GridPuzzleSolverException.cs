namespace GridPuzzleSolver
{
    /// <summary>
    /// Exception class for any exception thrown during the solving of a puzzle.
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
