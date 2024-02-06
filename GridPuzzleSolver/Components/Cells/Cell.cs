namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Base Cell class.
    /// </summary>
    internal abstract class Cell : ICell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="coordinate">The cell's Coordinate.</param>
        protected Cell(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }

        /// <summary>
        /// Gets the Cell's Coordinate.
        /// </summary>
        public Coordinate Coordinate { get; }
    }
}
