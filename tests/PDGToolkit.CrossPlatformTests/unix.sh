#!/bin/bash

# go to the root of the repo


# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out

cd ./out

ls

dotnet PDGToolkitCLI.dll -h
echo "####"

# run the toolkit
dotnet PDGToolkitCLI.dll -p 'dungeon'

#echo "[ ] should've finished the execution, taking a nap."
#sleep 30

echo "### PDGToolkit/out###"
ls


echo "### PDGToolkit###"
ls ../
echo "### PDGToolkit/PDGToolkit###"
ls ../..

echo "### ../PDGToolkit/PDGToolkit###"
ls ../../..

#cat ../dungeon.json
# if dungeon.json exists, spin up canvas, load the page, fetch the image and persist it.