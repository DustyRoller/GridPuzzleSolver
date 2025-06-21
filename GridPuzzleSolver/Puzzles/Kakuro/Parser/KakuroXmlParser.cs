using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using System.Xml.Linq;

namespace GridPuzzleSolver.Puzzles.Kakuro.Parser
{
    /// <summary>
    /// Class to parse Kakuro puzzles from a XML file.
    /// </summary>
    internal class KakuroXmlParser : BaseXmlParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="xmlDocument">The XML document file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public static Puzzle ParsePuzzle(XDocument xmlDocument)
        {
            var puzzle = new KakuroPuzzle();

            var cells = xmlDocument.Root?.Element("Cells")?.Elements("Cell");
            if (cells == null || !cells.Any())
            {
                throw new ParserException("Failed to find Cells elements.");
            }

            foreach (var cellElement in cells)
            {
                // Need to determine what type of cell we are dealing with.
                Cell cell;

                if (cellElement.HasAttributes)
                {
                    if ((cellElement.Attribute("blank") != null) &&
                        bool.TryParse(cellElement.Attribute("blank")?.Value, out bool value) && value)
                    {
                        cell = new BlankCell();
                    }
                    else if (cellElement.Attribute("column_clue") != null || (cellElement.Attribute("row_clue") != null))
                    {
                        // This is a clue cell.
                        var tempCell = new ClueCell();

                        var columnClueAttribute = cellElement.Attribute("column_clue");
                        if (columnClueAttribute != null)
                        {
                            if (!uint.TryParse(columnClueAttribute.Value, out uint columnClue))
                            {
                                throw new ParserException($"Failed to parse column_clue: {columnClueAttribute.Value}.");
                            }

                            tempCell.ColumnClue = columnClue;
                        }

                        var rowClueAttribute = cellElement.Attribute("row_clue");
                        if (rowClueAttribute != null)
                        {
                            if (!uint.TryParse(rowClueAttribute.Value, out uint rowClue))
                            {
                                throw new ParserException($"Failed to parse row_clue: {rowClueAttribute.Value}.");
                            }

                            tempCell.RowClue = rowClue;
                        }

                        cell = tempCell;
                    }
                    else
                    {
                        throw new ParserException($"Cell has unrecognised attributes: {string.Join(",", cellElement.Attributes())}.");
                    }
                }
                else
                {
                    // Cell must be a puzzle cell.
                    cell = new PuzzleCell();
                }

                cell.Coordinates = ParseCoordinates(cellElement);

                puzzle.Cells.Add(cell);
            }

            puzzle.CompletePuzzle();

            return puzzle;
        }
    }
}
