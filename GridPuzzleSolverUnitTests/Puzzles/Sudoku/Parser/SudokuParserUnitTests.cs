using GridPuzzleSolver.Components.Cells;
using NUnit.Framework;
using System.Text;

namespace GridPuzzleSolver.Puzzles.Sudoku.Parser.UnitTests
{
    [TestFixture]
    public class SudokuParserUnitTests
    {
        private const string TestPuzzleFileName = "TestPuzzle.sud";

        private readonly string testPuzzleDir = Path.Combine("TestPuzzles", "Sudoku");

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
        public void SudokuParser_ParsePuzzle_FailsWithNonExistantFile()
        {
            var parser = new SudokuParser();
            var ex = Assert.Throws<FileNotFoundException>(() => parser.ParsePuzzle("randomfile"));

            Assert.That(ex?.Message, Is.EqualTo("Unable to find puzzle file."));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new SudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.That(ex?.Message, Is.EqualTo("Invalid file type, expected .sud. (Parameter 'puzzleFilePath')"));

            File.Delete(fileName);
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new SudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file is empty. (Parameter 'puzzleFilePath')"));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfColumns()
        {
            var sb = new StringBuilder();

            // Create puzzle with 8 columns.
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new SudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Not all rows have 9 columns."));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidNumberOfRows()
        {
            var sb = new StringBuilder();

            // Create puzzle with 8 rows.
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new SudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle does not have 9 rows."));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithPuzzleWithInvalidCharacters()
        {
            var sb = new StringBuilder();

            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|?|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new SudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Failed to parse cell value: ?"));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithPuzzleWithNoSolvedCells()
        {
            var sb = new StringBuilder();

            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");
            sb.AppendLine("|-|-|-|-|-|-|-|-|-|");

            File.WriteAllText(TestPuzzleFileName, sb.ToString());

            var parser = new SudokuParser();
            var ex = Assert.Throws<ParserException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle contains no solved cells"));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(testPuzzleDir, "EasyPuzzle.sud");

            Assert.That(File.Exists(testFile));

            var parser = new SudokuParser();
            var puzzle = parser.ParsePuzzle(testFile);

            Assert.That(puzzle.Cells, Has.Count.EqualTo(81));
            Assert.That(puzzle.Sections, Has.Count.EqualTo(27));

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
