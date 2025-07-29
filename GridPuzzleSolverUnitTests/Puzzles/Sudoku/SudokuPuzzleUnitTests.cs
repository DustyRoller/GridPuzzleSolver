using GridPuzzleSolver;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Sudoku;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Puzzles.Sudoku
{
    [TestFixture]
    public class SudokuPuzzleUnitTests
    {
        [Test]
        public void SudokuPuzzle_CompletePuzzle_ThrowsExceptionIfThereAreNoSolvedCells()
        {
            var puzzle = new SudokuPuzzle();

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CompletePuzzle());

            Assert.That(ex?.Message, Is.EqualTo("Puzzle contains no solved cells."));
        }

        [Test]
        public void SudokuPuzzle_CreateSections_ThrowsExceptionIfSectionDoesNotHaveTheCorrectNumberOfCells()
        {
            var puzzle = new SudokuPuzzle();

            // Add one cell to the puzzle.
            puzzle.Cells.Add(new PuzzleCell()
            {
                Coordinates = new Coordinates
                {
                    X = 0,
                    Y = 0,
                },
            });

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CreateSections());

            Assert.That(ex?.Message, Is.EqualTo($"Section must have 9 cells, but received: {puzzle.Cells.Count}"));
        }

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
    }
}
