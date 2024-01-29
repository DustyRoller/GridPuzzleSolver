using GridPuzzleSolver.Solvers.SudokuSolver.Parser;
using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.SudokuSolver.UnitTests
{
    [TestFixture]
    public class SudokuPuzzleUnitTests
    {
        [TestCase("EasyPuzzle.sud")]
        [TestCase("EasyPuzzle2.sud")]
        [TestCase("MediumPuzzle.sud")]
        [TestCase("HardPuzzle.sud")]
        [TestCase("ExpertPuzzle.sud")]
        public void Puzzle_Solve_SuccessfullySolvesTestPuzzles(string testPuzzleFileName)
        {
            var testPuzzleDir = Path.Combine("TestPuzzles", "Sudoku");
            var testFile = Path.Combine(testPuzzleDir, testPuzzleFileName);

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = new SudokuParser().ParsePuzzle(testFile);

            var solve = puzzle.Solve();

            Assert.IsTrue(solve);
        }
    }
}
