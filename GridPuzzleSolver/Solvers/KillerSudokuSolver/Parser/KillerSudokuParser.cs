using GridPuzzleSolver.Parser;
using System.Xml;
using System.Xml.Schema;

namespace GridPuzzleSolver.Solvers.KillerSudokuSolver.Parser
{
    /// <summary>
    /// Class to parse Killer Sudoku puzzles from XML files.
    /// </summary>
    internal class KillerSudokuParser : BaseParser
    {
        /// <summary>
        /// Gets the file extension of the file that the parser will read.
        /// </summary>
        public static string FileExtension => ".ksud";

        private const string SchemaFile = "KillerSudokuSchema.xsd";

        public override Puzzle ParsePuzzle(string puzzleFilePath)
        {
            ValidateInputFile(puzzleFilePath, FileExtension);

            // Check that the schema file exists.
            if (!File.Exists(SchemaFile))
            {
                throw new FileNotFoundException("Failed to find schema file.", SchemaFile);
            }

            var killerSudukoSchema = new XmlSchemaSet();
            killerSudukoSchema.Add("", SchemaFile);

            // Now read in the puzzle.
            var xmlDoc = new XmlDocument();
            xmlDoc.Schemas = killerSudukoSchema;
            xmlDoc.Load(puzzleFilePath);

            xmlDoc.Validate(ValidationEventHandler);

            // Get elements
            XmlNodeList grid = xmlDoc.GetElementsByTagName("grid");
            if (grid.Count == 0)
            {
                throw new ParserException("Failed to find grid element in puzzle file.");
            }

            var puzzle = new Puzzle();


            return puzzle;
        }

        /// <summary>
        /// Event handler for when the puzzle file does not match the schema.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        /// <exception cref="ParserException">Thrown when this event occurs.</exception>
        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new ParserException("Failed to validate puzzle file against the schema.", e.Exception);
        }
    }
}
