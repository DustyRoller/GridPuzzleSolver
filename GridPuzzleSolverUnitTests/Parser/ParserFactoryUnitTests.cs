using GridPuzzleSolver.Puzzles.Kakuro.Parser;
using GridPuzzleSolver.Puzzles.Sudoku.Parser;
using NUnit.Framework;

namespace GridPuzzleSolver.Parser.UnitTests
{
    [TestFixture]
    public class ParserFactoryUnitTests
    {
        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithNullFileName()
        {
// Converting null literal or possible null value to non-nullable type.
// Possible null reference argument.
#pragma warning disable CS8600, CS8604
            string inputFile = null;

            var ex = Assert.Throws<ArgumentException>(() => ParserFactory.GetParser(inputFile));
#pragma warning restore CS8604, CS8600

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file is null or empty. (Parameter 'puzzleFile')"));
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithEmptyFileName()
        {
            var inputFile = string.Empty;

            var ex = Assert.Throws<ArgumentException>(() => ParserFactory.GetParser(inputFile));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file is null or empty. (Parameter 'puzzleFile')"));
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithInvalidFileName()
        {
            var inputFile = "adodgyfilename";

            var ex = Assert.Throws<ArgumentException>(() => ParserFactory.GetParser(inputFile));

            Assert.That($"Failed to get file extension from puzzle file - {inputFile}.", Is.EqualTo(ex?.Message));
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithUnknownFileExtension()
        {
            var inputFile = "puzzlefile.txt";

            var ex = Assert.Throws<ParserException>(() => ParserFactory.GetParser(inputFile));

            Assert.That($"File extension \'{Path.GetExtension(inputFile)}\' not recognised.", Is.EqualTo(ex?.Message));
        }

        [TestCase]
        public void ParserFactory_GetParser_ReturnsKakuroParser()
        {
            var inputFile = "puzzlefile.kak";

            var parser = ParserFactory.GetParser(inputFile);

            Assert.That(parser, Is.InstanceOf(typeof(KakuroParser)));
        }

        [TestCase]
        public void ParserFactory_GetParser_ReturnsSudokuParser()
        {
            var inputFile = "puzzlefile.sud";

            var parser = ParserFactory.GetParser(inputFile);

            Assert.That(parser, Is.InstanceOf(typeof(SudokuParser)));
        }
    }
}
