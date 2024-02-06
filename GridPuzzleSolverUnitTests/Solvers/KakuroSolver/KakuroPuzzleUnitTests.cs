using GridPuzzleSolver.Solvers.KakuroSolver.Parser;
using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.KakuroSolver.UnitTests
{
    [TestFixture]
    public class KakuroPuzzleUnitTests
    {
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
