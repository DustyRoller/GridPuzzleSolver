using GridPuzzleSolver;
using NUnit.Framework;

namespace GridPuzzleSolverSystemTests
{
    [TestFixture]
    public class SudokuSystemTests
    {
        [TestCase("EasyPuzzle.sud")]
        [TestCase("EasyPuzzle2.sud")]
        [TestCase("MediumPuzzle.sud")]
        [TestCase("HardPuzzle.sud")]
        [TestCase("ExpertPuzzle.sud")]
        [TestCase("EasyPuzzle.xml")]
        [TestCase("EasyPuzzle2.xml")]
        [TestCase("MediumPuzzle.xml")]
        [TestCase("HardPuzzle.xml")]
        [TestCase("ExpertPuzzle.xml")]
        public void Sudoku_SolveTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Sudoku");
            var testFilePath = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.That(File.Exists(testFilePath), $"{testFilePath} does not exist");

            Assert.That(Program.Run(testFilePath));
        }
    }
}
