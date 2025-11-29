namespace Akamah.Engine.Scenes;

public abstract class Tile : GameObject
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Black);
  }
}

public class GrassTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Green);
  }
}

public class SandTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Yellow);
  }
}

public class WaterTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Blue);
  }
}

public class ForestTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.DarkGreen);
  }
}

public class MountainTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Gray);
  }
}


public class GameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    GenerateMap();
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

  public void GenerateMap()
  {
    GameObjects.Clear();
    for (int x = 0; x < 80; x++)
    {
      for (int y = 0; y < 45; y++)
      {
        Tile tile = GetRandomTile();
        tile.Position = new Vector2(x * 16, y * 16);
        GameObjects.Add(tile);
      }
    }
  }


  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      GenerateMap();
    }
  }

  public override void Unload()
  {
    Console.WriteLine("Game scene unloaded");
    base.Unload();
  }
}