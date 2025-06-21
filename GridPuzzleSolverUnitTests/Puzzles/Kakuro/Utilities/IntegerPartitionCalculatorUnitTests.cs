using GridPuzzleSolver.Puzzles.Kakuro.Utilities;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests.Puzzles.Kakuro.Utilities
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

            Assert.That(expectedExceptionMessage, Is.EqualTo(ex?.Message));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsSinglePartitionForMagicNumber()
        {
            var sum = 17u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, 9u);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions, Has.Count.EqualTo(1));
            Assert.That(partitions[0], Is.EqualTo(new List<uint> { 8, 9, }));
        }

        [Test]
        [TestCaseSource(nameof(GetIntegerPartitionTestData))]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsExpectedValues(
            uint sum, uint partitionLength, uint minimumValue, uint maximumValue, List<List<uint>> expectedPartitions)
        {
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength, minimumValue, maximumValue);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions, Is.EqualTo(expectedPartitions));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsOfGivenLength()
        {
            var sum = 35u;
            var partitionLength = 6u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions.All(p => p.Count == partitionLength));

            // Change the length and try again.
            partitionLength = 5u;
            partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions.All(p => p.Count == partitionLength));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsWithValuesUsingMinimumValue()
        {
            var sum = 35u;
            var minValue = 2u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 6u, minValue);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions.All(p => p.All(i => i >= minValue)));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsOnlyPartitionsWithValuesUsingMaximumValue()
        {
            var sum = 7u;
            var maxValue = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, maxValue);

            ValidatePartitions(partitions, sum);

            Assert.That(partitions.All(p => p.All(i => i <= maxValue)));
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

            Assert.That(partitions.All(p => p.All(i => i <= maxValue)));

            var cachedPartitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, 2u, 1u, maxValue);

            Assert.That(partitions, Is.EqualTo(cachedPartitions));
        }

        [Test]
        public void IntegerPartitionCalulator_CalculateDistinctIntegerPartitions_ReturnsAnEmptyListIfUnableToFindPartitions()
        {
            var sum = 11u;
            var partitionLength = 2u;
            var maxValue = 5u;
            var partitions = IntegerPartitionCalculator.CalculateDistinctIntegerPartitions(sum, partitionLength, 1u, maxValue);

            Assert.That(partitions, Is.Empty);
        }

        private static void ValidatePartitions(List<List<uint>> partitions, uint sum)
        {
            // First check that the partitions isn't empty.
            Assert.That(partitions, Is.Not.Empty);

            // Every partition should add up to the expected total, only have
            // unique values and a unique combination of values compared to the
            // rest of the partitions.
            Assert.That(partitions.All(p => p.Sum() == sum));

            // Make sure each partition has unique numbers.
            Assert.That(partitions.All(p => p.Distinct().Count() == p.Count));
        }

        /// <summary>
        /// Create integer partition test data.
        /// </summary>
        /// <returns>Data containing integer partition parameters and expected results.</returns>
        private static IEnumerable<TestCaseData> GetIntegerPartitionTestData()
        {
            yield return new TestCaseData(11u, 2u, 1u, 9u, new List<List<uint>>
            {
                new List<uint>
                {
                    2u, 9u,
                },
                new List<uint>
                {
                    3u, 8u,
                },
                new List<uint>
                {
                    4u, 7u,
                },
                new List<uint>
                {
                    5u, 6u,
                },
            });

            yield return new TestCaseData(5u, 2u, 1u, 4u, new List<List<uint>>
            {
                new List<uint>
                {
                    1u, 4u,
                },
                new List<uint>
                {
                    2u, 3u,
                },
            });
        }
    }
}
