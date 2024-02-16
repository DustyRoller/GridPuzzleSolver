using GridPuzzleSolver.Components;

namespace GridPuzzleSolver.Puzzles.KillerSudoku
{
    /// <summary>
    /// Class defining a cage within a killer sudoku puzzle.
    /// </summary>
    internal class Cage : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cage"/> class.
        /// </summary>
        /// <param name="clueValue">The clue value of this Section.</param>
        public Cage(uint clueValue)
        {
            if (clueValue == 0u)
            {
                throw new ArgumentException("Clue value must be greater than 0.");
            }

            ClueValue = clueValue;
        }

        /// <summary>
        /// Gets the clue value of this Section.
        /// </summary>
        public uint ClueValue { get; private set; }

        /// <summary>
        /// Calculate all of the possible values that can be placed within this
        /// section.
        /// </summary>
        /// <returns>List of possible values for this section.</returns>
        public override List<uint> CalculatePossibleValues()
        {
            return new List<uint> { 1 };
        }
    }
}
