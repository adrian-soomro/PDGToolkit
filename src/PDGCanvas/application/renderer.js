const colours = require('./colours');

class Renderer {
    constructor() {
        this.canvas = document.getElementById("canvas");
    }

    setupCanvas(height, width) {
        canvas.height = height;
        canvas.width = width;
    }

    renderDungeon(grid) {
        this.setupCanvas(grid.height, grid.width)
        var canvasContext = canvas.getContext("2d");

        grid.tiles.forEach(tile => {
            canvasContext.fillStyle = colours.setColour(tile.type.name);
            canvasContext.fillRect(tile.position.x * grid.tileSize, tile.position.y * grid.tileSize, grid.tileSize, grid.tileSize);
        });
    }
}

module.exports = Renderer