import Grid from './grid';
import Renderer from './renderer';

const request = require('request');

request('http://localhost:3000/data', { json: true }, (err, res, body) => {
  if (err) { return console.log(err); }
  var grid = new Grid(body.Width, body.Height, body.TileConfig.Size, body.Tiles);
  var renderer = new Renderer()
  renderer.renderDungeon(grid)
});
