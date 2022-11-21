using NUnit.Framework;

namespace GridPuzzleSolver.UnitTests
{
    [TestFixture]
    public class CoordinateUnitTests
    {
        [Test]
        public void Coordinate_Equals_ReturnsFalseForNull()
        {
            var coordinate = new Coordinate(5u, 8u);

            Assert.IsFalse(coordinate.Equals(null));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForNonCoordinateObject()
        {
            var coordinate = new Coordinate(5u, 8u);

            Assert.IsFalse(coordinate.Equals("not a coordinate"));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForCoordinateWithDifferentValues()
        {
            var coordinate = new Coordinate(5u, 8u);

            var otherCoordinate = new Coordinate(6u, 12u);

            Assert.IsFalse(coordinate.Equals(otherCoordinate));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForCoordinateWithDifferentValue()
        {
            var coordinate = new Coordinate(5u, 8u);

            var otherCoordinate = new Coordinate(coordinate.X, 13u);

            Assert.IsFalse(coordinate.Equals(otherCoordinate));
        }

        [Test]
        public void Coordinate_Equals_Successful()
        {
            var coordinate = new Coordinate(5u, 8u);

            var otherCoordinate = new Coordinate(coordinate.X, coordinate.Y);

            Assert.IsTrue(coordinate.Equals(otherCoordinate));
        }
    }
}
