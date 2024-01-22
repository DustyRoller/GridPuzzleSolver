namespace GridPuzzleSolver.Solvers.KakuroSolver.Utilities
{
    /// <summary>
    /// Class containing 'magic numbers', these are known integer partitions
    /// which only have a single set of valid number combinations.
    /// </summary>
    internal sealed class MagicNumbers
    {
        /// <summary>
        /// Collection of known magic numbers.
        /// </summary>
        public static readonly List<IntegerPartitions> MagicNumberValues = new ()
        {
            new IntegerPartitions(3, 2, new List<List<uint>>() { new List<uint> { 1, 2, } }),
            new IntegerPartitions(4, 2, new List<List<uint>>() { new List<uint> { 1, 3, } }),
            new IntegerPartitions(16, 2, new List<List<uint>>() { new List<uint> { 7, 9, } }),
            new IntegerPartitions(17, 2, new List<List<uint>>() { new List<uint> { 8, 9, } }),
            new IntegerPartitions(6, 3, new List<List<uint>>() { new List<uint> { 1, 2, 3, } }),
            new IntegerPartitions(7, 3, new List<List<uint>>() { new List<uint> { 1, 2, 4, } }),
            new IntegerPartitions(23, 3, new List<List<uint>>() { new List<uint> { 6, 8, 9, } }),
            new IntegerPartitions(24, 3, new List<List<uint>>() { new List<uint> { 7, 8, 9, } }),
            new IntegerPartitions(10, 4, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, } }),
            new IntegerPartitions(11, 4, new List<List<uint>>() { new List<uint> { 1, 2, 3, 5, } }),
            new IntegerPartitions(29, 4, new List<List<uint>>() { new List<uint> { 5, 7, 8, 9, } }),
            new IntegerPartitions(30, 4, new List<List<uint>>() { new List<uint> { 6, 7, 8, 9, } }),
            new IntegerPartitions(15, 5, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, } }),
            new IntegerPartitions(16, 5, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 6, } }),
            new IntegerPartitions(34, 5, new List<List<uint>>() { new List<uint> { 4, 6, 7, 8, 9, } }),
            new IntegerPartitions(35, 5, new List<List<uint>>() { new List<uint> { 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(21, 6, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, } }),
            new IntegerPartitions(22, 6, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 7, } }),
            new IntegerPartitions(38, 6, new List<List<uint>>() { new List<uint> { 3, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(39, 6, new List<List<uint>>() { new List<uint> { 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(28, 7, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 7, } }),
            new IntegerPartitions(29, 7, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 8, } }),
            new IntegerPartitions(41, 7, new List<List<uint>>() { new List<uint> { 2, 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(42, 7, new List<List<uint>>() { new List<uint> { 3, 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(36, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 7, 8, } }),
            new IntegerPartitions(37, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 7, 9, } }),
            new IntegerPartitions(38, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 8, 9, } }),
            new IntegerPartitions(39, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 7, 8, 9, } }),
            new IntegerPartitions(40, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, } }),
            new IntegerPartitions(41, 8, new List<List<uint>>() { new List<uint> { 1, 2, 3, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(42, 8, new List<List<uint>>() { new List<uint> { 1, 2, 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(43, 8, new List<List<uint>>() { new List<uint> { 1, 3, 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(44, 8, new List<List<uint>>() { new List<uint> { 2, 3, 4, 5, 6, 7, 8, 9, } }),
            new IntegerPartitions(45, 9, new List<List<uint>>() { new List<uint> { 1, 2, 3, 4, 5, 6, 7, 8, 9, } }),
        };

        private MagicNumbers()
        {
        }
    }
}
