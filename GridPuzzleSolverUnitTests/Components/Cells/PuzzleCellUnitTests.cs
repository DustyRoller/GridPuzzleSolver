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
            var puzzleCell = new PuzzleCell(new Coordinate(0u, 0u));

            var ex = Assert.Throws<GridPuzzleSolverException>(() => puzzleCell.CellValue = 10u);

            Assert.AreEqual($"Puzzle cell value cannot be greater than 9. {puzzleCell.Coordinate}.", ex?.Message);
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsEveryValueForGivenSections()
        {
            var mockSection = new Mock<ISection>();

            var sectionPossibilities = new List<uint>
            {
                1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u,
            };

            mockSection.Setup(ms => ms.CalculatePossibleValues())
                       .Returns(sectionPossibilities);

            var puzzleCell = new PuzzleCell(new Coordinate(0u, 0u));

            puzzleCell.Sections.Add(mockSection.Object);

            var possibleValues = puzzleCell.PossibleValues;

            CollectionAssert.AreEqual(sectionPossibilities, possibleValues);
        }

        [Test]
        public void PuzzleCell_PossibleValues_ReturnsOnlyCommonValuesFromBothSections()
        {
            var columnSection = new Mock<ISection>();

            var columnSectionPossibilities = new List<uint>
            {
                1u, 2u, 3u,
            };

            columnSection.Setup(cs => cs.CalculatePossibleValues())
                         .Returns(columnSectionPossibilities);

            var rowSection = new Mock<ISection>();

            var rowSectionPossibilities = new List<uint>
            {
                2u, 3u, 4u, 5u,
            };

            rowSection.Setup(rs => rs.CalculatePossibleValues())
                      .Returns(rowSectionPossibilities);

            var puzzleCell = new PuzzleCell(new Coordinate(0u, 0u));

            puzzleCell.Sections.Add(columnSection.Object);
            puzzleCell.Sections.Add(rowSection.Object);

            var expectedValues = columnSectionPossibilities.Intersect(rowSectionPossibilities);

            var possibleValues = puzzleCell.PossibleValues;

            CollectionAssert.AreEqual(expectedValues, possibleValues);
        }
    }
}
