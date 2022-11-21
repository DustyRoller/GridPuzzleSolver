namespace GridPuzzleSolver
{
    /// <summary>
    /// Exception class for any exception thrown within the project that
    /// relates to the solving of a Kakuro puzzle.
    /// </summary>
    public class KakuroSolverException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KakuroSolverException"/> class.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public KakuroSolverException(string message)
            : base(message)
        {
        }
    }
}
