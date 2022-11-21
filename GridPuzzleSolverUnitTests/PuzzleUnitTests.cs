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

        [TestCase("Easy4x4Puzzle.txt")]
        [TestCase("Easy4x4Puzzle2.txt")]
        [TestCase("Easy6x6Puzzle.txt")]
        [TestCase("Intermediate4x4Puzzle.txt")]
        [TestCase("Hard9x11Puzzle.txt")]
        [TestCase("Hard9x11Puzzle2.txt")]
        [TestCase("Challenging9x17Puzzle.txt")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            const string TestPuzzleDir = "TestPuzzles";
            var testFile = Path.Combine(TestPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.Parser.ParsePuzzle(testFile);

            Assert.IsTrue(puzzle.Solve());
        }
    }
}
