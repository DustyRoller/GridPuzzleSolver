namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// The BlankCell class represents a cell within the puzzle that acts as a
    /// end point for rows and columns.
    /// </summary>
    internal class BlankCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlankCell"/> class.
        /// </summary>
        /// <param name="coordinate">The cell's Coordintates.</param>
        public BlankCell(Coordinate coordinate)
            : base(coordinate)
        {
        }

        /// <summary>
        /// Get a string representation of the cell.
        /// </summary>
        /// <returns>String representing the cell.</returns>
        public override string ToString()
        {
            return "  x  ";
        }
    }
}
