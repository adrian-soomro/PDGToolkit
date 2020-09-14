# PDGToolkit - a Toolkit for Procedural Dungeon Generation

Cross platform, game-engine independant .Net Core 3 Application for procedurally generating 2D dungeons from a configuration file

## Requirements
- [.Net Core 3.1.301+](https://dotnet.microsoft.com/download)
- [Node v12.18.3+](https://nodejs.org/en/)
  
**You can use either the latest version of Visual Studio or .NET CLI for Windows, Mac and Linux**.


## Getting started

### Toolkit

From the root of this repository:

1. Compile the projects and their dependencies
   ```sh
   dotnet publish -c Release -o ./out FYP.sln 
   ```
   
2. Run the project
   ```sh
   dotnet out/PDGToolkitAPI.dll
   ```

### Canvas

From the src/PDGCanvas directory:

1. Install node dependencies

   ```node
   npm install
   ```

2. Start the application

   ```node
   npm run start
   ```

3. View the generated dungeon
   
   With a web browser of your choice, navigate to http://localhost:3000

