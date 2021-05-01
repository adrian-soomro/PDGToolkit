#!/bin/bash

rootDir=$PWD
# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out
cd ./out
# run the toolkit
dotnet PDGToolkitCLI.dll
cd "$rootDir"
mv ../dungeon.json ./dungeon.json
# start the canvas in the background
cd ./src/PDGCanvas/
npm run start &
canvasPID=$!
echo "Starting up canvas ($canvasPID), giving it a minute to start up"
sleep 60
cd "$rootDir"
# fetch OS, to distinquish between Darwin(MacOS) and Linux results
os=$(uname -s)
bash ./tests/PDGToolkit.CrossPlatformTests/saveDungeon.sh $os
# cleanup
kill $canvasPID
