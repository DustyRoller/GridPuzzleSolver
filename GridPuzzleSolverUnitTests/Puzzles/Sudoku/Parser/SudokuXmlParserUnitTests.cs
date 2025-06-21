using GridPuzzleSolver;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Puzzles.Sudoku.Parser;
using NUnit.Framework;
using System.Xml.Linq;

namespace GridPuzzleSolverUnitTests.Puzzles.Sudoku.Parser
{
    [TestFixture]
    public class SudokuXmlParserUnitTests
    {
        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "Sudoku");

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfPuzzleDoesNotContainACellsElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><TestElement>test</TestElement></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIPuzzleDoesNotContainAnyCellElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><Cells><TestElement>test</TestElement></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCellDoesNotContainAnyCoordinates()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><Cells><Cell></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Cell did not contain \'Coordinates\' element."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAXValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><Cells><Cell><Coordinates y=\"1\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'x\' attribute."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesXValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var xValue = "notanint";
            var xmlDocument = XDocument.Parse($"<SudokuPuzzle><Cells><Cell><Coordinates x=\"{xValue}\" y=\"1\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse x coordinate value: {xValue}."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAYValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><Cells><Cell><Coordinates x=\"1\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'y\' attribute."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesYValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var yValue = "notanint";
            var xmlDocument = XDocument.Parse($"<SudokuPuzzle><Cells><Cell><Coordinates x=\"1\" y=\"{yValue}\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse y coordinate value: {yValue}."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCellValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var cellValue = "notanint";
            var xmlDocument = XDocument.Parse($"<SudokuPuzzle><Cells><Cell value=\"{cellValue}\"><Coordinates x=\"1\" y=\"1\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse cell value: {cellValue}."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCellValueIsGreaterThan9()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<SudokuPuzzle><Cells><Cell value=\"10\"><Coordinates x=\"1\" y=\"1\" /></Cell></Cells></SudokuPuzzle>");

            var ex = Assert.Throws<GridPuzzleSolverException>(() => SudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle cell value cannot be greater than 9. 1,1."));
        }

        [Test]
        public void SudokuXmlParser_ParsePuzzle_SuccessfullyLoadsPuzzle()
        {
            var testFile = Path.Combine(testPuzzleDir, "EasyPuzzle.xml");

            Assert.That(File.Exists(testFile));

            // Load the test puzzle.
            var xmlDocument = XDocument.Load(testFile);

            var puzzle = SudokuXmlParser.ParsePuzzle(xmlDocument);

            Assert.That(puzzle.Cells, Has.Count.EqualTo(81));

            // Assert that the cell's coordinates are correct.
            var index = 0;

            for (var x = 0u; x < 9; ++x)
            {
                for (var y = 0u; y < 9; ++y)
                {
                    Assert.That(puzzle.Cells[index].Coordinates.X, Is.EqualTo(x));
                    Assert.That(puzzle.Cells[index].Coordinates.Y, Is.EqualTo(y));

                    index++;
                }
            }

            Assert.That(((PuzzleCell)puzzle.Cells[0]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[1]).CellValue, Is.EqualTo(4u));
            Assert.That(((PuzzleCell)puzzle.Cells[2]).CellValue, Is.EqualTo(2u));
            Assert.That(((PuzzleCell)puzzle.Cells[3]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[4]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[5]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[6]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[7]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[8]).CellValue, Is.EqualTo(6u));

            Assert.That(((PuzzleCell)puzzle.Cells[9]).CellValue, Is.EqualTo(1u));
            Assert.That(((PuzzleCell)puzzle.Cells[10]).CellValue, Is.EqualTo(9u));
            Assert.That(((PuzzleCell)puzzle.Cells[11]).CellValue, Is.EqualTo(7u));
            Assert.That(((PuzzleCell)puzzle.Cells[12]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[13]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[14]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[15]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[16]).CellValue, Is.EqualTo(4u));
            Assert.That(((PuzzleCell)puzzle.Cells[17]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[18]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[19]).CellValue, Is.EqualTo(6u));
            Assert.That(((PuzzleCell)puzzle.Cells[20]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[21]).CellValue, Is.EqualTo(4u));
            Assert.That(((PuzzleCell)puzzle.Cells[22]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[23]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[24]).CellValue, Is.EqualTo(1u));
            Assert.That(((PuzzleCell)puzzle.Cells[25]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[26]).CellValue, Is.EqualTo(9u));

            Assert.That(((PuzzleCell)puzzle.Cells[27]).CellValue, Is.EqualTo(8u));
            Assert.That(((PuzzleCell)puzzle.Cells[28]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[29]).CellValue, Is.EqualTo(1u));
            Assert.That(((PuzzleCell)puzzle.Cells[30]).CellValue, Is.EqualTo(3u));
            Assert.That(((PuzzleCell)puzzle.Cells[31]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[32]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[33]).CellValue, Is.EqualTo(2u));
            Assert.That(((PuzzleCell)puzzle.Cells[34]).CellValue, Is.EqualTo(6u));
            Assert.That(((PuzzleCell)puzzle.Cells[35]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[36]).CellValue, Is.EqualTo(9u));
            Assert.That(((PuzzleCell)puzzle.Cells[37]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[38]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[39]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[40]).CellValue, Is.EqualTo(7u));
            Assert.That(((PuzzleCell)puzzle.Cells[41]).CellValue, Is.EqualTo(1u));
            Assert.That(((PuzzleCell)puzzle.Cells[42]).CellValue, Is.EqualTo(4u));
            Assert.That(((PuzzleCell)puzzle.Cells[43]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[44]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[45]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[46]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[47]).CellValue, Is.EqualTo(3u));
            Assert.That(((PuzzleCell)puzzle.Cells[48]).CellValue, Is.EqualTo(2u));
            Assert.That(((PuzzleCell)puzzle.Cells[49]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[50]).CellValue, Is.EqualTo(6u));
            Assert.That(((PuzzleCell)puzzle.Cells[51]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[52]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[53]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[54]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[55]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[56]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[57]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[58]).CellValue, Is.EqualTo(3u));
            Assert.That(((PuzzleCell)puzzle.Cells[59]).CellValue, Is.EqualTo(2u));
            Assert.That(((PuzzleCell)puzzle.Cells[60]).CellValue, Is.EqualTo(7u));
            Assert.That(((PuzzleCell)puzzle.Cells[61]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[62]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[63]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[64]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[65]).CellValue, Is.EqualTo(4u));
            Assert.That(((PuzzleCell)puzzle.Cells[66]).CellValue, Is.EqualTo(5u));
            Assert.That(((PuzzleCell)puzzle.Cells[67]).CellValue, Is.EqualTo(9u));
            Assert.That(((PuzzleCell)puzzle.Cells[68]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[69]).CellValue, Is.EqualTo(6u));
            Assert.That(((PuzzleCell)puzzle.Cells[70]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[71]).CellValue, Is.EqualTo(0u));

            Assert.That(((PuzzleCell)puzzle.Cells[72]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[73]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[74]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[75]).CellValue, Is.EqualTo(7u));
            Assert.That(((PuzzleCell)puzzle.Cells[76]).CellValue, Is.EqualTo(6u));
            Assert.That(((PuzzleCell)puzzle.Cells[77]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[78]).CellValue, Is.EqualTo(0u));
            Assert.That(((PuzzleCell)puzzle.Cells[79]).CellValue, Is.EqualTo(8u));
            Assert.That(((PuzzleCell)puzzle.Cells[80]).CellValue, Is.EqualTo(0u));
        }
    }
}
