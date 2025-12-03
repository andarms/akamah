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
  public (TileType, List<TileType>)[] Generation { get; } = new (TileType, List<TileType>)[width * height];

  public List<int> queue = [];

  PerlinNoise heightNoise = new(12345);
  PerlinNoise moistureNoise = new(54321);

  public int Width { get; } = width;
  public int Height { get; } = height;

  public Vector2 Limits { get; } = new(width * 16, height * 16);


  public void GenerateRandomMap()
  {
    Array.Fill(Tiles, null);
    var seed = DateTime.Now.Millisecond;
    heightNoise = new PerlinNoise(seed);
    moistureNoise = new PerlinNoise(seed + 1);

    float scale = 0.05f;
    for (int x = 0; x < Width; x++)
    {
      for (int y = 0; y < Height; y++)
      {
        float heightValue = heightNoise.Noise(x * scale, y * scale);
        float moistureValue = moistureNoise.Noise(x * scale, y * scale);


        var biome = GetBiome(heightValue, moistureValue);
        int index = y * Width + x;
        // Initially, all tile types are possible
        Tile tile = GetTileFromType(biome);
        tile.Position = new Vector2(x * 16, y * 16);
        Tiles[index] = tile;
      }
    }
  }


  TileType GetBiome(float height, float moisture)
  {
    // Normalize values to 0-1 range for better control
    height = Math.Clamp(height, 0f, 1f);
    moisture = Math.Clamp(moisture, 0f, 1f);

    // Water bodies - deep water and shallow coastal areas
    if (height < 0.25f)
      return TileType.Water;

    // Coastal and desert areas - influenced by both height and moisture
    if (height < 0.35f)
    {
      // Very dry coastal areas become sand, moist ones stay water-adjacent
      return moisture < 0.3f ? TileType.Sand : TileType.Water;
    }

    // Beach and desert transition zone
    if (height < 0.45f)
    {
      // Dry areas are sandy, moderate moisture creates grassland, high moisture creates marsh-like grass
      if (moisture < 0.4f)
        return TileType.Sand;
      else
        return TileType.Grass;
    }

    // Mid-elevation biomes - most diverse area
    if (height < 0.70f)
    {
      if (moisture < 0.25f)
        return TileType.Sand; // Arid grasslands/steppes
      else if (moisture < 0.45f)
        return TileType.Grass; // Temperate grasslands
      else if (moisture < 0.75f)
        return TileType.Forest; // Temperate forests
      else
        return TileType.Forest; // Dense forests/rainforests
    }

    // High elevation areas
    if (height < 0.85f)
    {
      if (moisture < 0.35f)
        return TileType.Mountain; // Arid highlands/mesas
      else if (moisture < 0.65f)
        return TileType.Grass; // Alpine meadows
      else
        return TileType.Forest; // Mountain forests
    }

    // Peak elevations - mostly mountains with some variation
    if (moisture > 0.8f && height < 0.95f)
      // return TileType.Forest; // High-altitude cloud forests
      return TileType.Mountain; // High-altitude cloud forests
    else
      return TileType.Mountain; // Rocky peaks and alpine zones
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

  public override void Draw()
  {
    foreach (var tile in Tiles)
    {
      if (tile == null) continue;
      tile.Draw();
    }
  }

  public override void Update(float deltaTime)
  {
    foreach (var tile in Tiles)
    {
      if (tile == null) continue;
      tile.Update(deltaTime);
    } // Map-specific update logic can go here
  }
}
