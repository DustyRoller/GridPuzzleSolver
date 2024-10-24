using System.Xml.Serialization;

namespace GridPuzzleSolver
{
    /// <summary>
    /// Coordinate of a cell within a puzzle, describing its X and Y position
    /// within the puzzle grid, starting 0, 0 at the top left hand corner.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Gets or sets the Coordinate's X position.
        /// </summary>
        [XmlElement("x")]
        public uint X { get; set; }

        /// <summary>
        /// Gets or sets the Coordinate's Y position.
        /// </summary>
        [XmlElement("y")]
        public uint Y { get; set; }

        /// <summary>
        /// Does this Coordinate equal the given object.
        /// </summary>
        /// <param name="obj">The object to be comparing against.</param>
        /// <returns>True if the Coordinates are equal, otherwise false.</returns>
        public override bool Equals(object? obj)
        {
            // Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var otherCoordinate = (Coordinate)obj;
            return (X == otherCoordinate.X) && (Y == otherCoordinate.Y);
        }

        /// <summary>
        /// Get the Coordinate's hash code.
        /// </summary>
        /// <returns>The Coordinate's hash code.</returns>
        public override int GetHashCode()
        {
            return (int)(X ^ Y);
        }

        /// <summary>
        /// Get a string representation of the Coordinate.
        /// </summary>
        /// <returns>The string representation fo the Coordinate.</returns>
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
