namespace Akamah.Engine.Scenes;

public class ForestTile : Tile
{
  Random random = new();
  public override TileType Type { get; } = TileType.Forest;

  Tree? tree = null;

  public ForestTile() : base()
  {
    if (random.NextDouble() < 0.3)
    {
      this.tree = new Tree
      {
        Position = Position + new Vector2(4, 4)
      };
    }
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
    if (tree != null)
    {
      tree.Position = Position + new Vector2(4, 4);
    }
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.DarkGreen);
    tree?.Draw();
  }

  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Forest, 3.0f),    // High chance to cluster
      (TileType.Grass, 2.0f),     // Medium chance (forest edges)
      (TileType.Mountain, 1.5f),  // Medium chance (forested hills)
    ];
  }
}
