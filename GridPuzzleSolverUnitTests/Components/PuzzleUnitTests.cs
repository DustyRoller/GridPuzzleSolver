using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.Components.UnitTests
{
    [TestFixture]
    public class PuzzleUnitTests
    {
        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u)));
            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u)));

            Assert.That(2, Is.EqualTo(puzzle.NumberOfUnsolvedCells));
        }

        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u)));
            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u)));
            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u))
            {
                CellValue = 1u,
            });

            Assert.That(2, Is.EqualTo(puzzle.NumberOfUnsolvedCells));
        }

        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u))
            {
                CellValue = 1u,
            });
            puzzle.AddCell(new PuzzleCell(new Coordinate(0u, 0u))
            {
                CellValue = 1u,
            });

            Assert.That(0, Is.EqualTo(puzzle.NumberOfUnsolvedCells));
        }
    }
}
