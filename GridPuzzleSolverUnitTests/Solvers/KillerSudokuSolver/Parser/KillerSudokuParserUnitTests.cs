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

        //[Test]
        //public void KillerSudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfColumns()
        //{
        //    var xmlDoc = new XmlDocument();
        //    var puzzleNode = xmlDoc.CreateElement("puzzle");
        //    xmlDoc.AppendChild(puzzleNode);

        //    var cellsNode = xmlDoc.CreateElement("cells");

        //    // Create the cells.
        //    var id = 0;
        //    for (int column = 0; column < 8; ++column)
        //    {
        //        for (int row = 0; row < 9; ++row)               
        //        {
        //            var cellNode = xmlDoc.CreateElement("cell");
        //            var cellIdNode = xmlDoc.CreateElement("id");
        //            cellIdNode.InnerText = id.ToString();
        //            var cellXNode = xmlDoc.CreateElement("x");
        //            cellXNode.InnerText = row.ToString();
        //            var cellYNode = xmlDoc.CreateElement("y");
        //            cellYNode.InnerText = column.ToString();

        //            cellNode.AppendChild(cellIdNode);
        //            cellNode.AppendChild(cellXNode);
        //            cellNode.AppendChild(cellYNode);

        //            cellsNode.AppendChild(cellNode);

        //            ++id;
        //        }
        //    }

        //    puzzleNode.AppendChild(cellsNode);

        //    xmlDoc.Save(TestPuzzleFileName);

        //    var parser = new KillerSudokuParser();
        //    var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

        //    Assert.AreEqual("Not all rows have 9 columns.", ex.Message);
        //}
    }
}
