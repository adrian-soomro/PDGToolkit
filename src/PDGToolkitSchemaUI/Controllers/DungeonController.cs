using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitSchemaUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DungeonController : ControllerBase
    {
        /// <summary>
        /// Schema of the end product - a serialised Dungeon, represented by the <see cref="Grid"/> class.
        /// For information on the dungeon schema, see the "Schemas" section below.
        /// </summary>
        /// <remarks>Please note that this site is not designed to serve as the point of entry, but rather as mere documentation.
        /// for a way to interact, please refer to the CLI.
        /// </remarks>
        [HttpGet]
        public async Task<Grid> GetGridAsync()
        {
            return await Task.Run(() => new Grid(0, 0, new TileConfig(0), new List<Tile>()));
        }
    }
}