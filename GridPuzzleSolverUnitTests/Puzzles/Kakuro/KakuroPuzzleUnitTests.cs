using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using NUnit.Framework;

namespace GridPuzzleSolver.Puzzles.Kakuro.UnitTests
{
    [TestFixture]
    public class KakuroPuzzleUnitTests
    {
        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.Cells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });
            puzzle.Cells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(0));
        }

        [TestCase("Easy4x4Puzzle.kak")]
        [TestCase("Easy4x4Puzzle2.kak")]
        [TestCase("Easy6x6Puzzle.kak")]
        [TestCase("Medium4x4Puzzle.kak")]
        [TestCase("Hard9x11Puzzle.kak")]
        [TestCase("Challenging9x17Puzzle.kak")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");
            var testFile = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.That(File.Exists(testFile));

            var puzzle = new KakuroParser().ParsePuzzle(testFile);

            var solve = puzzle.Solve();

            Assert.That(solve);
        }
    }
}
