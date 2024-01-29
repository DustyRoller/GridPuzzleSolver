using GridPuzzleSolver.Components;
using GridPuzzleSolver.Solvers.KakuroSolver.Utilities;

namespace GridPuzzleSolver.Solvers.KakuroSolver
{
    /// <summary>
    /// Class defining the sections that make up a kakuro puzzle.
    /// </summary>
    internal class KakuroSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KakuroSection"/> class.
        /// </summary>
        /// <param name="clueValue">The clue value of this Section.</param>
        public KakuroSection(uint clueValue)
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
            var clueValue = ClueValue - (uint)solvedPuzzleCells.Sum(pc => pc.CellValue);

            if (clueValue == 0)
            {
                // This would only happen if our solving has gone wrong somewhere.
                throw new GridPuzzleSolverException("Invalid clue value of 0 found");
            }

            // If we only have on cell left then we can figure out what its
            // value will be.
            if (numSolvedCells == PuzzleCells.Count - 1)
            {
                partitions = new List<List<uint>>() { new List<uint> { clueValue, } };
            }
            else
            {
                // Need to actually calculate the partitions.
                var maxValue = clueValue <= 9 ? clueValue - 1 : 9;
                var numCells = (uint)(PuzzleCells.Count - numSolvedCells);

                partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(clueValue, numCells, 1u, maxValue);

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
