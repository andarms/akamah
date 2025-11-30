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
      tree.Position = Position;
    }
  }

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(16, 0, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
    tree?.Draw();
  }
}
