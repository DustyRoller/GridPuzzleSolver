using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Sudoku;
using GridPuzzleSolver.Puzzles.Sudoku.Parser;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Puzzles.Sudoku
{
    [TestFixture]
    public class SudokuPuzzleUnitTests
    {
        [Test]
        public void SudokuPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new SudokuPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void SudokuPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new SudokuPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void SudokuPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new SudokuPuzzle();

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

            var solved = puzzle.Solve();

            Assert.That(solved);
        }
    }
}
