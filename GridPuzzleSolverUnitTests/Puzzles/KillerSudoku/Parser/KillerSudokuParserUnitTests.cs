using GridPuzzleSolver.Components;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace GridPuzzleSolver.Puzzles.KillerSudoku.Parser.UnitTests
{
    [TestFixture]
    public class KillerSudokuParserUnitTests
    {
        private const string EmptyPuzzleFileName = "EmptyPuzzle.ksud";
        private const string TestPuzzleFileName = "TestPuzzle.ksud";

        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "KillerSudoku");

        [SetUp]
        public void BaseSetUp()
        {
            // If the test file already exists fail the unit test.
            Assert.That(!File.Exists(TestPuzzleFileName), $"Test file {TestPuzzleFileName} already exists");
        }

        [TearDown]
        public void BaseTearDown()
        {
            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithNonExistantFile()
        {
            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<FileNotFoundException>(() => parser.ParsePuzzle("randomfile"));

            Assert.That(ex?.Message, Is.EqualTo("Unable to find puzzle file."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.That(ex?.Message, Is.EqualTo("Invalid file type, expected .ksud. (Parameter 'puzzleFilePath')"));

            File.Delete(fileName);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file is empty. (Parameter 'puzzleFilePath')"));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleFileThatDoesNotMatchExpectedSchema()
        {
            // Create the bare minimum file without the puzzle section.
            var xmlDoc = new XmlDocument();

            var cellsNode = xmlDoc.CreateElement("cells");

            // Create a cell.
            var cellNode = xmlDoc.CreateElement("cell");
            var cellIdNode = xmlDoc.CreateElement("id");
            cellIdNode.InnerText = "0";
            var cellXNode = xmlDoc.CreateElement("x");
            cellXNode.InnerText = "0";
            var cellYNode = xmlDoc.CreateElement("y");
            cellYNode.InnerText = "0";

            cellNode.AppendChild(cellIdNode);
            cellNode.AppendChild(cellXNode);
            cellNode.AppendChild(cellYNode);

            cellsNode.AppendChild(cellNode);

            xmlDoc.AppendChild(cellsNode);

            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Failed to validate puzzle file against the schema."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfCells()
        {
            // Read in the empty Puzzle file to be modified.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(testPuzzleDir, EmptyPuzzleFileName));

            var cellNodesList = xmlDoc.SelectNodes("//puzzle/grid/cells/cell");
            if (cellNodesList == null)
            {
                Assert.Fail("Failed to find cell nodes in test puzzle.");
            }

            var cell = cellNodesList?[40];

            var removedCell = cell?.ParentNode?.RemoveChild(cell);
            if (removedCell == null)
            {
                Assert.Fail("Failed to remove cell from test puzzle.");
            }

            // Write out modified puzzle.
            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Failed to validate puzzle file against the schema."));
            Assert.That(ex?.InnerException?.Message, Is.EqualTo("The element 'cells' has incomplete content. List of possible elements expected: 'cell'."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleWithNoCages()
        {
            // Read in the empty Puzzle file to be modified.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(testPuzzleDir, EmptyPuzzleFileName));

            var cageNodesList = xmlDoc.SelectNodes("//puzzle/cages/cage")
                ?? throw new Exception("Failed to find cage nodes in test puzzle.");

            foreach (XmlNode cage in cageNodesList)
            {
                cage.ParentNode?.RemoveChild(cage);
            }

            // Write out modified puzzle.
            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            var numCages = xmlDoc.SelectNodes("//puzzle/cages/cage")?.Count;

            Assert.That(ex?.Message, Is.EqualTo("Failed to validate puzzle file against the schema."));
            Assert.That(ex?.InnerException?.Message, Is.EqualTo("The element 'cages' has incomplete content. List of possible elements expected: 'cage'."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithCageWithSumGreaterThan45()
        {
            // Read in the empty Puzzle file to be modified.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(testPuzzleDir, EmptyPuzzleFileName));

            var sumNode = xmlDoc.SelectSingleNode("//puzzle/cages/cage/sum")
                ?? throw new Exception("Failed to find cage sum node in test puzzle.");

            var invalidSumValue = "99";

            sumNode.InnerText = invalidSumValue;

            // Write out modified puzzle.
            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            var numCages = xmlDoc.SelectNodes("//puzzle/cages/cage")?.Count;

            Assert.That(ex?.Message, Is.EqualTo($"Cage sum value {invalidSumValue} is invalid, must be 45 or lower."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithCageWithNoCells()
        {
            // Read in the empty Puzzle file to be modified.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(testPuzzleDir, EmptyPuzzleFileName));

            var cellsNode = xmlDoc.SelectSingleNode("//puzzle/cages/cage/cells")
                ?? throw new Exception("Failed to find cage cells node in test puzzle.");

            cellsNode.RemoveAll();

            // Write out modified puzzle.
            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            var numCages = xmlDoc.SelectNodes("//puzzle/cages/cage")?.Count;

            Assert.That(ex?.Message, Is.EqualTo("Cage sum value is invalid, must be 45 or lower."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithCageWithMoreThanNineCells()
        {
            // Read in the empty Puzzle file to be modified.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(testPuzzleDir, EmptyPuzzleFileName));

            var cellsNode = xmlDoc.SelectSingleNode("//puzzle/cages/cage/cells")
                ?? throw new Exception("Failed to find cage cells node in test puzzle.");

            // Create a cell.
            var newCellNode = xmlDoc.CreateElement("cell");
            var newCellIdNode = xmlDoc.CreateElement("id");
            newCellIdNode.InnerText = "82";

            newCellNode.AppendChild(newCellIdNode);

            cellsNode.AppendChild(newCellNode);

            // xmlDoc.AppendChild(cellsNode);

            // Write out modified puzzle.
            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            var numCages = xmlDoc.SelectNodes("//puzzle/cages/cage")?.Count;

            Assert.That(ex?.Message, Is.EqualTo("Failed to validate puzzle file against the schema."));
            Assert.That(ex?.InnerException?.Message, Is.EqualTo("The element 'cages' has incomplete content. List of possible elements expected: 'cage'."));
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_SuccessfullyCreatesPuzzle()
        {
            var testFile = Path.Combine(testPuzzleDir, "EasyPuzzle.ksud");

            var parser = new KillerSudokuParser();

            var puzzle = parser.ParsePuzzle(testFile);

            Assert.That(puzzle.Cells, Has.Count.EqualTo(81));

            // Make sure that we have sections for columns, rows, squares and cages.
            Assert.That(puzzle.Sections, Has.Count.EqualTo(61));

            // Assert that the cell coordinates are correct.
            var index = 0;

            for (var y = 0u; y < 9; ++y)
            {
                for (var x = 0u; x < 9; ++x)
                {
                    Assert.That(puzzle.Cells[index].Coordinate, Is.EqualTo(new Coordinate(x, y)));

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
                Assert.That(columnSections[i].PuzzleCells.All(c => c.Coordinate.Y == i));
                Assert.That(rowSections[i].PuzzleCells.All(c => c.Coordinate.X == i));
            }
        }

        private static XmlDocument CreateTestPuzzle(int numColumns = 9, int numRows = 9, int numCages = 9)
        {
            var xmlDoc = new XmlDocument();
            var puzzleNode = xmlDoc.CreateElement("puzzle");

            var gridNode = xmlDoc.CreateElement("grid");

            var cellsNode = xmlDoc.CreateElement("cells");

            // Create the cells.
            var id = 0;
            for (int column = 0; column < numColumns; ++column)
            {
                for (int row = 0; row < numRows; ++row)
                {
                    var cellNode = xmlDoc.CreateElement("cell");
                    var cellIdNode = xmlDoc.CreateElement("id");
                    cellIdNode.InnerText = id.ToString();
                    var cellXNode = xmlDoc.CreateElement("x");
                    cellXNode.InnerText = row.ToString();
                    var cellYNode = xmlDoc.CreateElement("y");
                    cellYNode.InnerText = column.ToString();
                    var cellValueNode = xmlDoc.CreateElement("value");

                    cellNode.AppendChild(cellIdNode);
                    cellNode.AppendChild(cellXNode);
                    cellNode.AppendChild(cellYNode);
                    cellNode.AppendChild(cellValueNode);

                    cellsNode.AppendChild(cellNode);

                    ++id;
                }
            }

            gridNode.AppendChild(cellsNode);

            puzzleNode.AppendChild(gridNode);

            // Create a simple cage.
            var cagesNode = xmlDoc.CreateElement("cages");

            var cageNode = xmlDoc.CreateElement("cage");

            var cageSumNode = xmlDoc.CreateElement("sum");
            cageSumNode.InnerText = "3";

            var cageCellsNode = xmlDoc.CreateElement("cells");

            for (int i = 0; i < 2; ++i)
            {
                var cageCellNode = xmlDoc.CreateElement("cell");

                var cageCellIdNode = xmlDoc.CreateElement("id");
                cageCellIdNode.InnerText = i.ToString();

                cageCellNode.AppendChild(cageCellIdNode);

                cageCellsNode.AppendChild(cageCellNode);
            }

            cageNode.AppendChild(cageSumNode);
            cageNode.AppendChild(cageCellsNode);

            cagesNode.AppendChild(cageNode);

            puzzleNode.AppendChild(cagesNode);

            xmlDoc.AppendChild(puzzleNode);

            return xmlDoc;
        }
    }
}
