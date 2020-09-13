const colours = require('./colours');

class Renderer {
    constructor() {
        this.canvas = document.getElementById("canvas");
    }

    setupCanvas(height, width) {
        canvas.height= height;
        canvas.width= width;
    }

    renderDungeon(grid) {
        this.setupCanvas(grid.height, grid.width)
        var canvasContext = canvas.getContext("2d");

        grid.tiles.forEach(tile => {
            canvasContext.fillStyle = colours.setColour(tile.Type.Name);
            canvasContext.fillRect(tile.Position.X * grid.cellSize, tile.Position.Y * grid.cellSize, grid.cellSize, grid.cellSize);
        });
    }
}

module.exports = Renderer