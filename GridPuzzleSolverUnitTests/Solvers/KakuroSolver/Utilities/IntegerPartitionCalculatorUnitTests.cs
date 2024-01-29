using NUnit.Framework;

namespace GridPuzzleSolver.Solvers.KakuroSolver.Utilities.UnitTests
{
    [TestFixture]
    public class IntegerPartitionCalculatorUnitTests
    {
        [TestCase(1u, 2u, 3u, 10u, "Maximum value cannot be greater than 9.")]
        [TestCase(5u, 2u, 3u, 5u, "Maximum value cannot be greater than or equal to sum.")]
        [TestCase(1u, 2u, 6u, 4u, "Maximum value cannot be greater than or equal to sum.")]
        [TestCase(10u, 2u, 3u, 2u, "Minimum value must be less than the maximum value.")]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ThrowsExceptionWithInvalidArgs(
            uint sum, uint partitionLength, uint minimumValue, uint maximumValue, string expectedExceptionMessage)
        {
            var ex = Assert.Throws<ArgumentException>(() => IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(
                sum, partitionLength, minimumValue, maximumValue));

            Assert.AreEqual(expectedExceptionMessage, ex?.Message);
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsExpectedValueForMagicNumber()
        {
            var sum = 17u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, 9u);

            ValidatePartitions(partitions, sum);

            Assert.AreEqual(1, partitions.Count);
            CollectionAssert.AreEqual(new List<uint> { 8, 9, }, partitions[0]);
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsExpectedValue()
        {
            var sum = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, 4u);

            ValidatePartitions(partitions, sum);

            Assert.AreEqual(2, partitions.Count);
            CollectionAssert.AreEqual(new List<uint> { 1, 4, }, partitions[0]);
            CollectionAssert.AreEqual(new List<uint> { 2, 3, }, partitions[1]);
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsOfGivenLength()
        {
            var sum = 35u;
            var partitionLength = 6u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength);

            ValidatePartitions(partitions, sum);

            Assert.IsTrue(partitions.All(p => p.Count == partitionLength));

            // Change the length and try again.
            partitionLength = 5u;
            partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength);

            ValidatePartitions(partitions, sum);

            Assert.IsTrue(partitions.All(p => p.Count == partitionLength));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsWithValuesUsingMinimumValue()
        {
            var sum = 35u;
            var minValue = 2u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 6u, minValue);

            ValidatePartitions(partitions, sum);

            Assert.IsTrue(partitions.All(p => p.All(i => i >= minValue)));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsWithValuesUsingMaximumValue()
        {
            var sum = 7u;
            var maxValue = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, maxValue);

            ValidatePartitions(partitions, sum);

            Assert.IsTrue(partitions.All(p => p.All(i => i <= maxValue)));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_RequestSamePartitionTwiceViaCacheReturnsSameResults()
        {
            var sum = 7u;
            var partitionLength = 2u;
            var minValue = 1u;
            var maxValue = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength, minValue, maxValue);

            ValidatePartitions(partitions, sum);

            Assert.IsTrue(partitions.All(p => p.All(i => i <= maxValue)));

            var cachedPartitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, maxValue);

            CollectionAssert.AreEqual(partitions, cachedPartitions);
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsAnEmptyListIfUnableToFindPartitions()
        {
            var sum = 11u;
            var partitionLength = 2u;
            var maxValue = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength, 1u, maxValue);

            Assert.AreEqual(0, partitions.Count);
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsAnEmptyListIfUnableToFindPartitionsa()
        {
            var sum = 11u;
            var partitionLength = 2u;
            var maxValue = 9u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength, 1u, maxValue);

            Assert.AreEqual(4, partitions.Count);
        }

        private static void ValidatePartitions(List<List<uint>> partitions, uint sum)
        {
            // First check that the partitions isn't empty.
            Assert.IsTrue(partitions.Count > 0);

            // Every partition should add up to the expected total, only have
            // unique values and a unique combination of values compared to the
            // rest of the partitions.
            Assert.IsTrue(partitions.All(p => p.Sum() == sum));

            // Make sure each partition has unique numbers.
            Assert.IsTrue(partitions.All(p => p.Distinct().Count() == p.Count));
        }
    }
}
