using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Puzzles.Sudoku;
using System.Xml;
using System.Xml.Schema;

namespace GridPuzzleSolver.Puzzles.KillerSudoku.Parser
{
    /// <summary>
    /// Class to parse Killer Sudoku puzzles from XML files.
    /// </summary>
    internal class KillerSudokuParser : BaseParser
    {
        private const string SchemaFile = "KillerSudokuSchema.xsd";

        /// <summary>
        /// Gets the file extension of the file that the parser will read.
        /// </summary>
        public static string FileExtension => ".ksud";

        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public override Puzzle ParsePuzzle(string puzzleFilePath)
        {
            ValidateInputFile(puzzleFilePath, FileExtension);

            // Check that the schema file exists.
            if (!File.Exists(SchemaFile))
            {
                throw new FileNotFoundException("Failed to find schema file.", SchemaFile);
            }

            var killerSudukoSchema = new XmlSchemaSet();
            killerSudukoSchema.Add(string.Empty, SchemaFile);

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
                throw new ParserException("Failed to find any cells.");
            }

            if (cellNodesList.Count != 81)
            {
                throw new ParserException($"Puzzle only contains {cellNodesList.Count} cells, expected 81.");
            }

            var puzzle = new Puzzle();

            foreach (XmlNode cellNode in cellNodesList)
            {
                puzzle.AddCell(ParseCell(cellNode));
            }

            ParseSections(puzzle);

            // Make sure we have at least one cage.
            var cageNodesList = xmlDoc.SelectNodes("//puzzle/cages/cage");

            // Shouldn't be possible for this to happen but check just in case.
            if (cageNodesList == null || cageNodesList.Count == 0)
            {
                throw new ParserException("Failed to find any cages.");
            }

            foreach (XmlNode cageNode in cageNodesList)
            {
                puzzle.Sections.Add(ParseCage(cageNode));
            }

            // As a cage can only contain each number once, the minium number of cages is 9.
            Console.Write(cageNodesList.Count);

            return puzzle;
        }

        private static Cage ParseCage(XmlNode cageNode)
        {
            var cageDataNodes = cageNode.ChildNodes;

            // Cast to a IEnumerable so that we can perform queries on the nodes list.
            var cageDataNodesList = cageDataNodes.Cast<XmlNode>();

            // Make sure that we have a sum and list of cell IDs.
            var sumNode = cageDataNodesList.FirstOrDefault(n => n.Name == "sum")
                ?? throw new ParserException("Failed to find sum value for cage.");

            if (!uint.TryParse(sumNode.InnerText, out uint sum))
            {
                throw new ParserException("Failed to parse sum value for cage.");
            }

            // Cage can have a maximum of 9 cells, meaning that its maxium value is 45.
            // Triangular number
            if (sum > 45)
            {
                throw new ParserException($"Cage sum value {sum} is invalid, must be 45 or lower.");
            }

            var cellListNode = cageDataNodesList.Where(n => n.Name == "cells");

            if (!cellListNode.Any())
            {
                throw new ParserException($"Cage must have at least one cell.");
            }

            if (cellListNode.Count() > 9)
            {
                throw new ParserException($"Cage cannot have more than 9 cells.");
            }

            Console.WriteLine(cellListNode);

            return new Cage(sum);
        }

        private static PuzzleCell ParseCell(XmlNode cellNode)
        {
            var cellDataNodes = cellNode.ChildNodes;
            if (cellDataNodes.Count != 4)
            {
                throw new ParserException("Failed to find all cell data.");
            }

            // Cast to a IEnumerable so that we can perform queries on the nodes list.
            var cellDataNodesList = cellDataNodes.Cast<XmlNode>();

            var idNode = cellDataNodesList.FirstOrDefault(n => n.Name == "id")
                ?? throw new ParserException("Failed to find ID value for cell.");

            if (!uint.TryParse(idNode.InnerText, out uint id))
            {
                throw new ParserException("Failed to parse ID value for cell.");
            }

            var xNode = cellDataNodesList.FirstOrDefault(n => n.Name == "x")
                ?? throw new ParserException($"Failed to find x value for cell {id}.");

            if (!uint.TryParse(xNode.InnerText, out uint x))
            {
                throw new ParserException($"Failed to parse x value for cell {id}.");
            }

            var yNode = cellDataNodesList.FirstOrDefault(n => n.Name == "y")
                ?? throw new ParserException($"Failed to find y value for cell {id}.");

            if (!uint.TryParse(yNode.InnerText, out uint y))
            {
                throw new ParserException($"Failed to parse ID value for cell {id}.");
            }

            var puzzleCell = new PuzzleCell(new Coordinate(x, y));

            // Cell may not have a value.
            var valueNode = cellDataNodesList.FirstOrDefault(n => n.Name == "value");
            if (valueNode != null && uint.TryParse(valueNode.InnerText, out uint value))
            {
                puzzleCell.CellValue = value;
            }

            return puzzleCell;
        }

        private static void ParseSections(Puzzle puzzle)
        {
            // Get all the column and row sections.
            for (int i = 0; i < 9; ++i)
            {
                // Get the column section cells.
                var columnCells = puzzle.Cells.Where(c => c.Coordinate.Y == i)
                                              .Select(c => (PuzzleCell)c)
                                              .ToList();

                var columnSection = new SudokuSection();
                columnSection.PuzzleCells.AddRange(columnCells);

                // Add the column section to the puzzle.
                puzzle.Sections.Add(columnSection);

                // Set the column section on each cell.
                columnCells.ForEach(cc => cc.Sections.Add(columnSection));

                // Get the row section cells.
                var rowCells = puzzle.Cells.Where(c => c.Coordinate.X == i)
                                           .Select(c => (PuzzleCell)c)
                                           .ToList();

                var rowSection = new SudokuSection();
                rowSection.PuzzleCells.AddRange(rowCells);

                // Add the row section to the puzzle.
                puzzle.Sections.Add(rowSection);

                // Set the row section on each cell.
                rowCells.ForEach(rc => rc.Sections.Add(rowSection));
            }

            // Get all of the 3x3 squares from within the puzzle,
            // this is a pretty ugly way of doing it but works for now.
            var squareStartingIndexes = new List<int>()
            {
                0, 3, 6, 27, 30, 33, 54, 57, 60,
            };

            foreach (var startingIndex in squareStartingIndexes)
            {
                var index = startingIndex;
                var squareCells = new List<PuzzleCell>();
                for (int x = 0; x < 3; ++x)
                {
                    for (int y = 0; y < 3; ++y)
                    {
                        squareCells.Add((PuzzleCell)puzzle.Cells[index]);
                        ++index;
                    }

                    index += 6;
                }

                var squareSection = new SudokuSection();
                squareSection.PuzzleCells.AddRange(squareCells);

                // Set the square section on each cell.
                squareCells.ForEach(sc => sc.Sections.Add(squareSection));

                puzzle.Sections.Add(squareSection);
            }
        }

        /// <summary>
        /// Event handler for when the puzzle file does not match the schema.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        /// <exception cref="ParserException">Thrown when this event occurs.</exception>
        private static void ValidationEventHandler(object? sender, ValidationEventArgs e)
        {
            throw new ParserException("Failed to validate puzzle file against the schema.", e.Exception);
        }
    }
}
