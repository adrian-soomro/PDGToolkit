# PDGToolkit - a Toolkit for Procedural Dungeon Generation

Cross platform, game-engine independant .Net Core 3 Application for procedurally generating 2D dungeons from a configuration file

## Requirements
- [.Net Core 3.1.301+](https://dotnet.microsoft.com/download)
- [Node v12.18.3+](https://nodejs.org/en/)
  
**You can use either the latest version of Visual Studio, JetBrain's Rider or .NET CLI for Windows, Mac and Linux**.


## Getting started

### Toolkit

From the root of this repository:

1. Build the project into a directory of your liking
   
   ```sh
   dotnet publish ./src/PDGToolkitCLI/PDGToolkitCLI.csproj -c Debug --output <YOUR-PATH>
   ```
2. Run the CLI executable
   
    ```sh
    ./<YOUR-PATH>/PDGToolkitCLI
    ```

**Note: The CLI takes additional options, to view them, add the '--help' flag while executing the CLI**

  ```sh
  ./<YOUR-PATH>/PDGToolkitCLI
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

### Schema UI
To view the schema of the dungeons that the Toolkit produces, build and run the PDGToolkitUI project.

From the root of this repository:

```sh
   dotnet run --project ./src/PDGToolkitSchemaUI/PDGToolkitSchemaUI.csproj
```

You should now be able to access the swagger page at http://localhost:1337/swagger/index.html

### Unit tests
To run Core's unit test, use the dotnet test command.

From the root of this repository:
```sh
dotnet test ./tests/PDGToolkitCore.UnitTests/PDGToolkitCore.UnitTests.csproj
```

