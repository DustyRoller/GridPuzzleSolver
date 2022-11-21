using GridPuzzleSolver.Cells;
using System.Collections.ObjectModel;
using System.Text;

namespace GridPuzzleSolver
{
    /// <summary>
    /// Class representing a kakuro puzzle.
    /// </summary>
    internal class Puzzle
    {
        /// <summary>
        /// Gets all of the Cells that make up the puzzle.
        /// </summary>
        public ReadOnlyCollection<Cell> Cells => cells.AsReadOnly();

        /// <summary>
        /// Gets or sets the height of the puzzle by number of Cells.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Gets the number of currently unsolved puzzle cells.
        /// </summary>
        public int NumberOfUnsolvedCells => puzzleCells.Count(pc => !pc.Solved);

        /// <summary>
        /// Gets or sets the sections of cells that make up this puzzle.
        /// </summary>
        public List<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// Gets or sets the width of the puzzle by number of Cells.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// List of all of the cells in the puzzle.
        /// </summary>
        private readonly List<Cell> cells;

        /// <summary>
        /// List of all of the puzzle cells in the puzzle.
        /// </summary>
        private readonly List<PuzzleCell> puzzleCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Puzzle"/> class.
        /// </summary>
        public Puzzle()
        {
            cells = new List<Cell>();
            puzzleCells = new List<PuzzleCell>();
        }

        /// <summary>
        /// Add the given cell to this puzzle.
        /// </summary>
        /// <param name="cell">The cell to add.</param>
        public void AddCell(Cell cell)
        {
            cells.Add(cell);

            if (cell is PuzzleCell puzzleCell)
            {
                puzzleCells.Add(puzzleCell);
            }
        }

        /// <summary>
        /// Solve the puzzle.
        /// </summary>
        /// <returns>True if the puzzle was solved, otherwise false.</returns>
        public bool Solve()
        {
            try
            {
                // Search for cells that only have one possible value as these
                // can be solved straight away.
                var solveableCells = puzzleCells.Where(pc => !pc.Solved && pc.PossibleValues.Count == 1)
                                                .ToList();
                while (solveableCells.Any())
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
                    RecursivelySolvePuzzle(unsolvedCells.ToList());
                }
            }
            catch (KakuroSolverException ex)
            {
                Console.Error.WriteLine("Caught exception whilst solving puzzle");
                Console.Error.WriteLine(ex.ToString());
            }

            return puzzleCells.All(pc => pc.Solved) &&
                   Sections.All(s => s.IsSolved());
        }

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
        private bool RecursivelySolvePuzzle(List<PuzzleCell> puzzleCells)
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
            if (puzzleCells.All(pc => pc.PossibleValues.Any()))
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
