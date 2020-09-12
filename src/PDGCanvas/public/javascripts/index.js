const colours = {
    GREY: 'rgb(211,211,211)',
    BLACK: 'rgb(0,0,0)',
    RED: 'rgb(204,0,0)',
}

const setColour = (tileType) => ({
    "Floor": colours.GREY,
    "Wall": colours.BLACK,
    "Obstacle": colours.RED
  })[tileType]

class Cell {
    constructor(x, y, cellSize, colour) {
        this.x = x;
        this.y = y;
        this.cellSize = cellSize
        this.colour = colour;
    }
}

class Grid {
    constructor(width, height, cellSize) {
        this.width = width,
        this.height = height
        this.cellSize = cellSize
        this.canvas = document.getElementById("canvas");
    }

    setupCanvas() {
        canvas.height= this.height;
        canvas.width= this.width;
    }

    renderDungeon(tiles) {
        var ctx = canvas.getContext("2d");

        tiles.forEach(tile => {
            ctx.fillStyle = setColour(tile.Type.Name);
            ctx.fillRect(tile.Position.X * this.cellSize, tile.Position.Y * this.cellSize, this.cellSize, this.cellSize);
        });
    }
}

function httpGetAsync(theUrl, callback)
{
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() { 
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
    }
    xmlHttp.open("GET", theUrl, true); // true for asynchronous 
    xmlHttp.send(null);
}

httpGetAsync("http://localhost:3000/data", (response) => { 
    data = JSON.parse(response)
    var grid = new Grid(data.Width, data.Height, data.TileConfig.Height);
    grid.setupCanvas();
    grid.renderDungeon(data.Tiles)
});
