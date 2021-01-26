const express = require('express');
const renderer = require('../application/renderer')
const colours = require('../application/colours')
const router = express.Router();

router.get('/', function(req, res, next) {
  let dungeonImage = renderer.renderDungeonAsBase64EncodedImg()
  let colourMapping = colours.getColourMapping()
  res.render('partials/main', { heading: "Generated Dungeon", img: dungeonImage, colourMapping: colourMapping });
});

module.exports = router;
