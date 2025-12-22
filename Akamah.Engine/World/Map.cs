using Akamah.Engine.Core.Camera;
using Akamah.Engine.Core.Engine;
using Akamah.Engine.Gameplay.World.Flora;
using Akamah.Engine.Gameplay.World.Minerals;
using Akamah.Engine.Systems;
using Akamah.Engine.Systems.Spatial;
using Akamah.Engine.World.Generation;
using Akamah.Engine.World.Tiles;

namespace Akamah.Engine.World;


public class Map(int width, int height) : GameObject
{
  public Tile[] Tiles { get; } = new Tile[width * height];
  public (TileType, List<TileType>)[] Generation { get; } = new (TileType, List<TileType>)[width * height];

  public List<int> queue = [];

  PerlinNoise heightNoise = new(GameManager.Seed);
  PerlinNoise moistureNoise = new(GameManager.Seed + 1);

  public int Width { get; } = width;
  public int Height { get; } = height;

  public Vector2 Limits { get; } = new(width * 16, height * 16);


  public void GenerateRandomMap()
  {
    // Properly remove all trees using GameManager.RemoveGameObject to clean up spatial systems
    foreach (var obj in GameManager.GameObjects.ToArray())
    {
      if (obj is Tree || obj is Rock)
      {
        GameManager.RemoveGameObject(obj);
      }
    }
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
        Tile tile = GetTileFromType(biome, new Vector2(x * 16, y * 16));
        Tiles[index] = tile;
      }
    }

    this.Initialize();

    // Resize spatial grid to match current map dimensions
    SpatialManager.ResizeToMapDimensions();
  }

  public override void Initialize()
  {
    foreach (var tile in Tiles)
    {
      if (tile == null) continue;
      tile.Initialize();
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


  public static Tile GetTileFromType(TileType type, Vector2 position)
  {
    return type switch
    {
      TileType.Grass => new GrassTile() { Position = position },
      TileType.Water => new WaterTile() { Position = position },
      TileType.Sand => new SandTile() { Position = position },
      TileType.Mountain => new MountainTile() { Position = position },
      TileType.Forest => new ForestTile() { Position = position },
      _ => new GrassTile() { Position = position },
    };
  }

  public override void Draw()
  {
    // Get the visible tile range based on camera viewport
    var (minX, minY, maxX, maxY) = ViewportManager.GetVisibleTileRange(16);

    // Only render tiles within the visible range
    for (int x = minX; x <= maxX; x++)
    {
      for (int y = minY; y <= maxY; y++)
      {
        int index = y * Width + x;
        if (index >= 0 && index < Tiles.Length)
        {
          var tile = Tiles[index];
          tile?.Draw();
        }
      }
    }
  }

  public override void Update(float deltaTime)
  {
    // Get the visible tile range for updates (with larger margin for gameplay logic)
    var (minX, minY, maxX, maxY) = ViewportManager.GetVisibleTileRange(16);

    // Expand the update range slightly beyond visible area for smooth gameplay
    minX = Math.Max(0, minX - 2);
    minY = Math.Max(0, minY - 2);
    maxX = Math.Min(Width - 1, maxX + 2);
    maxY = Math.Min(Height - 1, maxY + 2);

    // Only update tiles within the expanded visible range
    for (int x = minX; x <= maxX; x++)
    {
      for (int y = minY; y <= maxY; y++)
      {
        int index = y * Width + x;
        if (index >= 0 && index < Tiles.Length)
        {
          var tile = Tiles[index];
          tile?.Update(deltaTime);
        }
      }
    }
  }
}
