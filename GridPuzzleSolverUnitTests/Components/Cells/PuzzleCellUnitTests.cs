using GridPuzzleSolver;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Puzzles.Sudoku;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Components.Cells
{
    [TestFixture]
    public class PuzzleCellUnitTests
    {
        [Test]
        public void PuzzleCell_CellValue_ThrowsExceptionIfValueIsGreaterThan9()
        {
            var puzzleCell = new PuzzleCell();

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzleCell.CellValue = 10u);

            Assert.That($"Puzzle cell value cannot be greater than 9. {puzzleCell.Coordinate}.", Is.EqualTo(ex?.Message));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsEmptyListIfNoSectionsSet()
        {
            var puzzleCell = new PuzzleCell();

            var expectedPossibleValues = new List<uint>();

            var actualPossibleValues = puzzleCell.PossibleValues;

            Assert.That(expectedPossibleValues, Is.EqualTo(actualPossibleValues));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsEveryValueForGivenSections()
        {
            var section = new SudokuSection();

            var puzzleCell = new PuzzleCell();

            puzzleCell.Sections.Add(section);

            var expectedPossibleValues = new List<uint>
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            var actualPossibleValues = puzzleCell.PossibleValues;

            Assert.That(expectedPossibleValues, Is.EqualTo(actualPossibleValues));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsOnlyCommonValuesFromBothSections()
        {
            var section1 = new SudokuSection();
            section1.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1,
            });
            section1.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2,
            });

            var section2 = new SudokuSection();
            section2.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 8,
            });
            section2.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 9,
            });

            var puzzleCell = new PuzzleCell();
            puzzleCell.Sections.Add(section1);
            puzzleCell.Sections.Add(section2);

            var expectedPossibleValues = new List<uint>
            {
                 3u, 4u, 5u, 6u, 7u,
            };

            var actualPossibleValues = puzzleCell.PossibleValues;

            Assert.That(expectedPossibleValues, Is.EqualTo(actualPossibleValues));
        }
    }
}
