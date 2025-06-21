using System.Xml.Linq;

namespace GridPuzzleSolver.Parser
{
    /// <summary>
    /// Abstract class providing common functionality for XML parsers.
    /// </summary>
    internal abstract class BaseXmlParser
    {
        /// <summary>
        /// Parse the Cells' coordinates from the given Cell element.
        /// </summary>
        /// <param name="cell">The Cell element to parse the coordinates from.</param>
        /// <returns>A Coordinates object.</returns>
        /// <exception cref="ParserException">Thrown if an error occurs whilst
        /// parsing the coordinates data.</exception>
        protected static Coordinates ParseCoordinates(XElement cell)
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
