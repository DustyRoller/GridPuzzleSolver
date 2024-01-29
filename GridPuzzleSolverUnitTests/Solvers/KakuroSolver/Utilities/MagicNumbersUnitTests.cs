using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.KakuroSolver.Utilities.UnitTests
{
    [TestFixture]
    public class MagicNumbersUnitTests
    {
        [Test]
        public void MagicNumbers_ValidateMagicNumbers()
        {
            // To ensure no typos were made double check that the magic number
            // values add up to the correct value. Do this in a loop so that we
            // can determine which value was wrong, if any.
            foreach (var mn in MagicNumbers.MagicNumberValues)
            {
                Assert.IsTrue(mn.Values[0].Sum(v => v) == mn.Total, $"{mn.Total} - {mn.PartitionLength}");
            }
        }

        [Test]
        public void MagicNumbers_ValuesAreImmutable()
        {
            var mn = MagicNumbers.MagicNumberValues[0];

            var originalLength = mn.Values[0].Count;

            mn.Values[0].RemoveAt(0);

            // Make sure the number of values hasn't actually changed.
            Assert.AreEqual(originalLength, mn.Values[0].Count);
        }
    }
}
