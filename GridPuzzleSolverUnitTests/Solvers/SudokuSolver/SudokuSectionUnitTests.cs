using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.SudokuSolver.UnitTests
{
    [TestFixture]
    public class SudokuSectionUnitTests
    {
        [Test]
        public void SudokuSection_GetPossibleValues_ReturnsAllPotentialValuesWithNoSolvedCells()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell(new Coordinate(0u, 0u)));

            var expectedPossibleValues = new List<uint>()
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, section.CalculatePossibleValues());
        }

        [Test]
        public void SudokuSection_GetPossibleValues_ReturnsValidPotentialValuesWithSolvedCells()
        {
            var section = new SudokuSection();

            section.PuzzleCells.AddRange(new List<PuzzleCell>
            {
                new PuzzleCell(new Coordinate(0u, 0u))
                {
                    CellValue = 1u,
                },
                new PuzzleCell(new Coordinate(0u, 0u))
                {
                    CellValue = 3u,
                },
                new PuzzleCell(new Coordinate(0u, 0u))
                {
                    CellValue = 5u,
                },
            });

            var expectedPossibleValues = new List<uint>()
            {
                2u, 4u, 6u, 7u, 8u, 9u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, section.CalculatePossibleValues());
        }

        [Test]
        public void SudokuSection_Solved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell(new Coordinate(0u, 0u)));

            Assert.IsFalse(section.IsSolved());
        }

        [Test]
        public void SudokuSection_Solved_ReturnsTrueIfAllCellsAreSolved()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell(new Coordinate(0u, 0u))
            {
                CellValue = 3u,
            });

            Assert.IsTrue(section.IsSolved());
        }
    }
}
