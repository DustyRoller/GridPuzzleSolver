using System.Xml.Serialization;

namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Interface for all cell types.
    /// </summary>
    internal interface ICell
    {
        /// <summary>
        /// Gets or sets the Cell's Coordinate.
        /// </summary>
        [XmlElement("coordinates")]
        Coordinate Coordinate { get; set; }
    }
}
