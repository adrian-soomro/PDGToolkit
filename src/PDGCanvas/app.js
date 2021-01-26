const path = require('path');
const express = require('express');
const app = express();
const handlebars = require('express-handlebars');
const indexRouter = require('./routes/index')
const fs = require('fs')
const config = require('./config');

//Sets our app to use the handlebars engine
app.set('view engine', 'hbs');
app.engine('hbs', handlebars({
  layoutsDir: __dirname + '/views/layouts',
  partialsDir: __dirname + '/views/partials',
  extname: 'hbs',
  defaultLayout: 'index.hbs',
  }));
//Serves static files (we need it to import a css file)
app.use(express.static(path.join(__dirname, 'public')));
//app.use(express.urlencoded()); // to support URL-encoded bodies

app.use('/', indexRouter)


/**
 * Make sure the dungeon exists prior to starting the web server
 */
let pathToDungeon = path.resolve(config.relativePathToDungeon);
fs.stat(pathToDungeon, function(err, stat) {
  if(err != null && err.code === 'ENOENT') {
    console.log(`Coun't find the file containing the dungeon at ${pathToDungeon}.\nPlease make sure you've generated a dungeon before trying to run the canvas again.`)
    process.exit(1);
  } 
});

//Makes the app listen on port 3000
app.listen(config.port, () => console.log(`App listening on port ${config.port}`));