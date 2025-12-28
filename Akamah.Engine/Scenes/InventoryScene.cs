using Akamah.Engine.Engine.Scene;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  Color backgroundColor = Fade(Color.Gray, 0.5f);
  public InventoryScene()
  {
  }


  public override void Unload()
  {
    // Clean up inventory resources
  }

  public override void Update(float gameTime)
  {
    // base.Update(gameTime);
  }

  public override void Draw()
  {
    DrawRectangleV(new(0, 0), new(12800, 720), backgroundColor);
  }
}