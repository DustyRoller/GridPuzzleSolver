using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;
using System.Text;

namespace GridPuzzleSolver.Solvers.KakuroSolver.Parser.UnitTests
{
    [TestFixture]
    public class KakuroParserUnitTests
    {
        private const string TestPuzzleFileName = "TestPuzzle.kak";

        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "Kakuro");

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithNonExistantFile()
        {
            var parser = new KakuroParser();
            var ex = Assert.Throws<FileNotFoundException>(() => parser.ParsePuzzle("randomfile"));

            Assert.That("Unable to find puzzle file.", Is.EqualTo(ex?.Message));
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new KakuroParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.That("Invalid file type, expected .kak. (Parameter 'puzzleFilePath')", Is.EqualTo(ex?.Message));

            File.Delete(fileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new KakuroParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That("Puzzle file is empty. (Parameter 'puzzleFilePath')", Is.EqualTo(ex?.Message));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithPuzzleWithDifferentColumnLengths()
        {
            var sb = new StringBuilder();

            // First line has 3 columns, second has 4 columns.
            sb.AppendLine("|  x  |  x  |  x  |");
            sb.AppendLine("|  x  |  x  |  x  |  x  |");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new KakuroParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That("Mismatch in row width on row 2.", Is.EqualTo(ex?.Message));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithPuzzleWithInvalidCharacters()
        {
            var sb = new StringBuilder();

            sb.AppendLine("|  x  |  x  |  x  |");
            sb.AppendLine("|  x  |?|  x  |");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new KakuroParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That("Found invalid cell data: ?.", Is.EqualTo(ex?.Message));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(testPuzzleDir, "Easy4x4Puzzle.kak");

            Assert.That(File.Exists(testFile));

            var parser = new KakuroParser();
            var puzzle = parser.ParsePuzzle(testFile);

            Assert.That(25, Is.EqualTo(puzzle.Cells.Count));
            Assert.That(5u, Is.EqualTo(puzzle.Width));
            Assert.That(5u, Is.EqualTo(puzzle.Height));

            // Assert that the cell coordinates are correct.
            var index = 0;

            for (var y = 0u; y < puzzle.Height; ++y)
            {
                for (var x = 0u; x < puzzle.Width; ++x)
                {
                    Assert.That(new Coordinate(x, y), Is.EqualTo(puzzle.Cells[index].Coordinate));

                    index++;
                }
            }

            Assert.That(puzzle.Cells[0], Is.InstanceOf(typeof(BlankCell)));
            Assert.That(17u, Is.EqualTo(((ClueCell)puzzle.Cells[1]).ColumnClue));
            Assert.That(24u, Is.EqualTo(((ClueCell)puzzle.Cells[2]).ColumnClue));
            Assert.That(puzzle.Cells[3], Is.InstanceOf(typeof(BlankCell)));
            Assert.That(puzzle.Cells[4], Is.InstanceOf(typeof(BlankCell)));

            Assert.That(16u, Is.EqualTo(((ClueCell)puzzle.Cells[5]).RowClue));
            Assert.That(puzzle.Cells[6], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[7], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(20u, Is.EqualTo(((ClueCell)puzzle.Cells[8]).ColumnClue));
            Assert.That(puzzle.Cells[9], Is.InstanceOf(typeof(BlankCell)));

            Assert.That(23u, Is.EqualTo(((ClueCell)puzzle.Cells[10]).RowClue));
            Assert.That(puzzle.Cells[11], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[12], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[13], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(15u, Is.EqualTo(((ClueCell)puzzle.Cells[14]).ColumnClue));

            Assert.That(puzzle.Cells[15], Is.InstanceOf(typeof(BlankCell)));
            Assert.That(23u, Is.EqualTo(((ClueCell)puzzle.Cells[16]).RowClue));
            Assert.That(puzzle.Cells[17], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[18], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[19], Is.InstanceOf(typeof(PuzzleCell)));

            Assert.That(puzzle.Cells[20], Is.InstanceOf(typeof(BlankCell)));
            Assert.That(puzzle.Cells[21], Is.InstanceOf(typeof(BlankCell)));
            Assert.That(14u, Is.EqualTo(((ClueCell)puzzle.Cells[22]).RowClue));
            Assert.That(puzzle.Cells[13], Is.InstanceOf(typeof(PuzzleCell)));
            Assert.That(puzzle.Cells[24], Is.InstanceOf(typeof(PuzzleCell)));
        }
    }
}
