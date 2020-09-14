import Renderer from './renderer';

const request = require('request');

request('http://localhost:3000/data', { json: true }, (err, res, body) => {
  if (err) { return console.log(err); }

  var grid = {
    width: body.width,
    height: body.height,
    tileSize: body.tileConfig.size,
    tiles: body.tiles
  }

  var renderer = new Renderer()
  renderer.renderDungeon(grid)
});
