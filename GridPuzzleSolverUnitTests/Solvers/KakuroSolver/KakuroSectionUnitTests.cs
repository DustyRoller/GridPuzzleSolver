using GridPuzzleSolver.Cells;
using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.KakuroSolver.UnitTests
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
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });

            var partitions = section.CalculatePossibleValues();

            Assert.AreEqual(0, partitions.Count);
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsExpectedValuesForMagicNumber()
        {
            // 4 will be a magic number if there are two cells.
            var section = new KakuroSection(4u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 3u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, possibleValues);
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsExpectedValues()
        {
            var sectionClueValue = 9u;
            var section = new KakuroSection(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 8u, 2u, 7u, 3u, 6u, 4u, 5u
            };

            CollectionAssert.AreEqual(expectedPossibleValues, possibleValues);
        }

        [Test]
        public void KakuroSection_CalculatePossibleValues_ReturnsSingleValueIfOnlyOneUnsolvedPuzzleCell()
        {
            var sectionClueValue = 4u;
            var section = new KakuroSection(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 3u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, possibleValues);
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
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var possibleValues = section.CalculatePossibleValues();

            var expectedPossibleValues = new List<uint>
            {
                1u, 8u, 2u, 7u, 4u, 5u,
            };

            CollectionAssert.AreEqual(expectedPossibleValues, possibleValues);
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            Assert.IsFalse(section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfSumOfCellsIsNotEqualToClueValue()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(0u, 0u),
            });

            Assert.IsFalse(section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsFalseIfCellsHaveDuplicatedValues()
        {
            var section = new KakuroSection(3u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });

            Assert.IsFalse(section.IsSolved());
        }

        [Test]
        public void KakuroSection_IsSolved_ReturnsTrueForValidCompletedSection()
        {
            var section = new KakuroSection(12u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 1u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 2u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 9u,
                Coordinate = new Coordinate(0u, 0u),
            });

            Assert.IsTrue(section.IsSolved());
        }
    }
}
