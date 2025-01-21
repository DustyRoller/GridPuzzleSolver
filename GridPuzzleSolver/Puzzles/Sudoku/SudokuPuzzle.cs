using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Puzzles.Sudoku
{
    /// <summary>
    /// Class representing a Sudoku puzzle.
    /// </summary>
    [XmlRoot("sudoku-puzzle")]
    public class SudokuPuzzle : Puzzle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuPuzzle"/> class.
        /// </summary>
        public SudokuPuzzle()
        {
            Height = 9;
            Width = 9;
        }

        /// <summary>
        /// Complete the puzzle, creating any missing cells.
        /// </summary>
        public override void CompletePuzzle()
        {
            // Ensure that there is at least one solved cell.
            if (!SolvedCells.Any())
            {
                throw new GridPuzzleSolverException("Puzzle contains no solved cells.");
            }

            // Populate the Cells list by either using the Cells in the
            // SolvedCells list or by creating new cells.
            for (uint r = 0u; r < Width; r++)
            {
                for (uint c = 0u; c < Height; c++)
                {
                    var coordinate = new Coordinate
                    {
                        X = r,
                        Y = c,
                    };

                    Cells.Add(SolvedCells.FirstOrDefault(c => c.Coordinate.Equals(coordinate), new PuzzleCell()
                    {
                        Coordinate = coordinate,
                    }));
                }
            }

            // Now create the sections to complete the puzzle.
            CreateSections();
        }

        /// <summary>
        /// Create the puzzle's sections.
        /// </summary>
        public void CreateSections()
        {
            // Get all the column and row sections.
            for (int i = 0; i < 9; ++i)
            {
                // Create the column section.
                CreateSection(Cells.OfType<PuzzleCell>()
                                   .Where(c => c.Coordinate.Y == i)
                                   .ToList());

                // Create the row section.
                CreateSection(Cells.OfType<PuzzleCell>()
                                   .Where(c => c.Coordinate.X == i)
                                   .ToList());
            }

            // Get all of the 3x3 squares from within the puzzle,
            // this is a pretty ugly way of doing it but works for now.
            var squareStartingIndexes = new List<int>()
            {
                0, 3, 6, 27, 30, 33, 54, 57, 60,
            };

            foreach (var startingIndex in squareStartingIndexes)
            {
                var index = startingIndex;
                var squareCells = new List<PuzzleCell>();
                for (int x = 0; x < 3; ++x)
                {
                    for (int y = 0; y < 3; ++y)
                    {
                        squareCells.Add((PuzzleCell)Cells[index]);
                        ++index;
                    }

                    index += 6;
                }

                CreateSection(squareCells);
            }
        }

        /// <summary>
        /// Create a section using the given cells.
        /// </summary>
        /// <param name="cells">The cells that make up the section.</param>
        private void CreateSection(List<PuzzleCell> cells)
        {
            var section = new SudokuSection();
            section.PuzzleCells.AddRange(cells);

            // Add the section to the puzzle.
            Sections.Add(section);

            // Set the section on each cell.
            cells.ForEach(cc => cc.Sections.Add(section));
        }
    }
}
