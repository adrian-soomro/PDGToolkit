using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PDGToolkitCore.Application;
using PDGToolkitCore.Domain.Models;


namespace PDGToolkitCore.UnitTests
{
    [TestFixture]
    public class TileServiceTests
    {
        private readonly ITileService tileService = new TileService();
        
        [Test]
        public void Tile__HasTwoVerticallyAdjacentFloorTilesTest()
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
            
            var aWallTilePosition = new Position(0,1);
            
            var result = tileService.HasTwoAdjacentFloorTiles(allTiles, aWallTilePosition);
            result.Should().BeTrue();
        }
        
        [Test]
        public void Tile__HasNoVerticallyAdjacentFloorTilesTest()
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
            
            var aWallTilePosition = new Position(0,1);
            
            var result = tileService.HasTwoAdjacentFloorTiles(allTiles, aWallTilePosition);
            result.Should().BeFalse();
        }
        
        [Test]
        public void Tile__HasTwoHorizontallyAdjacentFloorTilesTest()
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
            
            var aWallTilePosition = new Position(1,0);
            
            var result = tileService.HasTwoAdjacentFloorTiles(allTiles, aWallTilePosition);
            result.Should().BeTrue();
        }
        
        [Test]
        public void Tile__HasNoHorizontallyAdjacentFloorTilesTest()
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
            
            var aWallTilePosition = new Position(1,0);
            
            var result = tileService.HasTwoAdjacentFloorTiles(allTiles, aWallTilePosition);
            result.Should().BeFalse();
        }

        [Test] public void Tile__HasBothVerticallyAndHorizontallyAdjacentFloorTiles()
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
            
            
            var aWallTilePosition = new Position(1,1);
            
            var result = tileService.HasTwoAdjacentFloorTiles(allTiles, aWallTilePosition);
            result.Should().BeTrue();
        }
    }
}