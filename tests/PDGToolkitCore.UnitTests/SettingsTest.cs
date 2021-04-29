using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.UnitTests
{
    [TestFixture]
    public class SettingsTest
    {
        private Settings settings;

        [SetUp]
        public void Init()
        {
          // mock settings with an in-memory configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"outputRelativePath", "outputfile.extension"},
                {"serialiser", "testSerialiser"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            settings = new Settings(configuration);
        }
        
        [Test]
        public void Settings_DeducesFileExtensionFromSerialiserUsed_Succeeds()
        {
            settings.RelativePathToOutput.Should().Be("outputfile.test");
        }
        
        [Test]
        public void Settings_DeducesFileExtensionFromSerialiserUsed_Fails()
        {
            settings.RelativePathToOutput.Should().NotBe("outputfile.extension.test");
            settings.RelativePathToOutput.Should().NotBe("outputfile.test.extension");
        }
    }
}