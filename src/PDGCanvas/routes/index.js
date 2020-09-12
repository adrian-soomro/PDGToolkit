var express = require('express');
var router = express.Router();
const path = require('path');
const config = require('../config');

router.get('/', function(req, res, next) {
  res.render('index');
});

router.get('/data', function(req, res, next) {
  res.sendFile(path.resolve(config.relativePathToDungeon));
});


module.exports = router;
