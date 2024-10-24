using Moq;
using NUnit.Framework;

namespace GridPuzzleSolver.Components.Cells.UnitTests
{
    [TestFixture]
    public class PuzzleCellUnitTests
    {
        [Test]
        public void PuzzleCell_CellValue_ThrowsExceptionIfValueIsGreaterThan9()
        {
            var puzzleCell = new PuzzleCell()
            {
                Coordinate = new Coordinate(),
            };

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzleCell.CellValue = 10u);

            Assert.That($"Puzzle cell value cannot be greater than 9. {puzzleCell.Coordinate}.", Is.EqualTo(ex?.Message));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsEmptyListIfNoSectionsSet()
        {
            var puzzleCell = new PuzzleCell()
            {
                Coordinate = new Coordinate(),
            };

            var expectedPossibleValues = new List<uint>();

            var possibleValues = puzzleCell.PossibleValues;

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsEveryValueForGivenSections()
        {
            var mockSection = new Mock<Section>();

            var sectionPossibilities = new List<uint>
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            mockSection.Setup(ms => ms.CalculatePossibleValues())
                       .Returns(sectionPossibilities);

            var puzzleCell = new PuzzleCell()
            {
                Coordinate = new Coordinate(),
            };

            puzzleCell.Sections.Add(mockSection.Object);

            var possibleValues = puzzleCell.PossibleValues;

            Assert.That(sectionPossibilities, Is.EqualTo(possibleValues));
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsOnlyCommonValuesFromBothSections()
        {
            var columnSection = new Mock<Section>();

            var columnSectionPossibilities = new List<uint>
            {
                1u, 2u, 3u,
            };

            columnSection.Setup(cs => cs.CalculatePossibleValues())
                         .Returns(columnSectionPossibilities);

            var rowSection = new Mock<Section>();

            var rowSectionPossibilities = new List<uint>
            {
                2u, 3u, 4u, 5u,
            };

            rowSection.Setup(rs => rs.CalculatePossibleValues())
                      .Returns(rowSectionPossibilities);

            var puzzleCell = new PuzzleCell()
            {
                Coordinate = new Coordinate(),
            };

            puzzleCell.Sections.Add(columnSection.Object);
            puzzleCell.Sections.Add(rowSection.Object);

            var expectedPossibleValues = columnSectionPossibilities.Intersect(rowSectionPossibilities);

            var possibleValues = puzzleCell.PossibleValues;

            Assert.That(expectedPossibleValues, Is.EqualTo(possibleValues));
        }
    }
}
