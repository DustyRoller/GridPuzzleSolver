using GridPuzzleSolver.Components.Cells;

namespace GridPuzzleSolver.Components
{
    /// <summary>
    /// Interface for sections within a puzzle.
    /// </summary>
    internal interface ISection
    {
        /// <summary>
        /// Gets the list of PuzzleCells that make up this section.
        /// </summary>
        List<PuzzleCell> PuzzleCells { get; }

        /// <summary>
        /// Calculate all of the possible values that can be placed within this
        /// section.
        /// </summary>
        /// <returns>List of possible values for this section.</returns>
        List<uint> CalculatePossibleValues();

        /// <summary>
        /// Is this Section solved with valid answers.
        /// </summary>
        /// <returns>True if section is solved, otherwise false.</returns>
        bool IsSolved();
    }
}
