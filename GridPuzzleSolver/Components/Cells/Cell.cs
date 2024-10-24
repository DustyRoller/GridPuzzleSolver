using System.Xml.Serialization;

namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Base Cell class.
    /// </summary>
    public abstract class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the Cell's Coordinate.
        /// </summary>
        [XmlElement("coordinates")]
        public Coordinate Coordinate { get; set; } = new Coordinate();
    }
}
