using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDGToolkitCore.Application.Extensions;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.UnitTests
{
    [TestFixture]
    public class TileExtensionsTests
    {
        private readonly List<Tile> allTiles = new List<Tile>();
        private readonly Position sharedPosition = new Position(0,0);
        
        [SetUp]
        public void Init()
        {
            for (var i = 0; i < 10; i++)
            {
                allTiles.Add(new Tile(TileType.Wall, sharedPosition));
            }
        }

        [Test]
        public void Extension_ReplacesTilesWithSharedPosition()
        {
            var replacement = new Tile(TileType.Floor, sharedPosition);
            
            allTiles.ReplaceTilesWithOtherTile(replacement);
            allTiles.Count().Should().Be(1);
            allTiles.First().Should().BeEquivalentTo(replacement);
        }
        
    }
}