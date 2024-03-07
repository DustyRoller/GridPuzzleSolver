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

            Assert.That("Unable to find puzzle file.", Is.EqualTo(ex?.Message));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithInvalidFileExtension()
        {
            var fileName = "test.fdg";

            File.Create(fileName).Close();

            var parser = new SudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(fileName));

            Assert.That("Invalid file type, expected .sud. (Parameter 'puzzleFilePath')", Is.EqualTo(ex?.Message));

            File.Delete(fileName);
        }

        [Test]
        public void SudokuParser_ParsePuzzle_FailsWithEmptyFile()
        {
            File.Create(TestPuzzleFileName).Close();

            var parser = new SudokuParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ParsePuzzle(TestPuzzleFileName));

            Assert.That("Puzzle file is empty. (Parameter 'puzzleFilePath')", Is.EqualTo(ex?.Message));
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

            Assert.That("Not all rows have 9 columns.", Is.EqualTo(ex?.Message));
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

            Assert.That("Puzzle does not have 9 rows.", Is.EqualTo(ex?.Message));
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

            Assert.That("Failed to parse cell value: ?", Is.EqualTo(ex?.Message));
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

            Assert.That("Puzzle contains no solved cells", Is.EqualTo(ex?.Message));
        }

        [Test]
        public void SudokuParser_ParsePuzzle_Successful()
        {
            var testFile = Path.Combine(testPuzzleDir, "EasyPuzzle.sud");

            Assert.That(File.Exists(testFile));

            var parser = new SudokuParser();
            var puzzle = parser.ParsePuzzle(testFile);

            Assert.That(81, Is.EqualTo(puzzle.Cells.Count));
            Assert.That(27, Is.EqualTo(puzzle.Sections.Count));

            // Assert that the cell coordinates are correct.
            var index = 0;

            for (var y = 0u; y < 9; ++y)
            {
                for (var x = 0u; x < 9; ++x)
                {
                    Assert.That(new Coordinate(x, y), Is.EqualTo(puzzle.Cells[index].Coordinate));

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

            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[0]).CellValue));
            Assert.That(4u, Is.EqualTo(((PuzzleCell)puzzle.Cells[1]).CellValue));
            Assert.That(2u, Is.EqualTo(((PuzzleCell)puzzle.Cells[2]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[3]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[4]).CellValue));
            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[5]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[6]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[7]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[8]).CellValue));

            Assert.That(1u, Is.EqualTo(((PuzzleCell)puzzle.Cells[9]).CellValue));
            Assert.That(9u, Is.EqualTo(((PuzzleCell)puzzle.Cells[10]).CellValue));
            Assert.That(7u, Is.EqualTo(((PuzzleCell)puzzle.Cells[11]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[12]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[13]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[14]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[15]).CellValue));
            Assert.That(4u, Is.EqualTo(((PuzzleCell)puzzle.Cells[16]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[17]).CellValue));

            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[18]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[19]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[20]).CellValue));
            Assert.That(4u, Is.EqualTo(((PuzzleCell)puzzle.Cells[21]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[22]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[23]).CellValue));
            Assert.That(1u, Is.EqualTo(((PuzzleCell)puzzle.Cells[24]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[25]).CellValue));
            Assert.That(9u, Is.EqualTo(((PuzzleCell)puzzle.Cells[26]).CellValue));

            Assert.That(8u, Is.EqualTo(((PuzzleCell)puzzle.Cells[27]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[28]).CellValue));
            Assert.That(1u, Is.EqualTo(((PuzzleCell)puzzle.Cells[29]).CellValue));
            Assert.That(3u, Is.EqualTo(((PuzzleCell)puzzle.Cells[30]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[31]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[32]).CellValue));
            Assert.That(2u, Is.EqualTo(((PuzzleCell)puzzle.Cells[33]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[34]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[35]).CellValue));

            Assert.That(9u, Is.EqualTo(((PuzzleCell)puzzle.Cells[36]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[37]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[38]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[39]).CellValue));
            Assert.That(7u, Is.EqualTo(((PuzzleCell)puzzle.Cells[40]).CellValue));
            Assert.That(1u, Is.EqualTo(((PuzzleCell)puzzle.Cells[41]).CellValue));
            Assert.That(4u, Is.EqualTo(((PuzzleCell)puzzle.Cells[42]).CellValue));
            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[43]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[44]).CellValue));

            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[45]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[46]).CellValue));
            Assert.That(3u, Is.EqualTo(((PuzzleCell)puzzle.Cells[47]).CellValue));
            Assert.That(2u, Is.EqualTo(((PuzzleCell)puzzle.Cells[48]).CellValue));
            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[49]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[50]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[51]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[52]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[53]).CellValue));

            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[54]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[55]).CellValue));
            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[56]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[57]).CellValue));
            Assert.That(3u, Is.EqualTo(((PuzzleCell)puzzle.Cells[58]).CellValue));
            Assert.That(2u, Is.EqualTo(((PuzzleCell)puzzle.Cells[59]).CellValue));
            Assert.That(7u, Is.EqualTo(((PuzzleCell)puzzle.Cells[60]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[61]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[62]).CellValue));

            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[63]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[64]).CellValue));
            Assert.That(4u, Is.EqualTo(((PuzzleCell)puzzle.Cells[65]).CellValue));
            Assert.That(5u, Is.EqualTo(((PuzzleCell)puzzle.Cells[66]).CellValue));
            Assert.That(9u, Is.EqualTo(((PuzzleCell)puzzle.Cells[67]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[68]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[69]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[70]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[71]).CellValue));

            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[72]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[73]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[74]).CellValue));
            Assert.That(7u, Is.EqualTo(((PuzzleCell)puzzle.Cells[75]).CellValue));
            Assert.That(6u, Is.EqualTo(((PuzzleCell)puzzle.Cells[76]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[77]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[78]).CellValue));
            Assert.That(8u, Is.EqualTo(((PuzzleCell)puzzle.Cells[79]).CellValue));
            Assert.That(0u, Is.EqualTo(((PuzzleCell)puzzle.Cells[80]).CellValue));
        }
    }
}
