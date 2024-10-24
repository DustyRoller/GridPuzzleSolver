using GridPuzzleSolver.Components.Cells;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Components
{
    /// <summary>
    /// Class representing a puzzle.
    /// </summary>
    [XmlRoot("puzzle")]
    public class Puzzle
    {
        /// <summary>
        /// List of all of the cells in the puzzle.
        /// </summary>
        private readonly List<Cell> cells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Puzzle"/> class.
        /// </summary>
        public Puzzle()
        {
            cells = new List<Cell>();
        }

        /// <summary>
        /// Gets or sets the puzzle's clue cells.
        /// </summary>
        [XmlArray("known_cells")]
        [XmlArrayItem("cell")]
        public List<PuzzleCell>? PuzzleCells { get; set; }

        /// <summary>
        /// Gets or sets the height of the puzzle by number of Cells.
        /// </summary>
        [XmlElement("height")]
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the puzzle type.
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the puzzle by number of Cells.
        /// </summary>
        [XmlElement("width")]
        public uint Width { get; set; }

        /// <summary>
        /// Gets all of the Cells that make up the puzzle.
        /// </summary>
        internal ReadOnlyCollection<Cell> Cells => cells.AsReadOnly();

        /// <summary>
        /// Gets or sets the sections of cells that make up this puzzle.
        /// </summary>
        internal List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// Get a string representation of the current state of the puzzle.
        /// </summary>
        /// <returns>String representing the current state of the puzzle.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < cells.Count; ++i)
            {
                sb.Append('|');

                if (i != 0 && i % Width == 0)
                {
                    sb.AppendLine();
                    sb.Append('|');
                }

                sb.Append(cells[i].ToString());
            }

            sb.Append('|');

            return sb.ToString();
        }

        /// <summary>
        /// Recursively solve the puzzle by working our way through all of the
        /// given PuzzleCell's possible values until we get to a solved puzzle.
        /// </summary>
        /// <param name="puzzleCells">The PuzzleCells to solve.</param>
        /// <returns>True if the all the PuzzleCells are solved, otherwise false.</returns>
        private static bool RecursivelySolvePuzzle(List<PuzzleCell> puzzleCells)
        {
            if (puzzleCells is null)
            {
                throw new ArgumentNullException(nameof(puzzleCells));
            }

            // Reached the end of the recursion.
            if (!puzzleCells.Any())
            {
                return true;
            }

            var success = false;

            // Check if this recursion path has provided us with more
            // possibilities to explore before continuing.
            if (puzzleCells.TrueForAll(pc => pc.PossibleValues.Any()))
            {
                // To save the amount of recursion required keep sorting
                // the list by the number of possible values.
                var orderedPuzzleCells = puzzleCells.OrderBy(pc => pc.PossibleValues.Count)
                                                    .ToList();
                var cell = orderedPuzzleCells[0];
                orderedPuzzleCells.RemoveAt(0);

                foreach (var possibleValue in cell.PossibleValues)
                {
                    // Set the cells value to this possible value so that future
                    // cells will use this value when calculate their possible values.
                    cell.CellValue = possibleValue;

                    success = RecursivelySolvePuzzle(orderedPuzzleCells);
                    if (success)
                    {
                        break;
                    }

                    cell.CellValue = 0u;
                }
            }

            return success;
        }
    }
}
