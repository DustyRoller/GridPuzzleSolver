namespace GridPuzzleSolver.Utilities
{
    /// <summary>
    /// Class to calculate integer partitions for given number combinations.
    /// </summary>
    internal static class IntegerPartitionCalculator
    {
        /// <summary>
        /// As calculating integer partitions can be quite slow, keep a cache
        /// of previously caclulated integer partitions and the known magic numbers.
        /// </summary>
        private static readonly List<IntegerPartitions> Cache = new(MagicNumbers.MagicNumberValues);

        /// <summary>
        /// Calculate the distinct integer partitionss for the given set
        /// of arguments.
        /// </summary>
        /// <param name="total">The total that the partitions must add up to.</param>
        /// <param name="partitionLength">The number of partitions.</param>
        /// <param name="minValue">The minimum value to be used in the partitions.</param>
        /// <param name="maxValue">The maximum value to be used in the partitions.</param>
        /// <returns>A List of integer partitions.</returns>
        public static List<List<uint>> CalculateDistinctIntegerPartitions(uint total, uint partitionLength, uint minValue = 1, uint maxValue = 9)
        {
            // Provide some input validation before calling the recursive function.
            if (maxValue > 9)
            {
                throw new ArgumentException("Maximum value cannot be greater than 9.");
            }

            if (maxValue >= total)
            {
                throw new ArgumentException("Maximum value cannot be greater than or equal to sum.");
            }

            if (minValue >= maxValue)
            {
                throw new ArgumentException("Minimum value must be less than the maximum value.");
            }

            if (maxValue <= minValue)
            {
                throw new ArgumentException("Maximum value must be greater than the minimum value.");
            }

            var integerPartitions = Cache.FirstOrDefault(ip => ip.PartitionLength == partitionLength &&
                                                               ip.Total == total &&
                                                               ip.Values[0].All(v => v >= minValue && v <= maxValue));

            if (integerPartitions == null)
            {
                // We need to calculate the integer partition.
                var values = IntegerPartitionCalculator.CalculateDistinctIntegerPartitionsRecursive(total, partitionLength, minValue, maxValue);

                integerPartitions = new IntegerPartitions(total, partitionLength, values);

                // Now add it to the cache for future use.
                Cache.Add(integerPartitions);
            }

            return integerPartitions.Values;
        }

        /// <summary>
        /// Recursively calculate the distinct integer partitionss for the
        /// given set of arguments.
        /// </summary>
        /// <param name="sum">The sum that the partitions must add up to.</param>
        /// <param name="partitionLength">The number of partitions.</param>
        /// <param name="minimumValue">The minimum value to be used in the partitions.</param>
        /// <param name="maximumValue">The maximum value to be used in the partitions.</param>
        /// <returns>A List of integer partitions.</returns>
        private static List<List<uint>> CalculateDistinctIntegerPartitionsRecursive(uint sum, uint partitionLength, uint minimumValue = 1, uint maximumValue = 9)
        {
            var partitions = new List<List<uint>>();

            // Recursively find the integer partitions until we get to 1 or less.
            if (sum > 1)
            {
                for (var i = Math.Min(sum, maximumValue); i >= minimumValue; i--)
                {
                    var recursivePartitions = CalculateDistinctIntegerPartitionsRecursive(sum - i, partitionLength - 1, minimumValue, i);
                    foreach (var recursivePartition in recursivePartitions)
                    {
                        recursivePartition.Add(i);

                        // Do any of the other partitions also contain the same numbers in a different order.
                        if (!partitions.Any(r => r.All(recursivePartition.Contains))
                            && recursivePartition.Distinct().Count() == recursivePartition.Count
                            && partitionLength == recursivePartition.Count
                            && recursivePartition.Sum() == sum)
                        {
                            partitions.Add(recursivePartition);
                        }
                    }
                }
            }
            else
            {
                var partition = (sum == 0 || sum < minimumValue)
                    ? new List<uint>()
                    : new List<uint>() { sum };

                partitions.Add(partition);
            }

            return partitions;
        }
    }
}
