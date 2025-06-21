using GridPuzzleSolver;
using NUnit.Framework;

namespace GridPuzzleSolverSystemTests
{
    [TestFixture]
    public class KakuroSystemTests
    {
        [TestCase("Easy4x4Puzzle.kak")]
        [TestCase("Easy4x4Puzzle.xml")]
        [TestCase("Easy4x4Puzzle2.kak")]
        [TestCase("Easy4x4Puzzle2.xml")]
        [TestCase("Easy6x6Puzzle.kak")]
        [TestCase("Easy6x6Puzzle.xml")]
        [TestCase("Medium4x4Puzzle.kak")]
        [TestCase("Medium4x4Puzzle.xml")]
        [TestCase("Hard9x11Puzzle.kak")]
        [TestCase("Hard9x11Puzzle.xml")]
        [TestCase("Hard9x11Puzzle2.kak")]
        [TestCase("Hard9x11Puzzle2.xml")]
        [TestCase("Challenging9x17Puzzle.kak")]
        [TestCase("Challenging9x17Puzzle.xml")]
        public void Kakuro_SolveTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");
            var testFilePath = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.That(File.Exists(testFilePath), $"{testFilePath} does not exist");

            Assert.That(Program.Run(testFilePath));
        }
    }
}
