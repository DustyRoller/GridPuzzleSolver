using GridPuzzleSolver.Components.Cells;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GridPuzzleSolver.Components
{
    /// <summary>
    /// A section represents a ClueCell and a list of PuzzleCells.
    /// The sum of the values of the PuzzleCells must add up to the value
    /// of the ClueCell.
    /// </summary>
    internal abstract class Section : ISection
    {
        /// <summary>
        /// Gets the list of PuzzleCells that make up this section.
        /// </summary>
        public List<PuzzleCell> PuzzleCells { get; private set; } = new List<PuzzleCell>();

        /// <summary>
        /// Calculate all of the possible values that can be placed within this
        /// section.
        /// </summary>
        /// <returns>List of possible values for this section.</returns>
        public abstract List<uint> CalculatePossibleValues();

        /// <summary>
        /// Is this Section solved with valid answers.
        /// </summary>
        /// <returns>True if section is solved, otherwise false.</returns>
        public virtual bool IsSolved()
        {
            // Make sure that all puzzle cells are solved, that the total of
            // their value adds up to the clue value and that they all have
            // unique values.
            return PuzzleCells.All(pc => pc.Solved) &&
                   PuzzleCells.Select(pc => pc.CellValue).Distinct().Count() == PuzzleCells.Count;
        }
    }
}
