using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using GridPuzzleSolver.Puzzles.Sudoku;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GridPuzzleSolver.Puzzles.KillerSudoku.Parser
{
    /// <summary>
    /// Class to parse Killer Sudoku puzzles from XML files.
    /// </summary>
    internal class KillerSudokuXmlParser : BaseXmlParser
    {
        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="xmlDocument">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public static Puzzle ParsePuzzle(XDocument xmlDocument)
        {
            var puzzle = new KillerSudokuPuzzle();

            var cages = xmlDocument.Root?.Element("Cages")?.Elements("Cage");
            if (cages == null || !cages.Any())
            {
                throw new ParserException("Failed to find Cages elements.");
            }

            foreach (var cageElement in cages)
            {
                // Make sure each cage has a sum value.
                var sumAttribute = cageElement.Attribute("sum");
                if (sumAttribute == null)
                {
                    throw new ParserException($"Cage did not contain \'sum\' attribute.");
                }

                if (!uint.TryParse(sumAttribute.Value, out uint clue))
                {
                    throw new ParserException($"Failed to parse sum value: {sumAttribute.Value}.");
                }

                // Create a SumSegment for each cage.
                var section = new SumSection
                {
                    ClueValue = clue,
                };

                // Each cage will be made up of a number of cells.
                var cellsElements = cageElement?.Element("Cells")?.Elements("Cell");
                if (cellsElements == null || !cellsElements.Any())
                {
                    throw new ParserException("Failed to find Cells elements within Cage.");
                }

                foreach (var cellElement in cellsElements)
                {
                    var cell = new PuzzleCell
                    {
                        Coordinates = ParseCoordinates(cellElement),
                    };

                    puzzle.Cells.Add(cell);

                    section.PuzzleCells.Add(cell);
                }

                puzzle.Sections.Add(section);
            }

            puzzle.CompletePuzzle();

            return puzzle;
        }
    }
}
