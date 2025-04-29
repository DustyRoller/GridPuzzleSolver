using System.Xml.Serialization;

namespace GridPuzzleSolver.Components.Cells
{
    /// <summary>
    /// Base Cell class.
    /// </summary>
    public class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the Cell's Coordinates.
        /// </summary>
        [XmlElement("Coordinates")]
        public Coordinates Coordinates { get; set; } = new Coordinates();
    }
}
