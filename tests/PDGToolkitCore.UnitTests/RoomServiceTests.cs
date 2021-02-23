using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PDGToolkitCore.Application;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.UnitTests
{
    [TestFixture]
    public class RoomServiceTests
    {
        private IRoomService roomService;
        private List<Room> rooms;
        
        [SetUp]
        public async Task Init()
        {
          // mock settings with an in-memory configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"outputRelativePath", "foo"},
                {"grid:width", "1280"},
                {"grid:height", "720"},
                {"tiles:size", "720"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
            roomService = new RoomService(new Random(), new Settings(configuration));
            
            var room1 = await RoomBuilder.Create().WithHeight(10).WithWidth(10).WithStartingPosition(new Position(0, 0))
                .WithInsideTilesOfType(TileType.Floor).WithOutsideWalls().BuildAsync();
            
            var room2 = await RoomBuilder.Create().WithHeight(10).WithWidth(10).WithStartingPosition(new Position(5, 5))
                .WithInsideTilesOfType(TileType.Floor).WithOutsideWalls().BuildAsync();
            
            var room3 = await RoomBuilder.Create().WithHeight(5).WithWidth(5).WithStartingPosition(new Position(25, 25))
                .WithInsideTilesOfType(TileType.Floor).WithOutsideWalls().BuildAsync();
            rooms = new List<Room>{room1, room2, room3};
        }
        
        [Test]
        public void SharedPosition_Returns_ParentRooms()
        {
            var foundRooms = roomService.GetRoomsByPosition(rooms, new Position(5, 5));

            var foundRoomsIds = foundRooms.Select(r => r.Id);
            foundRooms.Count().Should().Be(2);
            foundRoomsIds.Should().BeEquivalentTo(new List<Guid>{rooms[0].Id, rooms[1].Id});
        }
        
        [Test]
        public void PositionInsideOneRoom_Returns_ParentRoom()
        {
            var foundRooms = roomService.GetRoomsByPosition(rooms, new Position(0, 0));
            
            var foundRoomId = foundRooms.Select(r => r.Id).First();
            
            foundRooms.Count().Should().Be(1);
            foundRoomId.Should().Be(rooms.First().Id);
        }

        [Test]
        public void PositionOutsideRooms_Returns_EmptyGuid()
        {
            var foundRooms = roomService.GetRoomsByPosition(rooms, new Position(99, 99));

            var foundRoomId = foundRooms.Select(r => r.Id).FirstOrDefault();
            
            foundRooms.Count().Should().Be(0);
            foundRoomId.Should().BeEmpty();
        }

        [Test]
        public void MergingRooms_Succeeds()
        {
            var newRoom = roomService.MergeRooms(rooms[0], rooms[1]);
            
            newRoom.Id.Should().Be(rooms[0].Id);
            newRoom.Tiles.Count().Should().BeLessThan(rooms[0].Tiles.Count + rooms[1].Tiles.Count);
        }
        
        [Test]
        public void MergingRooms_EqualityHolds()
        {
            var uniqueRooms = new HashSet<Room>{rooms[0], rooms[1]};
            var newRoom = roomService.MergeRooms(rooms[0], rooms[1]);
            
            var insertSuccessful = uniqueRooms.Add(newRoom);

            insertSuccessful.Should().BeFalse();
        }

        [Test]
        public void OverlappingRooms_Return_True()
        {
            var result = roomService.AreRoomsOverlapping(rooms[0], rooms[1]);
            result.Should().BeTrue();
        }
        
        [Test]
        public void SeparatedRooms_Return_False()
        {
            var result = roomService.AreRoomsOverlapping(rooms[0], rooms[2]);
            result.Should().BeFalse();
        }
    }
}