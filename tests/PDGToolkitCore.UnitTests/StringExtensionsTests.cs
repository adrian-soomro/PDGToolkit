using System.IO;
using FluentAssertions;
using NUnit.Framework;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.UnitTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        private const string RelativePathToTestFileFromSolutionRoot = "tests/PDGToolkitCore.UnitTests/data/file";  
        
        [Test]
        public void Extension__ReturnsCorrectLocation()
        {
            var actualAbsolutePath = RelativePathToTestFileFromSolutionRoot.ToAbsolutePathFromPathRelativeToSolutionRoot();
            var expected =
                Path.Combine(
                    Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                    RelativePathToTestFileFromSolutionRoot);
            actualAbsolutePath.Should().Be(expected);
        }

        [Test]
        public void Extension__ReturnsAccessibleLocation()
        {
            var absolutePathToTestFile =
                RelativePathToTestFileFromSolutionRoot.ToAbsolutePathFromPathRelativeToSolutionRoot();
            var actualContents = File.ReadAllText(absolutePathToTestFile);
            const string expectedContents = "This is a test file.";
            actualContents.Should().Be(expectedContents);
        }
    }
}