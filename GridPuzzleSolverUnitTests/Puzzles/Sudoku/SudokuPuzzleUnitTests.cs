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
        public void SudokuPuzzle_CompletePuzzle_ThrowsExceptionIfPuzzleDoesNotContainExpectedNumberOfCells()
        {
            var puzzle = new SudokuPuzzle();

            puzzle.Cells.Add(new PuzzleCell()
            {
                Coordinates = new Coordinates
                {
                    X = 0,
                    Y = 0,
                },
            });

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CompletePuzzle());

            Assert.That(ex?.Message, Is.EqualTo("Puzzle should contain 81 cells but contains: 1"));
        }

        [Test]
        public void SudokuPuzzle_CompletePuzzle_ThrowsExceptionIfThereAreNoSolvedCells()
        {
            var puzzle = new SudokuPuzzle();

            for (uint x = 0u; x < 9u; ++x)
            {
                for (uint y = 0u; y < 9u; ++y)
                {
                    puzzle.Cells.Add(new PuzzleCell()
                    {
                        Coordinates = new Coordinates
                        {
                            X = x,
                            Y = y,
                        },
                    });
                }
            }

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CompletePuzzle());

            Assert.That(ex?.Message, Is.EqualTo("Puzzle contains no solved cells."));
        }

        [Test]
        public void SudokuPuzzle_CompletePuzzle_ThrowsExceptionIfSectionDoesNotHaveTheCorrectNumberOfCells()
        {
            var puzzle = new SudokuPuzzle();

            for (uint x = 0u; x < 9u; ++x)
            {
                for (uint y = 0u; y < 9u; ++y)
                {
                    // Fudge the coordinates so the first section ends up with
                    // twice as many cells in it.
                    var xCoord = x > 1 ? x : 0;
                    var yCoord = y > 1 ? y : 0;
                    var value = x == 0 && y == 0 ? 1u : 0u;

                    puzzle.Cells.Add(new PuzzleCell()
                    {
                        Coordinates = new Coordinates
                        {
                            X = xCoord,
                            Y = yCoord,
                        },
                        CellValue = value,
                    });
                }
            }

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CompletePuzzle());

            Assert.That(ex?.Message, Is.EqualTo("Section must have 9 cells, but received: 18"));
        }

        [Test]
        public void SudokuPuzzle_Solve_ThrowsExceptionIfASectionDoesNotContainUniqueSolvedValues()
        {
            var puzzle = new SudokuPuzzle();

            for (uint x = 0u; x < 9u; ++x)
            {
                for (uint y = 0u; y < 9u; ++y)
                {
                    puzzle.Cells.Add(new PuzzleCell()
                    {
                        Coordinates = new Coordinates
                        {
                            X = x,
                            Y = y,
                        },
                    });
                }
            }

            // Set only the first cell to have a value.
            ((PuzzleCell)puzzle.Cells[0]).CellValue = 1;
            ((PuzzleCell)puzzle.Cells[1]).CellValue = 1;

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzle.CompletePuzzle());

            Assert.That(ex?.Message, Is.EqualTo("Section's solved cells are not all unique"));
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

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.Zero);
        }
    }
}
