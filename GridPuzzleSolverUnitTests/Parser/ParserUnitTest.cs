using GridPuzzleSolver.Cells;
using NUnit.Framework;
using System.Text;

namespace GridPuzzleSolver.Parser.UnitTests
{
    [TestFixture]
    public class ParserUnitTests
    {
        private const string TestPuzzleFileName = "TestPuzzle.txt";

        private const string TestPuzzleDir = "TestPuzzles";

        [Test]
        public void Parser_ParsePuzzle_FailsWithNonExistantFile()
        {
            var ex = Assert.Throws<FileNotFoundException>(() => Parser.ParsePuzzle("randomfile"));

            Assert.AreEqual("Unable to find puzzle file.", ex.Message);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var ex = Assert.Throws<ArgumentException>(() => Parser.ParsePuzzle(fileName));

            Assert.AreEqual("Invalid file type, expected .txt. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(fileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var ex = Assert.Throws<ArgumentException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Puzzle file is empty. (Parameter 'puzzleFilePath')", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithPuzzleWithDifferentColumnLengths()
        {
            var sb = new StringBuilder();

            // First line has 3 columns, second has 4 columns.
            sb.AppendLine("|  x  |  x  |  x  |");
            sb.AppendLine("|  x  |  x  |  x  |  x  |");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var ex = Assert.Throws<ParserException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Mismatch in row width on row 2.", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_FailsWithPuzzleWithInvalidCharacters()
        {
            var sb = new StringBuilder();

            sb.AppendLine("|  x  |  x  |  x  |");
            sb.AppendLine("|  x  |?|  x  |");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var ex = Assert.Throws<ParserException>(() => Parser.ParsePuzzle(TestPuzzleFileName));

            Assert.AreEqual("Found invalid cell data: ?.", ex.Message);

            File.Delete(TestPuzzleFileName);
        }

        [Test]
        public void Parser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(TestPuzzleDir, "Easy4x4Puzzle.txt");

            Assert.IsTrue(File.Exists(testFile));

            var puzzle = Parser.ParsePuzzle(testFile);

            Assert.AreEqual(25, puzzle.Cells.Count);
            Assert.AreEqual(5u, puzzle.Width);
            Assert.AreEqual(5u, puzzle.Height);

            // Assert that the cell coordinates are correct.
            var index = 0;

            for (var y = 0u; y < puzzle.Height; ++y)
            {
                for (var x = 0u; x < puzzle.Width; ++x)
                {
                    Assert.AreEqual(new Coordinate(x, y), puzzle.Cells[index].Coordinate);

                    index++;
                }
            }

            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[0]);
            Assert.AreEqual(17u, ((ClueCell)puzzle.Cells[1]).ColumnClue);
            Assert.AreEqual(24u, ((ClueCell)puzzle.Cells[2]).ColumnClue);
            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[3]);
            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[4]);

            Assert.AreEqual(16u, ((ClueCell)puzzle.Cells[5]).RowClue);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[6]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[7]);
            Assert.AreEqual(20u, ((ClueCell)puzzle.Cells[8]).ColumnClue);
            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[9]);

            Assert.AreEqual(23u, ((ClueCell)puzzle.Cells[10]).RowClue);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[11]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[12]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[13]);
            Assert.AreEqual(15u, ((ClueCell)puzzle.Cells[14]).ColumnClue);

            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[15]);
            Assert.AreEqual(23u, ((ClueCell)puzzle.Cells[16]).RowClue);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[17]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[18]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[19]);

            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[20]);
            Assert.IsInstanceOf(typeof(BlankCell), puzzle.Cells[21]);
            Assert.AreEqual(14u, ((ClueCell)puzzle.Cells[22]).RowClue);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[13]);
            Assert.IsInstanceOf(typeof(PuzzleCell), puzzle.Cells[24]);
        }
    }
}
