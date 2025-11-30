namespace Akamah.Engine.Scenes;


[Flags]
public enum TileType
{
  None,
  Grass,
  Sand,
  Water,
  Mountain,
  Forest
}

public class Map(int width, int height) : GameObject
{
  public Tile[] Tiles { get; } = new Tile[width * height];
  public TileType[] Generation { get; } = new TileType[width * height];

  public List<int> queue = [];


  const int INVALID_NEIGHBOR = -1;
  private static Random random = new();

  public void GenerateRandomMap()
  {
    Array.Fill(Tiles, null);
    Array.Fill(Generation, TileType.None);
    // for (int x = 0; x < width; x++)
    // {
    //   for (int y = 0; y < height; y++)
    //   {
    //     GenerateTileAtPosition(x, y);
    //   }
    // }

    int index = random.Next(0, width * height);
    GenerateTileAtPosition(index % width, index / width);


    while (queue.Count > 0)
    {
      int qIndex = 0;
      int tileIndex = queue[qIndex];
      queue.RemoveAt(qIndex);

      if (Generation[tileIndex] != TileType.None) continue;

      GenerateTileAtPosition(tileIndex % width, tileIndex / width);
    }

  }

  private void GenerateTileAtPosition(int x, int y)
  {
    int index = y * width + x;
    if (Generation[index] == TileType.None)
    {
      Tile tile = GetRandomTile();
      tile.Position = new Vector2(x * 16, y * 16);
      Tiles[index] = tile;
      Generation[index] = tile.Type;
    }

    var neighbors = NeighborIndices(index);
    List<TileType> minValidNeighbors = [];
    foreach (var neighborIndex in neighbors)
    {
      if (neighborIndex == INVALID_NEIGHBOR) continue;
      Tile neighborTile = Tiles[neighborIndex];

      if (neighborTile == null)
      {
        queue.Add(neighborIndex);
        continue;
      }


      var valid = neighborTile.ValidNeighbors();
      if (minValidNeighbors.Count == 0 || valid.Count < minValidNeighbors.Count)
      {
        minValidNeighbors = valid;
      }
    }

    if (minValidNeighbors.Count > 0)
    {
      Generation[index] = TileType.Sand;
      TileType type = minValidNeighbors[random.Next(minValidNeighbors.Count)];
      Tiles[index] = GetTileFromType(type);
      Tiles[index].Position = new Vector2(x * 16, y * 16);
    }
  }

  public static Tile GetRandomTile()
  {
    int tile = GetRandomValue(0, 4);
    return tile switch
    {
      0 => new GrassTile(),
      1 => new WaterTile(),
      2 => new WaterTile(),
      3 => new ForestTile(),
      4 => new MountainTile(),
      _ => new GrassTile(),
    };
  }


  public static Tile GetTileFromType(TileType type)
  {
    return type switch
    {
      TileType.Grass => new GrassTile(),
      TileType.Water => new WaterTile(),
      TileType.Sand => new SandTile(),
      TileType.Mountain => new MountainTile(),
      TileType.Forest => new ForestTile(),
      _ => new GrassTile(),
    };
  }

  private static readonly (int dx, int dy)[] Directions =
  [
    (-1, 0), // Left
    (1, 0),  // Right
    (0, -1), // Up
    (0, 1)   // Down
  ];

  int GetIndex(int x, int y)
  {
    return y * width + x;
  }

  int[] NeighborIndices(int index)
  {
    int x = index % width;
    int y = index / width;

    List<int> neighbors = new(4);

    foreach (var (dx, dy) in Directions)
    {
      int nx = x + dx;
      int ny = y + dy;


      if (nx >= 0 && nx < width && ny >= 0 && ny < height)
      {
        neighbors.Add(GetIndex(nx, ny));
      }
      else
      {
        neighbors.Add(INVALID_NEIGHBOR);
      }
    }

    return [.. neighbors];
  }

  public override void Draw()
  {
    foreach (var tile in Tiles)
    {
      if (tile == null) continue;
      tile.Draw();
    }
    // int len = Enum.GetValues<TileType>().Length;

    // for (int x = 0; x < width; x++)
    // {
    //   for (int y = 0; y < height; y++)
    //   {
    //     DrawText($"{len}", x * 16, y * 16, 16, Color.Black);
    //   }
    // }
  }

  public override void Update(float deltaTime)
  {
    // Map-specific update logic can go here
  }
}
