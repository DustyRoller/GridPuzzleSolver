namespace GridPuzzleSolver.Cells
{
    /// <summary>
    /// The PuzzleCell class represents a cell within the puzzle that requires
    /// solving.
    /// </summary>
    internal class PuzzleCell : Cell
    {
        /// <summary>
        /// Gets or sets the value of the cell, will be 0 if it hasn't been solved yet.
        /// </summary>
        public uint CellValue
        {
            get => cellValue;
            set
            {
                if (value > 9)
                {
                    throw new GridPuzzleSolverException($"Puzzle cell value cannot be greater than 9. {Coordinate}.");
                }

                cellValue = value;
            }
        }

        /// <summary>
        /// Gets the possible values for this cell.
        /// </summary>
        /// <remarks>
        /// This calculates the integer partitions for the column and row
        /// sections that this cell belongs to, and returns all of the common
        /// values into a single list.
        /// </remarks>
        public List<uint> PossibleValues => 
            Sections.Select(s => s.CalculatePossibleValues())
                    .Aggregate((previousPossValues, nextPossValues) => previousPossValues.Intersect(nextPossValues)
                                                                                         .ToList());

        /// <summary>
        /// Gets the sections that this cell belongs to.
        /// </summary>
        public List<ISection> Sections { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this cell has been solved or not.
        /// </summary>
        public bool Solved => CellValue != 0u;

        private uint cellValue = 0u;

        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleCell"/> class.
        /// </summary>
        public PuzzleCell()
        {
            Sections = new List<ISection>();
        }

        /// <summary>
        /// Get a string representation of the current state of the cell.
        /// </summary>
        /// <returns>String representing the current state of the cell.</returns>
        public override string ToString()
        {
            return Solved ? $"  {CellValue}  " : "  -  ";
        }
    }
}
