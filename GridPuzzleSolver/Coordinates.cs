using System.Xml.Serialization;

namespace GridPuzzleSolver
{
    /// <summary>
    /// Coordinates of a cell within a puzzle, describing its X and Y position
    /// within the puzzle grid, starting 0, 0 at the top left hand corner.
    /// </summary>
    public class Coordinates
    {
        /// <summary>
        /// Gets or sets the Coordinate's X position.
        /// </summary>
        [XmlAttribute("x")]
        public uint X { get; set; }

        /// <summary>
        /// Gets or sets the Coordinate's Y position.
        /// </summary>
        [XmlAttribute("y")]
        public uint Y { get; set; }

        /// <summary>
        /// Does this Coordinates object equal the given object.
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

            var otherCoordinates = (Coordinates)obj;
            return (X == otherCoordinates.X) && (Y == otherCoordinates.Y);
        }

        /// <summary>
        /// Get the Coordinates' hash code.
        /// </summary>
        /// <returns>The Coordinates' hash code.</returns>
        public override int GetHashCode()
        {
            return (int)(X ^ Y);
        }

        /// <summary>
        /// Get a string representation of the Coordinates.
        /// </summary>
        /// <returns>The string representation fo the Coordinates.</returns>
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
