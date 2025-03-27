using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Kakuro;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Puzzles.Kakuro
{
    [TestFixture]
    public class KakuroPuzzleUnitTests
    {
        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.AllCells.Add(new PuzzleCell());
            puzzle.AllCells.Add(new PuzzleCell());

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.AllCells.Add(new PuzzleCell());
            puzzle.AllCells.Add(new PuzzleCell());
            puzzle.AllCells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.AllCells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });
            puzzle.AllCells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(0));
        }
    }
}
