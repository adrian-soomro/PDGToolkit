const express = require('express');
const renderer = require('../application/renderer')
const router = express.Router();

router.get('/', function(req, res, next) {
  let dungeonImage = renderer.renderDungeonAsBase64EncodedImg()
  res.render('partials/main', { heading: "Generated Dungeon", img: dungeonImage });
});

module.exports = router;
