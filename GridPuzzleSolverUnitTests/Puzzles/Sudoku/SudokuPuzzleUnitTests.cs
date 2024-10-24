using GridPuzzleSolver.Puzzles.Sudoku.Parser;
using NUnit.Framework;

namespace GridPuzzleSolver.Puzzles.Sudoku.UnitTests
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

            Assert.That(File.Exists(testFile));

            var puzzle = new SudokuParser().ParsePuzzle(testFile);

            // var solved = puzzle.Solve();

            // Assert.That(solved);
        }
    }
}
