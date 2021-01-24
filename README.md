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

# Schema overview

<h2 id="tocS_TileConfig">TileConfig</h2>
<!-- backwards compatibility -->
<a id="schematileconfig"></a>
<a id="schema_TileConfig"></a>
<a id="tocStileconfig"></a>
<a id="tocstileconfig"></a>

```json
{
  "size": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|size|integer(int32)|false|read-only|none|

<h2 id="tocS_TileType">TileType</h2>
<!-- backwards compatibility -->
<a id="schematiletype"></a>
<a id="schema_TileType"></a>
<a id="tocStiletype"></a>
<a id="tocstiletype"></a>

```json
{
  "name": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|name|string¦null|false|read-only|none|

<h2 id="tocS_Position">Position</h2>
<!-- backwards compatibility -->
<a id="schemaposition"></a>
<a id="schema_Position"></a>
<a id="tocSposition"></a>
<a id="tocsposition"></a>

```json
{
  "x": 0,
  "y": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|x|integer(int32)|false|read-only|none|
|y|integer(int32)|false|read-only|none|

<h2 id="tocS_Tile">Tile</h2>
<!-- backwards compatibility -->
<a id="schematile"></a>
<a id="schema_Tile"></a>
<a id="tocStile"></a>
<a id="tocstile"></a>

```json
{
  "type": {
    "name": "string"
  },
  "position": {
    "x": 0,
    "y": 0
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|type|[TileType](#schematiletype)|false|none|none|
|position|[Position](#schemaposition)|false|none|none|

<h2 id="tocS_Grid">Grid</h2>
<!-- backwards compatibility -->
<a id="schemagrid"></a>
<a id="schema_Grid"></a>
<a id="tocSgrid"></a>
<a id="tocsgrid"></a>

```json
{
  "height": 0,
  "width": 0,
  "tileConfig": {
    "size": 0
  },
  "tiles": [
    {
      "type": {
        "name": "string"
      },
      "position": {
        "x": 0,
        "y": 0
      }
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|height|integer(int32)|false|read-only|none|
|width|integer(int32)|false|read-only|none|
|tileConfig|[TileConfig](#schematileconfig)|false|none|none|
|tiles|[[Tile](#schematile)]¦null|false|read-only|none|

