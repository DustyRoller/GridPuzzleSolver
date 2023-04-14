using GridPuzzleSolver.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Solvers.SudokuSolver;
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
            var xmlDoc = new XmlDocument
            {
                Schemas = killerSudukoSchema,
            };
            xmlDoc.Load(puzzleFilePath);

            xmlDoc.Validate(ValidationEventHandler);

            // Get elements
            var cellNodesList = xmlDoc.SelectNodes("//puzzle/grid/cells/cell");
            
            // Shouldn't be possible for this to happen but check just in case.
            if (cellNodesList == null || cellNodesList.Count == 0)
            {
                throw new ParserException("Failed to find cells.");
            }

            if (cellNodesList.Count != 81)
            {
                throw new ParserException($"Puzzle only contains {cellNodesList.Count}, expected 81.");
            }

            var puzzleCells = new List<PuzzleCell>();
                        
            foreach (XmlNode cellNode in cellNodesList)
            {
                var cellDataValues = cellNode.ChildNodes;
                if (cellDataValues.Count != 4)
                {
                    throw new ParserException("Failed to find cell data.");
                }

                if (!uint.TryParse(cellDataValues[0]?.InnerText, out uint id))
                {
                    throw new ParserException("Failed to parse id for cell.");
                }

                if (!uint.TryParse(cellDataValues[1]?.InnerText, out uint x))
                {
                    throw new ParserException($"Failed to parse X coordinate for cell {id}.");
                }

                if (!uint.TryParse(cellDataValues[2]?.InnerText, out uint y))
                {
                    throw new ParserException($"Failed to parse Y coordinate for cell {id}.");
                }

                if (!uint.TryParse(cellDataValues[3]?.InnerText, out uint value))
                {
                    throw new ParserException($"Failed to parse value for cell {id}.");
                }

                puzzleCells.Add(new PuzzleCell
                {
                    CellValue = value,
                    Coordinate = new Coordinate(x, y),
                });
            }

            // Setup the sections.
            var columnCellGroups = puzzleCells.GroupBy(pc => pc.Coordinate.X);

            var columns = new List<Section>();

            foreach (var columnCells in columnCellGroups)
            {
                var column = new SudokuSection();
                column.PuzzleCells.AddRange(columnCells);
                columnCells.ToList().ForEach(cc => cc.Sections.Add(column));
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
