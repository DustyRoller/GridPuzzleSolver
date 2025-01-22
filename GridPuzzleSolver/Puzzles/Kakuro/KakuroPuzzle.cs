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
        /// Gets or sets the puzzle's clue cells.
        /// </summary>
        public List<ClueCell> ClueCells { get; set; } = new List<ClueCell>();

        /// <summary>
        /// Complete the puzzle, creating any missing cells.
        /// </summary>
        public override void CompletePuzzle()
        {
            throw new NotImplementedException();
        }
    }
}
