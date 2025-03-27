using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;

namespace GridPuzzleSolver.Puzzles.Kakuro
{
    /// <summary>
    /// Class representing a Kakuro puzzle.
    /// </summary>
    internal class KakuroPuzzle : Puzzle
    {
        /// <summary>
        /// Gets or sets the complete set of the puzzle's cells.
        /// </summary>
        public List<Cell> AllCells { get; set; } = new List<Cell>();

        /// <summary>
        /// Gets the complete set of the puzzle's cells.
        /// </summary>
        public override List<PuzzleCell> Cells => GetPuzzleCells();

        /// <summary>
        /// Complete the puzzle, creating any missing cells.
        /// </summary>
        public override void CompletePuzzle()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get this puzzle's puzzle cells.
        /// </summary>
        /// <returns>List of this puzzle's puzzle cells.</returns>
        private List<PuzzleCell> GetPuzzleCells()
        {
            return AllCells.OfType<PuzzleCell>().ToList();
        }
    }
}
