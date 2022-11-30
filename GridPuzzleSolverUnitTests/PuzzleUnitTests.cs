using GridPuzzleSolver.Cells;
using GridPuzzleSolver.KakuroSolver.Parser;
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

        [TestCase("Easy4x4Puzzle.kak")]
        [TestCase("Easy4x4Puzzle2.kak")]
        [TestCase("Easy6x6Puzzle.kak")]
        [TestCase("Intermediate4x4Puzzle.kak")]
        [TestCase("Hard9x11Puzzle.kak")]
        [TestCase("Hard9x11Puzzle2.kak")]
        [TestCase("Challenging9x17Puzzle.kak")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");
            var testFile = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = new KakuroParser().ParsePuzzle(testFile);

            Assert.IsTrue(puzzle.Solve());
        }
    }
}
