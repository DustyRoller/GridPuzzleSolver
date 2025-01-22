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
        public void SudokuPuzzle_CompletePuzzle_SuccessfullyCreatesPuzzle()
        {
            var puzzle = new SudokuPuzzle();

            var solvedPuzzleCell = new PuzzleCell
            {
                CellValue = 1,
                Coordinates = new Coordinates
                {
                    X = 0,
                    Y = 0,
                },
            };

            puzzle.SolvedCells.Add(solvedPuzzleCell);

            puzzle.CompletePuzzle();

            Assert.That(puzzle.Cells, Has.Count.EqualTo(81));
            Assert.That(puzzle.Cells[0], Is.EqualTo(solvedPuzzleCell));
            Assert.That(puzzle.Sections, Has.Count.EqualTo(27));

            var index = 0;

            for (var x = 0u; x < 9; ++x)
            {
                for (var y = 0u; y < 9; ++y)
                {
                    Assert.That(puzzle.Cells[index].Coordinates.X, Is.EqualTo(x));
                    Assert.That(puzzle.Cells[index].Coordinates.Y, Is.EqualTo(y));

                    index++;
                }
            }

            // Assert that all the columns and rows have the expected coordinates.
            // The first eighteen sections alternate between rows and columns
            // (with squares at the end).
            var columnAndRowSections = puzzle.Sections.Take(18);

            var columnSections = columnAndRowSections.Where((c, i) => i % 2 == 0).ToList();
            var rowSections = columnAndRowSections.Skip(1).Where((c, i) => i % 2 == 0).ToList();

            for (int i = 0; i < 9; ++i)
            {
                Assert.That(columnSections[i].PuzzleCells.All(c => c.Coordinates.Y == i));
                Assert.That(rowSections[i].PuzzleCells.All(c => c.Coordinates.X == i));
            }
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
