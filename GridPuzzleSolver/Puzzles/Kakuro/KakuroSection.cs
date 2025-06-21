using GridPuzzleSolver.Components;
using GridPuzzleSolver.Puzzles.Kakuro.Utilities;

namespace GridPuzzleSolver.Puzzles.Kakuro
{
    /// <summary>
    /// Class defining the sections that make up a kakuro puzzle.
    /// </summary>
    internal class KakuroSection : Section
    {
        /// <summary>
        /// The sections clue value.
        /// </summary>
        private uint clueValue;

        /// <summary>
        /// Gets or sets the clue value of this Section.
        /// </summary>
        public uint ClueValue
        {
            get => clueValue;
            set
            {
                if (value == 0u)
                {
                    throw new ArgumentException("Clue value must be greater than 0.");
                }

                clueValue = value;
            }
        }

        /// <summary>
        /// Calculate all of the possible values that can be placed within this
        /// section.
        /// </summary>
        /// <returns>List of possible values for this section.</returns>
        public override List<uint> CalculatePossibleValues()
        {
            return CalculateIntegerPartitions().SelectMany(ip => ip).ToList();
        }

        /// <summary>
        /// Is this Section solved with valid answers.
        /// </summary>
        /// <returns>True if section is solved, otherwise false.</returns>
        public override bool IsSolved()
        {
            // In addition to the base solve check, check that all of the
            // puzzle cells add up to the clue value.
            return base.IsSolved() &&
                   PuzzleCells.Sum(pc => pc.CellValue) == ClueValue;
        }

        /// <summary>
        /// Calculate all of the possible integer partitions for this section,
        /// taking into account already solved cells.
        /// </summary>
        /// <returns>List of integer partitions.</returns>
        private List<List<uint>> CalculateIntegerPartitions()
        {
            if (PuzzleCells.TrueForAll(pc => pc.Solved))
            {
                // This section is solved so return an empty list.
                return new List<List<uint>>();
            }

            List<List<uint>> partitions;

            var solvedPuzzleCells = PuzzleCells.FindAll(pc => pc.Solved);
            var numSolvedCells = solvedPuzzleCells.Count;
            var remainingClueValue = ClueValue - (uint)solvedPuzzleCells.Sum(pc => pc.CellValue);

            if (remainingClueValue == 0)
            {
                // This would only happen if our solving has gone wrong somewhere.
                throw new GridPuzzleSolverException("Invalid clue value of 0 found");
            }

            // If we only have on cell left then we can figure out what its
            // value will be.
            if (numSolvedCells == PuzzleCells.Count - 1)
            {
                partitions = new List<List<uint>>() { new List<uint> { remainingClueValue, } };
            }
            else
            {
                // Need to actually calculate the partitions.
                var maxValue = remainingClueValue <= 9 ? remainingClueValue - 1 : 9;
                var numCells = (uint)(PuzzleCells.Count - numSolvedCells);

                partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(remainingClueValue, numCells, 1u, maxValue);

                // Remove any partitions that contain a solved value.
                foreach (var solvedPuzzleCell in solvedPuzzleCells)
                {
                    partitions.RemoveAll(p => p.Contains(solvedPuzzleCell.CellValue));
                }
            }

            return partitions;
        }
    }
}
