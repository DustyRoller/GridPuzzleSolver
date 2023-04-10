namespace GridPuzzleSolver.SudokuSolver
{
    /// <summary>
    /// An object representing a section within a sudoku puzzle. Each section
    /// is made up of nine cells and can represent either a column, row or 3x3
    /// square within a puzzle.
    /// </summary>
    internal class SudokuSection : Section
    {
        private static readonly List<uint> ValidValues = new()
        {
            1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
        };

        /// <summary>
        /// Calculate all of the possible values that can be placed within this
        /// section.
        /// </summary>
        /// <returns>List of possible values for this section.</returns>
        public override List<uint> CalculatePossibleValues()
        {
            return ValidValues.Except(PuzzleCells.Where(pc => pc.Solved)
                                                 .Select(c => c.CellValue))
                              .ToList();
        }
    }
}
