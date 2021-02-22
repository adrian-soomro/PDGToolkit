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
        [Test]
        public void TileExtensions_ReplacesTilesWithSharedPosition()
        {
            var tilesWithSharedPositions = new List<Tile>();
            var sharedPosition = new Position(0,0);
            for (var i = 0; i < 10; i++)
            {
                tilesWithSharedPositions.Add(new Tile(TileType.Wall, sharedPosition));
            }
            
            var replacement = new Tile(TileType.Floor, sharedPosition);
            
            tilesWithSharedPositions.ReplaceTilesWithOtherTile(replacement);
            tilesWithSharedPositions.Count().Should().Be(1);
            tilesWithSharedPositions.First().Should().BeEquivalentTo(replacement);
        }
        
        [Test]
        public void TileExtensions_HasTwoVerticallyAdjacentFloorTilesTest()
        {
            /* create 3 horizontal lines, composed of 10 tiles
            like so:    FFFFF
                        WWWWW
                        FFFFF
            where F = Floor tile, W = Wall tile
             */
            var allTiles = TileBuilder.Create()
                .WithHorizontalLineOfFloorsStartingAtPosition(5, new Position(0, 0))
                .WithHorizontalWallStartingAtPosition(5, new Position(0, 1))
                .WithHorizontalLineOfFloorsStartingAtPosition(5, new Position(0, 2))
                .Build();
            
            var aWallTile = new Tile(TileType.Wall, new Position(0,1));

            var result = aWallTile.HasTwoAdjacentFloorTiles(allTiles);
            result.Should().BeTrue();
        }
        
        [Test]
        public void TileExtensions_HasNoVerticallyAdjacentFloorTilesTest()
        {
            /* create 3 horizontal lines, composed of 10 tiles
            like so:    FFFFF
                        WWWWW
                        WWWWW
            where F = Floor tile, W = Wall tile
             */
            var allTiles = TileBuilder.Create()
                .WithHorizontalLineOfFloorsStartingAtPosition(5, new Position(0, 0))
                .WithHorizontalWallStartingAtPosition(5, new Position(0, 1))
                .WithHorizontalWallStartingAtPosition(5, new Position(0, 2))
                .Build();
            
            var aWallTile = new Tile(TileType.Wall, new Position(0,1));

            var result = aWallTile.HasTwoAdjacentFloorTiles(allTiles);
            result.Should().BeFalse();
        }
        
        [Test]
        public void TileExtensions_HasTwoHorizontallyAdjacentFloorTilesTest()
        {
            /* create 3 vertical lines, composed of 5 tiles
            like so:    FWF
                        FWF
                        FWF
                        FWF
                        FWF
            where F = Floor tile, W = Wall tile
             */
            var allTiles = TileBuilder.Create()
                .WithVerticalLineOfFloorsStartingAtPosition(5, new Position(0, 0))
                .WithVerticalWallStartingAtPosition(5, new Position(1, 0))
                .WithVerticalLineOfFloorsStartingAtPosition(5, new Position(2, 0))
                .Build();
            
            var aWallTile = new Tile(TileType.Wall, new Position(1,0));

            var result = aWallTile.HasTwoAdjacentFloorTiles(allTiles);
            result.Should().BeTrue();
        }
        
        [Test]
        public void TileExtensions_HasNoHorizontallyAdjacentFloorTilesTest()
        {
            /* create 3 vertical lines, composed of 5 tiles
            like so:    FWW
                        FWW
                        FWW
                        FWW
                        FWW
            where F = Floor tile, W = Wall tile
             */
            var allTiles = TileBuilder.Create()
                .WithVerticalLineOfFloorsStartingAtPosition(5, new Position(0, 0))
                .WithVerticalWallStartingAtPosition(5, new Position(1, 0))
                .WithVerticalWallStartingAtPosition(5, new Position(2, 0))
                .Build();
            
            var aWallTile = new Tile(TileType.Wall, new Position(1,0));

            var result = aWallTile.HasTwoAdjacentFloorTiles(allTiles);
            result.Should().BeFalse();
        }

        [Test] public void TileExtensions_HasBothVerticallyAndHorizontallyAdjacentFloorTiles()
        {
            /* Given a set of tiles with positions
            like so:    012 x
            0            F
            1           FWF
            2            F
            y
            where F = Floor tile, W = Wall tile
             */
            var allTiles = new List<Tile>
            {
                new Tile(TileType.Floor, new Position(1,0)),
                new Tile(TileType.Floor, new Position(0,1)),
                new Tile(TileType.Wall, new Position(1,1)),
                new Tile(TileType.Floor, new Position(2,1)),
                new Tile(TileType.Floor, new Position(1,2))
            };
            
            var aWallTile = new Tile(TileType.Wall, new Position(1,1));

            var result = aWallTile.HasTwoAdjacentFloorTiles(allTiles);
            result.Should().BeTrue();
        }
        
        [Test] public void TileExtensions_HasMaxTwoAdjacentWallTiles()
        {
            /* Given a set of tiles with positions
            like so:    012 x
            0            W
            1           FWF
            2            W
            y
            where F = Floor tile, W = Wall tile
             */
            var allTiles = new List<Tile>
            {
                new Tile(TileType.Wall, new Position(1,0)),
                new Tile(TileType.Floor, new Position(0,1)),
                new Tile(TileType.Wall, new Position(1,1)),
                new Tile(TileType.Floor, new Position(2,1)),
                new Tile(TileType.Wall, new Position(1,2))
            };
            
            var aWallTile = new Tile(TileType.Wall, new Position(1,1));

            var result = aWallTile.HasMaxTwoAdjacentWallTiles(allTiles);
            result.Should().BeTrue();
        }
        
        [Test] public void TileExtensions_DoesNotHaveMaxTwoAdjacentWallTiles()
        {
            /* Given a set of tiles with positions
            like so:    012 x
            0            W
            1           FWF
            2            F
            y
            where F = Floor tile, W = Wall tile
             */
            var allTiles = new List<Tile>
            {
                new Tile(TileType.Wall, new Position(1,0)),
                new Tile(TileType.Floor, new Position(0,1)),
                new Tile(TileType.Wall, new Position(1,1)),
                new Tile(TileType.Floor, new Position(2,1)),
                new Tile(TileType.Floor, new Position(1,2))
            };
            
            var aWallTile = new Tile(TileType.Wall, new Position(1,1));

            var result = aWallTile.HasMaxTwoAdjacentWallTiles(allTiles);
            result.Should().BeFalse();
        }
    }
}