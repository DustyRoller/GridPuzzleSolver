namespace GridPuzzleSolver.Puzzles.Kakuro.Utilities
{
    /// <summary>
    /// Extension class to provide useful extension functions.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Calculate the sum of the values within the given IEnumerable.
        /// </summary>
        /// <remarks>
        /// The sum function isn't provided for uint values within .NET.
        /// </remarks>
        /// <param name="values">The IEnumerable object.</param>
        /// <returns>The sum of all of the values.</returns>
        public static uint Sum(this IEnumerable<uint> values)
        {
            var sum = 0u;

            foreach (var value in values)
            {
                sum += value;
            }

            return sum;
        }
    }
}
