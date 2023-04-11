using GridPuzzleSolver;
using NUnit.Framework;

namespace GridPuzzleSolverSystemTests
{
    [TestFixture]
    public class KillerSudokuSystemTests
    {
        [TestCase("EasyPuzzle.xml")]
        public void KillerSudoku_SolveTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "KillerSudoku");
            var testFilePath = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.That(File.Exists(testFilePath), $"{testFilePath} does not exist");

            Assert.That(Program.Run(testFilePath));
        }
    }
}
