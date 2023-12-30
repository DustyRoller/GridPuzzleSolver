namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Base Cell class.
    /// </summary>
    internal abstract class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the Cell's Coordinate.
        /// </summary>
        public Coordinate Coordinate { get; set; }
    }
}
