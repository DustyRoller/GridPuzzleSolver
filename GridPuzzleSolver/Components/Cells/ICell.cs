namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Interface for all cell types.
    /// </summary>
    internal interface ICell
    {
        /// <summary>
        /// Gets the Cell's Coordinate.
        /// </summary>
        Coordinate Coordinate { get; }
    }
}
