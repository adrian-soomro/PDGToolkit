#!/bin/bash

rootDir=$PWD

# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out

cd ./out

ls

dotnet PDGToolkitCLI.dll -h
echo "####"

# run the toolkit
dotnet PDGToolkitCLI.dll

cd "$rootDir"
mv ../dungeon.json ./dungeon.json


cd ./src/PDGCanvas/
npm run start &
canvasPID=$!
echo "Starting up canvas ($canvasPID), giving it a minute to start up"
sleep 60
cd "$rootDir"

os=$(uname -s)
bash ./tests/PDGToolkit.CrossPlatformTests/saveDungeon.sh $os

ls $rootDir

# cleanup
kill $canvasPID