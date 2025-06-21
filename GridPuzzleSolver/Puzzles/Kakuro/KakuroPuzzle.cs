using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using System.Xml.Serialization;

namespace GridPuzzleSolver.Puzzles.Kakuro
{
    /// <summary>
    /// Class representing a Kakuro puzzle.
    /// </summary>
    [XmlRoot("KakuroPuzzle")]
    public class KakuroPuzzle : Puzzle
    {
        /// <summary>
        /// Complete the puzzle, creating any missing cells.
        /// </summary>
        public override void CompletePuzzle()
        {
            // Calculate the height and width of the puzzle.
            Height = (uint)Cells.Count(c => c.Coordinates.Y == 0);
            Width = (uint)Cells.Count(c => c.Coordinates.X == 0);

            // Now create the sections to complete the puzzle.
            CreateSections();
        }

        /// <summary>
        /// Create the puzzle's sections.
        /// </summary>
        public void CreateSections()
        {
            // Need to generate sections that can be solved.
            for (var i = 0; i < Cells.Count; ++i)
            {
                if (Cells[i] is not ClueCell clueCell)
                {
                    continue;
                }

                // Depending on which direction of the cell's clue
                // determines which way the section will be created.
                if (clueCell.ColumnClue != 0u)
                {
                    ParseColumnSection(clueCell, i);
                }

                if (clueCell.RowClue != 0u)
                {
                    ParseRowSection(clueCell, i);
                }
            }
        }

        /// <summary>
        /// Parse a row section out of the puzzle from the given ClueCell.
        /// </summary>
        /// <param name="clueCell">The ClueCell that the section originates from.</param>
        /// <param name="cellIndex">The index of where the ClueCell is in the puzzle.</param>
        private void ParseRowSection(ClueCell clueCell, int cellIndex)
        {
            var section = new KakuroSection
            {
                ClueValue = clueCell.RowClue,
            };

            // Find all clue cells in the row until there is a break.
            for (var j = cellIndex + 1; j < Height * Width; ++j)
            {
                if (Cells[j] is not PuzzleCell)
                {
                    break;
                }

                var puzzleCell = (PuzzleCell)Cells[j];

                section.PuzzleCells.Add(puzzleCell);

                // Let the cell know it belongs to this section.
                puzzleCell.Sections.Add(section);
            }

            // Add the section to the puzzle.
            Sections.Add(section);
        }

        /// <summary>
        /// Parse a column section out of the puzzle from the given ClueCell.
        /// </summary>
        /// <param name="clueCell">The ClueCell that the section originates from.</param>
        /// <param name="cellIndex">The index of where the ClueCell is in the puzzle.</param>
        private void ParseColumnSection(ClueCell clueCell, int cellIndex)
        {
            var section = new KakuroSection
            {
                ClueValue = clueCell.ColumnClue,
            };

            // Find all clue cells in the column until there is a break.
            for (var j = (int)(cellIndex + Width); j < Height * Width; j += (int)Width)
            {
                if (Cells[j] is not PuzzleCell)
                {
                    break;
                }

                var puzzleCell = (PuzzleCell)Cells[j];

                section.PuzzleCells.Add(puzzleCell);

                // Let the cell know it belongs to this section.
                puzzleCell.Sections.Add(section);
            }

            // Add the section to the puzzle.
            Sections.Add(section);
        }
    }
}
