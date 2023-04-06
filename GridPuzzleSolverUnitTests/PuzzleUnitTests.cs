using GridPuzzleSolver.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.UnitTests
{
    [TestFixture]
    public class PuzzleUnitTests
    {
        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell());
            puzzle.AddCell(new PuzzleCell());

            Assert.AreEqual(2, puzzle.NumberOfUnsolvedCells);
        }

        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell());
            puzzle.AddCell(new PuzzleCell());
            puzzle.AddCell(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.AreEqual(2, puzzle.NumberOfUnsolvedCells);
        }

        [Test]
        public void Puzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new Puzzle();

            puzzle.AddCell(new PuzzleCell()
            {
                CellValue = 1u,
            });
            puzzle.AddCell(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.AreEqual(0, puzzle.NumberOfUnsolvedCells);
        }
    }
}
