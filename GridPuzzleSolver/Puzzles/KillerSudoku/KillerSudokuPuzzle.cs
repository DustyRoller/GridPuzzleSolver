using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Sudoku;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Puzzles.KillerSudoku
{
    /// <summary>
    /// Class representing a Killer Sudoku puzzle.
    /// </summary>
    [XmlRoot("KillerSudoku")]
    public class KillerSudokuPuzzle : Puzzle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KillerSudokuPuzzle"/> class.
        /// </summary>
        public KillerSudokuPuzzle()
        {
            Height = 9;
            Width = 9;
        }

        /// <summary>
        /// Complete the puzzle, ensuring that all the sections are setup properly.
        /// </summary>
        public override void CompletePuzzle()
        {
            if (Cells.Count != 81)
            {
                throw new GridPuzzleSolverException($"Puzzle should contain 81 cells but contains: {Cells.Count}");
            }

            // Now create the sections to complete the puzzle.
            CreateSections();
        }

        /// <summary>
        /// Create the puzzle's sections.
        /// </summary>
        public void CreateSections()
        {
            var puzzleCells = GetPuzzleCells();

            // Get all the column and row sections.
            for (int i = 0; i < 9; ++i)
            {
                // Create the column section.
                CreateSection(puzzleCells.Where(c => c.Coordinates.Y == i)
                                         .ToList());

                // Create the row section.
                CreateSection(puzzleCells.Where(c => c.Coordinates.X == i)
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
                        squareCells.Add(puzzleCells[index]);
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
            if (cells.Count != 9)
            {
                throw new GridPuzzleSolverException($"Section can only contain 9 cells but received: {cells.Count}");
            }

            var section = new SudokuSection();
            section.PuzzleCells.AddRange(cells);

            // Add the section to the puzzle.
            Sections.Add(section);

            // Set the section on each cell.
            cells.ForEach(cc => cc.Sections.Add(section));
        }
    }
}
