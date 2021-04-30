$rootDir=Get-Location

# build the toolkit
dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output ./out

cd ./out

ls

# run the toolkit
dotnet PDGToolkitCLI.dll

cd "$rootDir"
mv ../dungeon.json ./dungeon.json


cd ./src/PDGCanvas/
$canvas = Start-Job -ScriptBlock {npm run start }
Write-Host "Starting up canvas... giving it a minute to start up"
Start-Sleep -s 60
cd "$rootDir"

### Save 
./tests/PDGToolkit.CrossPlatformTests/saveDungeon.ps1

ls $rootDir

# cleanup
Remove-Job -Job $canvas -Force