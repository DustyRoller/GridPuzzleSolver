using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.Puzzles.Sudoku.UnitTests
{
    [TestFixture]
    public class SudokuSectionUnitTests
    {
        [Test]
        public void SudokuSection_GetPossibleValues_ReturnsAllPotentialValuesWithNoSolvedCells()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            var expectedPossibleValues = new List<uint>()
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(section.CalculatePossibleValues()));
        }

        [Test]
        public void SudokuSection_GetPossibleValues_ReturnsValidPotentialValuesWithSolvedCells()
        {
            var section = new SudokuSection();

            section.PuzzleCells.AddRange(new List<PuzzleCell>
            {
                new PuzzleCell
                {
                    CellValue = 1u,
                    Coordinate = new Coordinate(),
                },
                new PuzzleCell
                {
                    CellValue = 3u,
                    Coordinate = new Coordinate(),
                },
                new PuzzleCell
                {
                    CellValue = 5u,
                    Coordinate = new Coordinate(),
                },
            });

            var expectedPossibleValues = new List<uint>()
            {
                2u, 4u, 6u, 7u, 8u, 9u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(section.CalculatePossibleValues()));
        }

        [Test]
        public void SudokuSection_Solved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell());

            Assert.That(!section.IsSolved());
        }

        [Test]
        public void SudokuSection_Solved_ReturnsTrueIfAllCellsAreSolved()
        {
            var section = new SudokuSection();

            section.PuzzleCells.Add(new PuzzleCell()
            {
                CellValue = 3u,
            });

            Assert.That(section.IsSolved());
        }
    }
}
