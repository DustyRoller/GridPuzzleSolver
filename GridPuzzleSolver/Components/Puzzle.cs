using GridPuzzleSolver.Components.Cells;
using System.Text;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Components
{
    /// <summary>
    /// Base class representing a grid based puzzle, every puzzle will be made
    /// up of a number of Cells and Sections.
    /// </summary>
    public abstract class Puzzle
    {
        /// <summary>
        /// Gets or sets the puzzle's cells.
        /// </summary>
        [XmlArray("Cells")]
        [XmlArrayItem("Cell")]
        public List<Cell> Cells { get; set; } = new List<Cell>();

        /// <summary>
        /// Gets or sets the height of the puzzle by number of Cells.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Gets the number of currently unsolved puzzle cells.
        /// </summary>
        public int NumberOfUnsolvedCells => GetPuzzleCells().Count(pc => !pc.Solved);

        /// <summary>
        /// Gets or sets the width of the puzzle by number of Cells.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the sections of cells that make up this puzzle.
        /// </summary>
        internal List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// Complete the puzzle, creating any missing cells.
        /// </summary>
        public abstract void CompletePuzzle();

        /// <summary>
        /// Solve the puzzle.
        /// </summary>
        /// <returns>True if the puzzle was solved, otherwise false.</returns>
        public bool Solve()
        {
            var puzzleCells = Cells.OfType<PuzzleCell>();

            try
            {
                // Search for cells that only have one possible value as these
                // can be solved straight away.
                var solveableCells = puzzleCells.Where(pc => !pc.Solved && pc.PossibleValues.Count == 1)
                                                .ToList();
                while (solveableCells.Count != 0)
                {
                    solveableCells.ForEach(sc => sc.CellValue = sc.PossibleValues[0]);

                    solveableCells = puzzleCells.Where(pc => !pc.Solved && pc.PossibleValues.Count == 1)
                                                .ToList();
                }

                // If there are still unsolved cells then we can recursively
                // attempt to assign them values until we get a solution.
                var unsolvedCells = puzzleCells.Where(pc => !pc.Solved);
                if (unsolvedCells.Any())
                {
                    _ = RecursivelySolvePuzzle(unsolvedCells.ToList());
                }
            }
            catch (GridPuzzleSolverException ex)
            {
                Console.Error.WriteLine("Caught exception whilst solving puzzle");
                Console.Error.WriteLine(ex.ToString());
            }

            return puzzleCells.All(pc => pc.Solved) &&
                   Sections.TrueForAll(s => s.IsSolved());
        }

        /// <summary>
        /// Get a string representation of the current state of the puzzle.
        /// </summary>
        /// <returns>String representing the current state of the puzzle.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            var orderedCells = Cells.OrderBy(c => c.Coordinates.Y)
                                    .ThenBy(c => c.Coordinates.X)
                                    .ToList();

            for (var i = 0; i < orderedCells.Count; ++i)
            {
                sb.Append('|');

                if (i != 0 && i % Width == 0)
                {
                    sb.AppendLine();
                    sb.Append('|');
                }

                sb.Append(orderedCells[i].ToString());
            }

            sb.Append('|');

            return sb.ToString();
        }

        /// <summary>
        /// Gets the puzzle's puzzle cells.
        /// </summary>
        /// <returns>List of the puzzle's puzzle cells.</returns>
        protected List<PuzzleCell> GetPuzzleCells()
        {
            return Cells.OfType<PuzzleCell>().ToList();
        }

        /// <summary>
        /// Recursively solve the puzzle by working our way through all of the
        /// given PuzzleCell's possible values until we get to a solved puzzle.
        /// </summary>
        /// <param name="puzzleCells">The PuzzleCells to solve.</param>
        /// <returns>True if the all the PuzzleCells are solved, otherwise false.</returns>
        private static bool RecursivelySolvePuzzle(List<PuzzleCell> puzzleCells)
        {
            ArgumentNullException.ThrowIfNull(puzzleCells);

            // Reached the end of the recursion.
            if (puzzleCells.Count == 0)
            {
                return true;
            }

            var success = false;

            // Check if this recursion path has provided us with more
            // possibilities to explore before continuing.
            if (puzzleCells.TrueForAll(pc => pc.PossibleValues.Count != 0))
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
