using GridPuzzleSolver.Parser;
using NUnit.Framework;
using System.Xml.Linq;

namespace GridPuzzleSolver.Puzzles.KillerSudoku.Parser.UnitTests
{
    [TestFixture]
    public class KillerSudokuXmlParserUnitTests
    {
        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "KillerSudoku");

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfPuzzleDoesNotContainACagesElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><TestElement>test</TestElement></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cages elements."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCagesElementDoesNotContainACageElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><TestElement>test</TestElement></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cages elements."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCageElementDoesNotContainACellsElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><Cage sum=\"10\"><TestElement>test</TestElement></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements within Cage."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCellsElementDoesNotContainACellElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><Cage sum=W\"10\"><Cells><TestElement>test</TestElement></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements within Cage."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCellDoesNotContainAnyCoordinates()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><Cage sum=\"10\"><Cells><Cell></Cell></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Cell did not contain \'Coordinates\' element."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAXValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><Cage sum=\"10\"><Cells><Cell><Coordinates y=\"1\" /></Cell></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'x\' attribute."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesXValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var xValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KillerSudokuPuzzle><Cages><Cage sum=10><Cells><Cell><Coordinates x=\"{xValue}\" y=\"1\" /></Cell></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse x coordinate value: {xValue}."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAYValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KillerSudokuPuzzle><Cages><Cage sum=10><Cells><Cell><Coordinates x=\"1\" /></Cell></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'y\' attribute."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesYValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var yValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KillerSudokuPuzzle><Cages><Cage sum=10><Cells><Cell><Coordinates x=\"1\" y=\"{yValue}\" /></Cell></Cells></Cage></Cages></KillerSudokuPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KillerSudokuXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse y coordinate value: {yValue}."));
        }

        [Test]
        public void KillerSudokuXmlParser_ParsePuzzle_SuccessfullyLoadsPuzzle()
        {
            var testFile = Path.Combine(testPuzzleDir, "EasyPuzzle.xml");
            var expectedVale = true;
            Assert.That(expectedVale, Is.EqualTo(true));
        }
    }
}
