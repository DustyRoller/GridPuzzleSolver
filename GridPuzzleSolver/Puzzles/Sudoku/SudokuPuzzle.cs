using GridPuzzleSolver.Components;

namespace GridPuzzleSolver.Puzzles.Sudoku
{
    /// <summary>
    /// Class representing a Sudoku puzzle.
    /// </summary>
    internal class SudokuPuzzle : Puzzle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuPuzzle"/> class.
        /// </summary>
        public SudokuPuzzle()
        {
            Height = 9;
            Width = 9;
        }
    }
}
