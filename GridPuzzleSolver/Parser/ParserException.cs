namespace GridPuzzleSolver
{
    /// <summary>
    /// Exception class for exceptions throw whilst parsing a puzzle.
    /// </summary>
    internal class ParserException : GridPuzzleSolverException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message to be associate with the exception.</param>
        public ParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message to be associate with the exception.</param>
        /// <param name="innerExcepion">The exception that is the cause of the current exception.</param>
        public ParserException(string message, Exception innerExcepion)
            : base(message, innerExcepion)
        {
        }
    }
}
