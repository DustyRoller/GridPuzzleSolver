using GridPuzzleSolver.Solvers.KakuroSolver.Parser;
using GridPuzzleSolver.Solvers.SudokuSolver.Parser;
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

            Assert.AreEqual("Puzzle file is null or empty. (Parameter 'puzzleFile')", ex?.Message);
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithEmptyFileName()
        {
            var inputFile = string.Empty;

            var ex = Assert.Throws<ArgumentException>(() => ParserFactory.GetParser(inputFile));

            Assert.AreEqual("Puzzle file is null or empty. (Parameter 'puzzleFile')", ex?.Message);
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithInvalidFileName()
        {
            var inputFile = "adodgyfilename";

            var ex = Assert.Throws<ArgumentException>(() => ParserFactory.GetParser(inputFile));

            Assert.AreEqual($"Failed to get file extension from puzzle file - {inputFile}.", ex?.Message);
        }

        [TestCase]
        public void ParserFactory_GetParser_ThrowsExceptionWithUnknownFileExtension()
        {
            var inputFile = "puzzlefile.txt";

            var ex = Assert.Throws<ParserException>(() => ParserFactory.GetParser(inputFile));

            Assert.AreEqual($"File extension \'{Path.GetExtension(inputFile)}\' not recognised.", ex?.Message);
        }

        [TestCase]
        public void ParserFactory_GetParser_ReturnsKakuroParser()
        {
            var inputFile = "puzzlefile.kak";

            var parser = ParserFactory.GetParser(inputFile);

            Assert.IsInstanceOf(typeof(KakuroParser), parser);
        }

        [TestCase]
        public void ParserFactory_GetParser_ReturnsSudokuParser()
        {
            var inputFile = "puzzlefile.sud";

            var parser = ParserFactory.GetParser(inputFile);

            Assert.IsInstanceOf(typeof(SudokuParser), parser);
        }
    }
}
