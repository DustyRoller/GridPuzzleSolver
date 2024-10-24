using NUnit.Framework;

namespace GridPuzzleSolver.UnitTests
{
    [TestFixture]
    public class CoordinateUnitTests
    {
        [Test]
        public void Coordinate_Equals_ReturnsFalseForNull()
        {
            var coordinate = new Coordinate();

            Assert.That(!coordinate.Equals(null));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForNonCoordinateObject()
        {
            var coordinate = new Coordinate();

            Assert.That(!coordinate.Equals("not a coordinate"));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForCoordinateWithDifferentValues()
        {
            var coordinate = new Coordinate
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinate = new Coordinate
            {
                X = 6u,
                Y = 12u,
            };

            Assert.That(!coordinate.Equals(otherCoordinate));
        }

        [Test]
        public void Coordinate_Equals_ReturnsFalseForCoordinateWithDifferentValue()
        {
            var coordinate = new Coordinate
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinate = new Coordinate
            {
                X = coordinate.X,
                Y = 13u,
            };

            Assert.That(!coordinate.Equals(otherCoordinate));
        }

        [Test]
        public void Coordinate_Equals_Successful()
        {
            var coordinate = new Coordinate
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinate = new Coordinate
            {
                X = coordinate.X,
                Y = coordinate.Y,
            };

            Assert.That(coordinate.Equals(otherCoordinate));
        }
    }
}
