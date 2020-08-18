# PDGToolkit - a Toolkit for Procedural Dungeon Generation

Cross platform, game-engine independant .Net Core 3 Application for procedurally generating 2D dungeons from a configuration file

## Requirements
- [.Net Core 3.1.301+](https://dotnet.microsoft.com/download)
  
**You can use either the latest version of Visual Studio or .NET CLI for Windows, Mac and Linux**.


## Getting started

1. Compile the projects and their dependencies
   ```sh
    dotnet publish -c Release -o ./out FYP.sln 
   ```
   
2. Run the project
   ```sh
   dotnet out/PDGToolkitAPI.dll
   ```
   