using System.Linq;

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


public enum Directions
{
  Left = 0,
  Right,
  Up,
  Down,
  TopLeft,
  TopRight,
  BottomLeft,
  BottomRight
}

public class Map(int width, int height) : GameObject
{
  public Tile[] Tiles { get; } = new Tile[width * height];
  public (TileType, List<TileType>)[] Generation { get; } = new (TileType, List<TileType>)[width * height];

  public List<int> queue = [];


  const int INVALID_NEIGHBOR = -1;
  private static Random random = new();

  public void GenerateRandomMap()
  {
    Array.Fill(Tiles, null);
    // Initialize Generation with None type and all possible neighbors
    for (int i = 0; i < Generation.Length; i++)
    {
      Generation[i] = (TileType.None, [TileType.Grass, TileType.Sand, TileType.Water, TileType.Mountain, TileType.Forest]);
    }

    int index = random.Next(0, width * height);
    GenerateTileAtPosition(index % width, index / width);


    while (queue.Count > 0)
    {
      int qIndex = 0;
      int tileIndex = queue[qIndex];
      queue.RemoveAt(qIndex);

      if (Generation[tileIndex].Item1 != TileType.None) continue;

      GenerateTileAtPosition(tileIndex % width, tileIndex / width);
    }

  }

  private void GenerateTileAtPosition(int x, int y)
  {
    int index = y * width + x;

    // If this position hasn't been set yet, choose a tile based on constraints
    if (Generation[index].Item1 == TileType.None)
    {
      var (_, possibleTypes) = Generation[index];

      if (possibleTypes.Count == 0)
      {
        // No valid options, use fallback
        var tile = new GrassTile();
        tile.Position = new Vector2(x * 16, y * 16);
        Tiles[index] = tile;
        Generation[index] = (tile.Type, []);
      }
      else
      {
        // Select from possible types using weights
        var weightsForPossibleTypes = GetWeightsForTypes(possibleTypes);
        TileType selectedType = SelectWeightedNeighbor(weightsForPossibleTypes);

        var tile = GetTileFromType(selectedType);
        tile.Position = new Vector2(x * 16, y * 16);
        Tiles[index] = tile;
        Generation[index] = (selectedType, []);
      }
    }

    // Now propagate constraints to neighbors
    var neighbors = NeighborIndices(index);
    var currentTileValidNeighbors = Tiles[index].ValidNeighbors();

    foreach (var neighborIndex in neighbors)
    {
      if (neighborIndex == INVALID_NEIGHBOR) continue;

      var (neighborType, neighborPossibleTypes) = Generation[neighborIndex];

      // If neighbor is already set, skip
      if (neighborType != TileType.None) continue;

      // Constrain neighbor's possible types based on current tile
      var validTypesFromCurrent = currentTileValidNeighbors.Select(n => n.type).ToList();
      var newPossibleTypes = neighborPossibleTypes.Intersect(validTypesFromCurrent).ToList();

      // If constraints changed, update and add to queue
      if (!newPossibleTypes.SequenceEqual(neighborPossibleTypes))
      {
        Generation[neighborIndex] = (TileType.None, newPossibleTypes);
        if (!queue.Contains(neighborIndex))
        {
          queue.Add(neighborIndex);
        }
      }
    }
  }

  public static Tile GetRandomTile()
  {
    int tile = GetRandomValue(0, 4);
    return tile switch
    {
      0 => new GrassTile(),
      1 => new SandTile(),
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

  private static List<(TileType type, float weight)> GetWeightsForTypes(List<TileType> types)
  {
    // Create a sample tile to get default weights, then filter for our types
    var result = new List<(TileType type, float weight)>();

    foreach (var type in types)
    {
      // Use default weight of 1.0 for each type when selecting from constraints
      // The actual neighbor weights are used during constraint propagation
      result.Add((type, 1.0f));
    }

    return result;
  }

  private static TileType SelectWeightedNeighbor(List<(TileType type, float weight)> validNeighbors)
  {
    if (validNeighbors.Count == 1)
      return validNeighbors[0].type;

    // Calculate total weight
    float totalWeight = validNeighbors.Sum(n => n.weight);

    // Generate random value between 0 and totalWeight
    float randomValue = (float)(random.NextDouble() * totalWeight);

    // Select based on weight
    float currentWeight = 0;
    foreach (var (type, weight) in validNeighbors)
    {
      currentWeight += weight;
      if (randomValue <= currentWeight)
      {
        return type;
      }
    }

    // Fallback (should rarely happen)
    return validNeighbors[^1].type;
  }

  private static readonly (int dx, int dy)[] NeighborPositions =
  [
    (-1, 0), // Left
    (1, 0),  // Right
    (0, -1), // Up
    (0, 1),   // Down
    (-1, -1), // Top-Left
    (1, -1),  // Top-Right
    (-1, 1), // Bottom-Left
    (1, 1)   // Bottom-Right
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

    foreach (var (dx, dy) in NeighborPositions)
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
