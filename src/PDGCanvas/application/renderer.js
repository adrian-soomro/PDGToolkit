const { createCanvas } = require('canvas')
const config = require('../config')
const path = require('path');
const fs = require('fs');
const colours = require('./colours')

function getDungeonData() {
  let rawDungeonData = fs.readFileSync(path.resolve(config.relativePathToDungeon))
  let dungeonJson = JSON.parse(rawDungeonData)
  return dungeonJson
}

function render(dungeon) {
  let canvas = createCanvas(dungeon.width, dungeon.height)
  let ctx = canvas.getContext('2d')

  dungeon.tiles.forEach(tile => {
    ctx.fillStyle = colours.setColour(tile.type.name);
    ctx.fillRect(tile.position.x * dungeon.tileConfig.size, tile.position.y * dungeon.tileConfig.size, dungeon.tileConfig.size, dungeon.tileConfig.size);
  })
  return canvas.toDataURL()
}

module.exports = {
  renderDungeonAsBase64EncodedImg: () => {
    let dungeonData = getDungeonData()
    return render(dungeonData)
  }
}