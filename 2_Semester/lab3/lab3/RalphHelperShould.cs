using FluentAssertions;
using NUnit.Framework;

namespace lab3
{
    [TestFixture]
    public class RalphHelperShould
    {
        private RalphHelper sut;

        private readonly string[] exampleArgs =
        {
            "4 5",
            "1 4 5 7 5 2 -2 4",
            "-4 -2 3 9 1 2 -1 3 8 -3",
        };

        private const int ExampleAnswer = 6;

        [SetUp]
        public void SetUp()
        {
            sut = new RalphHelper(exampleArgs);
        }

        [Test]
        public void WorkCorrectly_WithExample()
        {
            sut.HelpRalph().Should().Be(ExampleAnswer);
        }

        [Test]
        public void InitCorrectly()
        {
            sut.MeetingPoints.Count.Should().Be(4);
            sut.InterestingPlaces.Count.Should().Be(5);
            
        }
    }
}