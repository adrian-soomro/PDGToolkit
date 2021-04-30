#!/bin/bash

# go to the root of the repo
cd ../../

# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out

cd ./out

# run the toolkit
./PDGToolkitCLI

ls

ls ../

# if dungeon.json exists, spin up canvas, load the page, fetch the image and persist it.