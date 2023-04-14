using GridPuzzleSolver.Solvers.KakuroSolver.Parser;
using GridPuzzleSolver.Solvers.KakuroSolver.Utilities;
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
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");
            var testFile = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = new KakuroParser().ParsePuzzle(testFile);

            var solve = puzzle.Solve();

            Assert.IsTrue(solve);

            IntegerPartitionCalculator.Reset();
        }
    }
}
