using GridPuzzleSolver;
using NUnit.Framework;

namespace GridPuzzleSolverUnitTests
{
    [TestFixture]
    public class CoordinatesUnitTests
    {
        [Test]
        public void Coordinates_Equals_ReturnsFalseForNull()
        {
            var coordinates = new Coordinates
            {
                X = 5u,
                Y = 8u,
            };

            Assert.That(coordinates, Is.Not.EqualTo(null));
        }

        [Test]
        public void Coordinates_Equals_ReturnsFalseForNonCoordinatesObject()
        {
            var coordinates = new Coordinates
            {
                X = 5u,
                Y = 8u,
            };

            Assert.That(!coordinates.Equals("not a coordinate"));
        }

        [Test]
        public void Coordinates_Equals_ReturnsFalseForCoordinatesWithDifferentValues()
        {
            var coordinates = new Coordinates
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinates = new Coordinates
            {
                X = 6u,
                Y = 12u,
            };

            Assert.That(coordinates, Is.Not.EqualTo(otherCoordinates));
        }

        [Test]
        public void Coordinates_Equals_ReturnsFalseForCoordinatesWithDifferentValue()
        {
            var coordinates = new Coordinates
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinates = new Coordinates
            {
                X = coordinates.X,
                Y = 13u,
            };

            Assert.That(coordinates, Is.Not.EqualTo(otherCoordinates));
        }

        [Test]
        public void Coordinates_Equals_Successful()
        {
            var coordinates = new Coordinates
            {
                X = 5u,
                Y = 8u,
            };

            var otherCoordinates = new Coordinates
            {
                X = coordinates.X,
                Y = coordinates.Y,
            };

            Assert.That(coordinates, Is.EqualTo(otherCoordinates));
        }
    }
}
