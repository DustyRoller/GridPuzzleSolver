using GridPuzzleSolver.Components;
using GridPuzzleSolver.Parser;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("GridPuzzleSolverUnitTests")]

namespace GridPuzzleSolver
{
    /// <summary>
    /// The class containing the entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// a.
        /// </summary>
        /// <typeparam name="T">b.</typeparam>
        /// <param name="filepath">c.</param>
        /// <returns>d.</returns>
        public static T? DeserializeToObject<T>(string filepath)
            where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(filepath))
            {
                return xmlSerializer.Deserialize(streamReader) as T;
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Expected 1 argument - puzzle file name");
                Environment.Exit(1);
            }

            var puzzle = DeserializeToObject<Puzzle>(args[0]);
            if (puzzle == null)
            {
                throw new ArgumentException("Failed to deserialize");
            }

            Console.WriteLine($"Height: {puzzle.Height}");
            Console.WriteLine($"Width: {puzzle.Width}");
        }
    }
}