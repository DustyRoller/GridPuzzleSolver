using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using NUnit.Framework;
using System.Xml.Linq;

namespace GridPuzzleSolverUnitTests.Puzzles.Kakuro.Parser
{
    [TestFixture]
    public class KakuroXmlParserUnitTests
    {
        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfPuzzleDoesNotContainACellsElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KakuroPuzzle><TestElement>test</TestElement></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIPuzzleDoesNotContainAnyCellElement()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KakuroPuzzle><Cells><TestElement>test</TestElement></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Failed to find Cells elements."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfCellDoesNotContainAnyCoordinates()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KakuroPuzzle><Cells><Cell></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Cell did not contain \'Coordinates\' element."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAXValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KakuroPuzzle><Cells><Cell><Coordinates y=\"1\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'x\' attribute."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesXValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var xValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KakuroPuzzle><Cells><Cell><Coordinates x=\"{xValue}\" y=\"1\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse x coordinate value: {xValue}."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesDoesNotContainAYValue()
        {
            // Create the test XDocument.
            var xmlDocument = XDocument.Parse("<KakuroPuzzle><Cells><Cell><Coordinates x=\"1\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo("Coordinate did not contain \'y\' attribute."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfCoordinatesYValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var yValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KakuroPuzzle><Cells><Cell><Coordinates x=\"1\" y=\"{yValue}\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse y coordinate value: {yValue}."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfColumnClueValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var columnClueValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KakuroPuzzle><Cells><Cell column_clue=\"{columnClueValue}\"><Coordinates x=\"1\" y=\"notanint\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse column_clue: {columnClueValue}."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionIfRowClueValueIsNotAnInteger()
        {
            // Create the test XDocument.
            var rowClueValue = "notanint";
            var xmlDocument = XDocument.Parse($"<KakuroPuzzle><Cells><Cell row_clue=\"{rowClueValue}\"><Coordinates x=\"1\" y=\"notanint\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse row_clue: {rowClueValue}."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_ThrowsExceptionCellHasUnknownAttribute()
        {
            // Create the test XDocument.
            var unknownAttribute = "unknown_attribute=\"attribute\"";
            var xmlDocument = XDocument.Parse($"<KakuroPuzzle><Cells><Cell {unknownAttribute}><Coordinates x=\"1\" y=\"notanint\" /></Cell></Cells></KakuroPuzzle>");

            var ex = Assert.Throws<ParserException>(() => KakuroXmlParser.ParsePuzzle(xmlDocument));

            Assert.That(ex?.Message, Is.EqualTo($"Cell has unrecognised attributes: {unknownAttribute}."));
        }

        [Test]
        public void KakuroXmlParser_ParsePuzzle_SuccessfullyLoadsPuzzle()
        {
            var testFile = Path.Combine(testPuzzleDir, "Easy4x4Puzzle.xml");

            Assert.That(File.Exists(testFile));

            // Load the test puzzle.
            var xmlDocument = XDocument.Load(testFile);

            var puzzle = KakuroXmlParser.ParsePuzzle(xmlDocument);

            Assert.That(puzzle.Height, Is.EqualTo(5u));
            Assert.That(puzzle.Width, Is.EqualTo(5u));
            Assert.That(puzzle.Cells, Has.Count.EqualTo(25));
            Assert.That(puzzle.Cells.OfType<PuzzleCell>().ToList(), Has.Count.EqualTo(10));

            // Assert that the cell's coordinates are correct.
            var index = 0;

            for (var x = 0u; x < puzzle.Height; ++x)
            {
                for (var y = 0u; y < puzzle.Width; ++y)
                {
                    Assert.That(puzzle.Cells[index].Coordinates.X, Is.EqualTo(x));
                    Assert.That(puzzle.Cells[index].Coordinates.Y, Is.EqualTo(y));

                    index++;
                }
            }

            Assert.That(puzzle.Cells[0], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.Cells[1]).ColumnClue, Is.EqualTo(17u));
            Assert.That(((ClueCell)puzzle.Cells[2]).ColumnClue, Is.EqualTo(24u));
            Assert.That(puzzle.Cells[3], Is.InstanceOf<BlankCell>());
            Assert.That(puzzle.Cells[4], Is.InstanceOf<BlankCell>());

            Assert.That(((ClueCell)puzzle.Cells[5]).RowClue, Is.EqualTo(16u));
            Assert.That(puzzle.Cells[6], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[7], Is.InstanceOf<PuzzleCell>());
            Assert.That(((ClueCell)puzzle.Cells[8]).ColumnClue, Is.EqualTo(20u));
            Assert.That(puzzle.Cells[9], Is.InstanceOf<BlankCell>());

            Assert.That(((ClueCell)puzzle.Cells[10]).RowClue, Is.EqualTo(23u));
            Assert.That(puzzle.Cells[11], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[12], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[13], Is.InstanceOf<PuzzleCell>());
            Assert.That(((ClueCell)puzzle.Cells[14]).ColumnClue, Is.EqualTo(15u));

            Assert.That(puzzle.Cells[15], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.Cells[16]).RowClue, Is.EqualTo(23u));
            Assert.That(puzzle.Cells[17], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[18], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[19], Is.InstanceOf<PuzzleCell>());

            Assert.That(puzzle.Cells[20], Is.InstanceOf<BlankCell>());
            Assert.That(puzzle.Cells[21], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.Cells[22]).RowClue, Is.EqualTo(14u));
            Assert.That(puzzle.Cells[13], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.Cells[24], Is.InstanceOf<PuzzleCell>());
        }
    }
}
