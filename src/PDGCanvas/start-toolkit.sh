toolkitBuildDir="out"
currentDir=$CWD
# go to the root of the repo
cd ../../

# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output "./${toolkitBuildDir}"

cd $toolkitBuildDir

# start the toolkit
./PDGToolkitCLI -g TeenyZonesGenerator -s JsonSerialiser

# go back to the starting point
cd $currentDir 