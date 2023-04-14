using GridPuzzleSolver;
using GridPuzzleSolver.Solvers.KillerSudokuSolver.Parser;
using NUnit.Framework;
using System.Xml;

namespace GridPuzzleSolverUnitTests.Solvers.KillerSudokuSolver.Parser
{
    [TestFixture]
    public class KillerSudokuParserUnitTests
    {
        private const string TestPuzzleFileName = "TestPuzzle.ksud";

        private readonly string TestPuzzleDir = Path.Combine("TestPuzzles", "Sudoku");

        [SetUp]
        public void BaseSetUp()
        {
            // If the test file already exists fail the unit test.
            Assert.IsFalse(File.Exists(TestPuzzleFileName), $"Test file {TestPuzzleFileName} already exists");
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

            Assert.AreEqual("Unable to find puzzle file.", ex.Message);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.AreEqual("Invalid file type, expected .ksud. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(fileName);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Puzzle file is empty. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleFileThatDoesNotMatchExpectedSchema()
        {
            // Create the bare minimum file without the puzzle section.
            var xmlDoc = new XmlDocument();

            var cellsNode = xmlDoc.CreateElement("cells");

            // Create a cells.
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

            Assert.AreEqual("Failed to validate puzzle file against the schema.", ex.Message);
        }

        [Test]
        public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfCells()
        {
            var xmlDoc = new XmlDocument();
            var puzzleNode = xmlDoc.CreateElement("puzzle");

            var gridNode = xmlDoc.CreateElement("grid");

            var cellsNode = xmlDoc.CreateElement("cells");

            // Create the cells.
            var id = 0;
            for (int column = 0; column < 8; ++column)
            {
                for (int row = 0; row < 9; ++row)
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

            xmlDoc.Save(TestPuzzleFileName);

            var parser = new KillerSudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual($"Puzzle only contains {id + 1}, expected 81.", ex.Message);
        }
    }
}
