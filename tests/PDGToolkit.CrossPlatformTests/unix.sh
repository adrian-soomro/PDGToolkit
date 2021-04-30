#!/bin/bash

# go to the root of the repo


# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out

cd ./out

ls

./PDGToolkitCLI -h
echo "####"
# run the toolkit
./PDGToolkitCLI

echo "[ ] should've finished the execution, taking a nap."
sleep 30

ls

echo "####"

ls ../

echo "###"

cat ../dungeon.json
# if dungeon.json exists, spin up canvas, load the page, fetch the image and persist it.