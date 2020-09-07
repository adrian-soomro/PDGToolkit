const colours = {
    GREY: 'rgb(211,211,211)',
    BLACK: 'rgb(0,0,0)',
    RED: 'rgb(204,0,0)',
}

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

    displayCells() {
        var ctx = canvas.getContext("2d");

        for (var y=0; y<this.height; y+=this.cellSize) {
            for (var x=0; x<this.width; x+=this.cellSize) {
                ctx.fillStyle = colours.GREY;
                ctx.fillRect(x, y, this.cellSize, this.cellSize);
            }
        }
    }
}



httpGetAsync("http://localhost:3000/data", (res) => { 
    console.log(JSON.parse(res))
    data = JSON.parse(res)
    var grid = new Grid(data.dungeon.x, data.dungeon.y, data.dungeon.cellSize);
    grid.setupCanvas();
    grid.displayCells();
    });

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