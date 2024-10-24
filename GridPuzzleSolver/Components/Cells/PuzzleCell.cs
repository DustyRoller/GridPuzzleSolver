using System.Linq;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// The PuzzleCell class represents a cell within the puzzle that requires
    /// solving.
    /// </summary>
    public class PuzzleCell : Cell
    {
        private uint cellValue = 0u;

        /// <summary>
        /// Gets or sets the value of the cell, will be 0 if it hasn't been solved yet.
        /// </summary>
        [XmlElement("value")]
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
        public List<uint> PossibleValues
        {
            get
            {
                if (Sections.Any())
                {
                    return Sections.Select(s => s.CalculatePossibleValues())
                        .Aggregate((previousPossValues, nextPossValues) => previousPossValues.Intersect(nextPossValues).ToList());
                }
                else
                {
                    return new List<uint>();
                }
            }
        }

        /// <summary>
        /// Gets the sections that this cell belongs to.
        /// </summary>
        public List<Section> Sections { get; } = new List<Section>();

        /// <summary>
        /// Gets a value indicating whether this cell has been solved or not.
        /// </summary>
        public bool Solved => CellValue != 0u;

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
