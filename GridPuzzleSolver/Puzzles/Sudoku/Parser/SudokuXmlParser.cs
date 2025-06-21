using System.Xml.Linq;
using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;

namespace GridPuzzleSolver.Puzzles.Sudoku.Parser
{
    /// <summary>
    /// Class to parse Sudoku puzzles from a XML file.
    /// </summary>
    internal class SudokuXmlParser : BaseXmlParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="xmlDocument">The XML document file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public static Puzzle ParsePuzzle(XDocument xmlDocument)
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
    }
}
