using GridPuzzleSolver;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Puzzles.Kakuro;
using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using NUnit.Framework;
using System.Text;

namespace GridPuzzleSolverUnitTests.Puzzles.Kakuro.Parser
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

            Assert.That(ex?.Message, Is.EqualTo("Unable to find puzzle file."));
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new KakuroParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.That(ex?.Message, Is.EqualTo("Invalid file type, expected .kak. (Parameter 'puzzleFilePath')"));

            File.Delete(fileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new KakuroParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file is empty. (Parameter 'puzzleFilePath')"));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_FailsWithPuzzleWithWidthOfOneCell()
        {
            var sb = new StringBuilder();

            // First line has 3 columns, second has 4 columns.
            sb.AppendLine("|  x  |");
            sb.AppendLine("|  x  |");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new KakuroParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle must be at least two cells wide."));

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

            Assert.That(ex?.Message, Is.EqualTo("Mismatch in row width on row 2."));

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

            Assert.That(ex?.Message, Is.EqualTo("Found invalid cell data: ?."));

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void KakuroParser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(testPuzzleDir, "Easy4x4Puzzle.kak");

            Assert.That(File.Exists(testFile));

            var parser = new KakuroParser();
            var puzzle = (KakuroPuzzle)parser.ParsePuzzle(testFile);

            Assert.That(puzzle.AllCells, Has.Count.EqualTo(25));
            Assert.That(puzzle.Cells, Has.Count.EqualTo(10));
            Assert.That(puzzle.Width, Is.EqualTo(5u));
            Assert.That(puzzle.Height, Is.EqualTo(5u));

            // Assert that the cell's coordinates are correct.
            var index = 0;

            for (var y = 0u; y < puzzle.Height; ++y)
            {
                for (var x = 0u; x < puzzle.Width; ++x)
                {
                    var expectedCoordinates = new Coordinates
                    {
                        X = x,
                        Y = y,
                    };

                    Assert.That(puzzle.AllCells[index].Coordinates, Is.EqualTo(expectedCoordinates));

                    index++;
                }
            }

            Assert.That(puzzle.AllCells[0], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.AllCells[1]).ColumnClue, Is.EqualTo(17u));
            Assert.That(((ClueCell)puzzle.AllCells[2]).ColumnClue, Is.EqualTo(24u));
            Assert.That(puzzle.AllCells[3], Is.InstanceOf<BlankCell>());
            Assert.That(puzzle.AllCells[4], Is.InstanceOf<BlankCell>());

            Assert.That(((ClueCell)puzzle.AllCells[5]).RowClue, Is.EqualTo(16u));
            Assert.That(puzzle.AllCells[6], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[7], Is.InstanceOf<PuzzleCell>());
            Assert.That(((ClueCell)puzzle.AllCells[8]).ColumnClue, Is.EqualTo(20u));
            Assert.That(puzzle.AllCells[9], Is.InstanceOf<BlankCell>());

            Assert.That(((ClueCell)puzzle.AllCells[10]).RowClue, Is.EqualTo(23u));
            Assert.That(puzzle.AllCells[11], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[12], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[13], Is.InstanceOf<PuzzleCell>());
            Assert.That(((ClueCell)puzzle.AllCells[14]).ColumnClue, Is.EqualTo(15u));

            Assert.That(puzzle.AllCells[15], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.AllCells[16]).RowClue, Is.EqualTo(23u));
            Assert.That(puzzle.AllCells[17], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[18], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[19], Is.InstanceOf<PuzzleCell>());

            Assert.That(puzzle.AllCells[20], Is.InstanceOf<BlankCell>());
            Assert.That(puzzle.AllCells[21], Is.InstanceOf<BlankCell>());
            Assert.That(((ClueCell)puzzle.AllCells[22]).RowClue, Is.EqualTo(14u));
            Assert.That(puzzle.AllCells[13], Is.InstanceOf<PuzzleCell>());
            Assert.That(puzzle.AllCells[24], Is.InstanceOf<PuzzleCell>());
        }
    }
}
