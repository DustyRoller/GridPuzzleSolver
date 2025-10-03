using GridPuzzleSolver;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Kakuro;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Puzzles.Kakuro
{
    [TestFixture]
    public class KakuroPuzzleUnitTests
    {
        [Test]
        public void KakuroPuzzle_CompletePuzzle_FailsWithPuzzleWithHeightLessThanThreeCells()
        {
            var puzzle = new KakuroPuzzle();

            for (uint x = 0u; x < 4u; ++x)
            {
                for (uint y = 0u; y < 1u; ++y)
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

            Assert.That(ex?.Message, Is.EqualTo("Puzzle must be at least two cells high."));
        }

        [Test]
        public void KakuroPuzzle_CompletePuzzle_FailsWithPuzzleWithWidthOfLessThanThreeCells()
        {
            var puzzle = new KakuroPuzzle();

            for (uint x = 0u; x < 1u; ++x)
            {
                for (uint y = 0u; y < 4u; ++y)
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

            Assert.That(ex?.Message, Is.EqualTo("Puzzle must be at least two cells wide."));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfNoneAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsNumberOfUnsolvedCellsIfSomeAreSolved()
        {
            var puzzle = new KakuroPuzzle();

            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell());
            puzzle.Cells.Add(new PuzzleCell()
            {
                CellValue = 1u,
            });

            Assert.That(puzzle.NumberOfUnsolvedCells, Is.EqualTo(2));
        }

        [Test]
        public void KakuroPuzzle_NumberOfUnsolvedCells_SuccessfullyReturnsZeroIfAllCellsAreSolved()
        {
            var puzzle = new KakuroPuzzle();

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
