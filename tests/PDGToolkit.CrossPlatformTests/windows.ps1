$rootDir=Get-Location
# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out
cd ./out
# run the toolkit
dotnet PDGToolkitCLI.dll
cd "$rootDir"
mv ../dungeon.json ./dungeon.json
# start the canvas in the background
cd ./src/PDGCanvas/
$canvas = Start-Job -ScriptBlock {npm run start }
Write-Host "Starting up canvas... giving it a minute to start up"
Start-Sleep -s 60
cd "$rootDir"
# save the dungeon
./tests/PDGToolkit.CrossPlatformTests/saveDungeon.ps1
# cleanup
Remove-Job -Job $canvas -Force