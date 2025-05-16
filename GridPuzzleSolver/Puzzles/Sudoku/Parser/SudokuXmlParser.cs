using System.Xml;
using System.Xml.Linq;
using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;

namespace GridPuzzleSolver.Puzzles.Sudoku.Parser
{
    /// <summary>
    /// Class to parse Sudoku puzzles from XML files.
    /// </summary>
    internal class SudokuXmlParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="xmlDocument">The XML document file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public Puzzle ParsePuzzle(XDocument xmlDocument)
        {
            var puzzle = new SudokuPuzzle();

            var cells = xmlDocument.Root?.Element("Cells")?.Elements("Cell");
            if (cells == null || !cells.Any())
            {
                throw new ParserException("Failed to find Cells elements.");
            }

            foreach (var cell in cells)
            {
                var puzzleCell = new PuzzleCell
                {
                    Coordinates = ParseCoordinates(cell),
                };

                if (cell.Attribute("value") != null)
                {
                    if (!uint.TryParse(cell.Attribute("value")?.Value, out uint value))
                    {
                        throw new ParserException($"Failed to parse cell value: {cell.Attribute("value")?.Value}.");
                    }

                    puzzleCell.CellValue = value;
                }

                puzzle.Cells.Add(puzzleCell);
            }

            puzzle.CompletePuzzle();

            return puzzle;
        }

        /// <summary>
        /// Parse the Cells' coordinates from the given Cell element.
        /// </summary>
        /// <param name="cell">The Cell element to parse the coordinates from.</param>
        /// <returns>A Coordinates object.</returns>
        /// <exception cref="ParserException">Thrown if an error occurs whilst
        /// parsing the coordinates data.</exception>
        private static Coordinates ParseCoordinates(XElement cell)
        {
            var coordinatesElement = cell.Element("Coordinates");
            if (coordinatesElement == null)
            {
                throw new ParserException("Cell did not contain \'Coordinates\' element.");
            }

            var x = ParseCoordinateElement(coordinatesElement, "x");
            var y = ParseCoordinateElement(coordinatesElement, "y");

            return new Coordinates
            {
                X = x,
                Y = y,
            };
        }

        /// <summary>
        /// Parse the given attribute value from the given coordinate element.
        /// </summary>
        /// <param name="coordinatesElement">The coordinates element contain the
        /// attribute.</param>
        /// <param name="attributeName">The name of the attribute to parse.</param>
        /// <returns>The attribute value.</returns>
        /// <exception cref="ParserException">Thrown if the attribute doesn't exist
        /// or is not of the expected type.</exception>
        private static uint ParseCoordinateElement(XElement coordinatesElement, string attributeName)
        {
            var attribute = coordinatesElement.Attribute(attributeName);
            if (attribute == null)
            {
                throw new ParserException($"Coordinate did not contain \'{attributeName}\' attribute.");
            }

            if (!uint.TryParse(attribute.Value, out uint value))
            {
                throw new ParserException($"Failed to parse {attributeName} coordinate value: {attribute.Value}.");
            }

            return value;
        }
    }
}
