const express = require('express');
const renderer = require('../application/renderer')
const colours = require('../application/colours')
const router = express.Router();
const {exec} = require('child_process');

function regenerateDungeon(res) {
  exec('./start-toolkit.sh', (err, stdout, stderr) => {
    if (err) {
      console.error(err)
    }
    let dungeonImage = renderer.renderDungeonAsBase64EncodedImg()
    let colourMapping = colours.getColourMapping()
    res.render('partials/main', { heading: "Generated Dungeon", img: dungeonImage, colourMapping: colourMapping });
  });
}

router.get('/', function(req, res, next) {
  let dungeonImage = renderer.renderDungeonAsBase64EncodedImg()
  let colourMapping = colours.getColourMapping()
  res.render('partials/main', { heading: "Generated Dungeon", img: dungeonImage, colourMapping: colourMapping });
});

router.post('/', function(req, res, next) {
  regenerateDungeon(res);
});

module.exports = router;
