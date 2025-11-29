namespace Akamah.Engine.Scenes;

public class Map(int width, int height) : GameObject
{
  public List<Tile> Tiles { get; } = [];

  public void GenerateRandomMap()
  {
    Tiles.Clear();
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Tile tile = GetRandomTile();
        tile.Position = new Vector2(x * 16, y * 16);
        Tiles.Add(tile);
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

  public override void Draw()
  {
    foreach (var tile in Tiles)
    {
      tile.Draw();
    }
  }

  public override void Update(float deltaTime)
  {
    // Map-specific update logic can go here
  }
}
