namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Interface for all cell types.
    /// </summary>
    internal interface ICell
    {
        /// <summary>
        /// Gets or sets the Cell's Coordinates.
        /// </summary>
        Coordinates Coordinates { get; set; }
    }
}
