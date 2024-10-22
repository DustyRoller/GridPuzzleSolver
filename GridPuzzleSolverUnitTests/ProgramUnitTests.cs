using NUnit.Framework;

namespace GridPuzzleSolver.UnitTests
{
    [TestFixture]
    public class ProgramUnitTests
    {
        [TestCase]
        public void Program_Run_ThrowsExceptionWithNullFileName()
        {
            // Converting null literal or possible null value to non-nullable type.
            // Possible null reference argument.
#pragma warning disable CS8600, CS8604
            string inputFile = null;

            var ex = Assert.Throws<ArgumentException>(() => Program.Run(inputFile));
#pragma warning restore CS8604, CS8600

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file path is null or empty. (Parameter 'puzzleFilePath')"));
        }

        [TestCase]
        public void Program_Run_ThrowsExceptionWithEmptyFileName()
        {
            var inputFile = string.Empty;

            var ex = Assert.Throws<ArgumentException>(() => Program.Run(inputFile));

            Assert.That(ex?.Message, Is.EqualTo("Puzzle file path is null or empty. (Parameter 'puzzleFilePath')"));
        }

        [TestCase]
        public void Program_Run_ThrowsExceptionWithInvalidFileName()
        {
            var inputFile = "adodgyfilename";

            var ex = Assert.Throws<ArgumentException>(() => Program.Run(inputFile));

            Assert.That($"Failed to get file extension from puzzle file - {inputFile}.", Is.EqualTo(ex?.Message));
        }
    }
}