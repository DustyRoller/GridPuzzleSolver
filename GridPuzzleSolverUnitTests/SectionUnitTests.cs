using GridPuzzleSolver.Cells;
using GridPuzzleSolver.Utilities;
using NUnit.Framework;

namespace GridPuzzleSolver.UnitTests
{
    [TestFixture]
    public class SectionUnitTests
    {
        [Test]
        public void Section_CalculatePossibilities_ReturnsEmptyListsIfAllPuzzleCellsSolved()
        {
            var section = new Section(4u);

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

            var partitions = section.CalculateIntegerPartitions();

            Assert.AreEqual(0, partitions.Count);
        }

        [Test]
        public void Section_CalculatePossibilities_ReturnsSinglePartitionListForMagicNumber()
        {
            var section = new Section(4u);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var partitions = section.CalculateIntegerPartitions();

            Assert.AreEqual(1, partitions.Count);
            Assert.AreEqual(2, partitions[0].Count);
            Assert.IsTrue(partitions[0].Contains(1u));
            Assert.IsTrue(partitions[0].Contains(3u));
        }

        [Test]
        public void Section_CalculatePossibilities_ReturnsMultiplePartitionListForValue()
        {
            var sectionClueValue = 9u;
            var section = new Section(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var partitions = section.CalculateIntegerPartitions();

            Assert.AreEqual(4, partitions.Count);
            Assert.IsTrue(partitions.All(p => p.Count == 2));
            Assert.IsTrue(partitions.All(p => p.Sum() == sectionClueValue));
        }

        [Test]
        public void Section_CalculatePossibilities_ReturnsSingleValueIfOnlyOneUnsolvedPuzzleCell()
        {
            var sectionClueValue = 4u;
            var section = new Section(sectionClueValue);

            section.PuzzleCells.Add(new PuzzleCell
            {
                CellValue = 3u,
                Coordinate = new Coordinate(0u, 0u),
            });
            section.PuzzleCells.Add(new PuzzleCell
            {
                Coordinate = new Coordinate(0u, 0u),
            });

            var partitions = section.CalculateIntegerPartitions();

            Assert.AreEqual(1, partitions.Count);
            Assert.AreEqual(1, partitions[0].Count);
            Assert.AreEqual(1u, partitions[0][0]);
        }

        [Test]
        public void Section_CalculatePossibilities_ReturnsMultiplePartitionsWithSolvedPuzzleCell()
        {
            var sectionClueValue = 12u;
            var section = new Section(sectionClueValue);

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

            var partitions = section.CalculateIntegerPartitions();

            Assert.AreEqual(3, partitions.Count);
            Assert.IsTrue(partitions.All(p => p.Count == 2));
            var expectedSectionTotal = sectionClueValue - solvedPuzzleCellValue;
            Assert.IsTrue(partitions.All(p => p.Sum() == expectedSectionTotal));
        }

        [Test]
        public void Section_IsSolved_ReturnsFalseIfNotAllCellsAreSolved()
        {
            var section = new Section(12u);

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
        public void Section_IsSolved_ReturnsFalseIfSumOfCellsIsNotEqualToClueValue()
        {
            var section = new Section(12u);

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
        public void Section_IsSolved_ReturnsFalseIfCellsHaveDuplicatedValues()
        {
            var section = new Section(3u);

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
        public void Section_IsSolved_ReturnsTrueForValidCompletedSection()
        {
            var section = new Section(12u);

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
