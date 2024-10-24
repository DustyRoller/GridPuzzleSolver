using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.Puzzles.Kakuro.UnitTests
{
    [TestFixture]
    public class KakuroSectionUnitTests
    {
        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsEmptyListsIfAllPuzzleCellsSolved()
        {
            var section = new KakuroSection(4u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });

            var partitions = section.CalculatePossibleValues();

            Assert.That(partitions, Is.Empty);
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsExpectedValuesForMagicNumber()
        {
            // 4 will be a magic number if there are two cells.
            var section = new KakuroSection(4u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 3u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsExpectedValues()
        {
            var sectionClueValue = 9u;
            var section = new KakuroSection(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 8u, 2u, 7u, 3u, 6u, 4u, 5u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsSingleValueIfOnlyOneUnsolvedPuzzleCell()
        {
            var sectionClueValue = 4u;
            var section = new KakuroSection(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 3u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsExpectedValuesWithSolvedPuzzleCell()
        {
            var sectionClueValue = 12u;
            var section = new KakuroSection(sectionClueValue);

            var solvedPuzzleCellValue = 3u;
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = solvedPuzzleCellValue,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 8u, 2u, 7u, 4u, 5u,
            };

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(),
            });

            Assert.That(!section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfSumOfCellsIsNotEqualToClueValue()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(),
            });

            Assert.That(!section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfCellsHaveDuplicatedValues()
        {
            var section = new KakuroSection(3u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });

            Assert.That(!section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsTrueForValidCompletedSection()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 9u,
                Coordinate = new Coordinate(),
            });

            Assert.That(section.IsSolved());
        }
    }
}
