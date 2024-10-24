using System.Text;

namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// The ClueCell class represents a cell within the puzzle that gives
    /// either or both a horizontal or vertical clue, enabling the puzzle
    /// to be solved.
    /// </summary>
    public class ClueCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClueCell"/> class.
        /// </summary>
        /// <param name="columnClue">The cell's column's clue.</param>
        /// <param name="rowClue">The cell's row's clue.</param>
        public ClueCell(uint columnClue, uint rowClue)
        {
            ColumnClue = columnClue;
            RowClue = rowClue;
        }

        /// <summary>
        /// Gets the Cell's column clue.
        /// </summary>
        public uint ColumnClue { get; }

        /// <summary>
        /// Gets the Cell's row clue.
        /// </summary>
        public uint RowClue { get; }

        /// <summary>
        /// Get a string representation of the cell.
        /// </summary>
        /// <returns>String representing the cell.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (ColumnClue != 0u)
            {
                if (ColumnClue < 10)
                {
                    sb.Append(' ');
                }

                sb.Append(ColumnClue);
            }
            else
            {
                sb.Append("  ");
            }

            sb.Append('\\');
            if (RowClue != 0u)
            {
                sb.Append(RowClue);
                if (RowClue < 10)
                {
                    sb.Append(' ');
                }
            }
            else
            {
                sb.Append("  ");
            }

            return sb.ToString();
        }
    }
}
