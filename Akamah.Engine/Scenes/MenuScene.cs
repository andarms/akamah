namespace Akamah.Engine.Scenes;

/// <summary>
/// Main menu scene
/// </summary>
public class MenuScene : Scene
{
  private float titlePulse = 0f;
  private int selectedOption = 0;
  private readonly string[] menuOptions = { "Start Game", "Settings", "Exit" };

  public override void Initialize()
  {
    base.Initialize();
    Console.WriteLine("Menu scene initialized");
  }

  public override void OnEnter()
  {
    Console.WriteLine("Entered menu scene");
    selectedOption = 0;
  }

  public override void OnExit()
  {
    Console.WriteLine("Exited menu scene");
  }

  public override void HandleInput()
  {
    // Navigate menu with arrow keys
    if (IsKeyPressed(KeyboardKey.Up))
    {
      selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
    }
    else if (IsKeyPressed(KeyboardKey.Down))
    {
      selectedOption = (selectedOption + 1) % menuOptions.Length;
    }
    else if (IsKeyPressed(KeyboardKey.Enter))
    {
      HandleMenuSelection();
    }
  }

  private void HandleMenuSelection()
  {
    switch (selectedOption)
    {
      case 0: // Start Game
        SceneManager.SwitchTo<GameScene>();
        break;
      case 1: // Settings
              // Could change to settings scene
        Console.WriteLine("Settings not implemented yet");
        break;
      case 2: // Exit
              // Exit the game - you might want to add a proper exit method to Game class
        Environment.Exit(0);
        break;
    }
  }

  public override void Update(float deltaTime)
  {
    titlePulse += deltaTime * 2f; // Animate title
  }

  public override void Draw()
  {
    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Draw animated title
    float titleScale = 1.0f + (float)Math.Sin(titlePulse) * 0.1f;
    string title = "AKAMAH ENGINE";
    int titleWidth = (int)(MeasureText(title, 48) * titleScale);
    Vector2 titlePos = new Vector2((screenWidth - titleWidth) / 2f, screenHeight * 0.2f);

    // Draw title shadow
    DrawText(title, (int)titlePos.X + 3, (int)titlePos.Y + 3, (int)(48 * titleScale), Color.DarkGray);
    // Draw title
    DrawText(title, (int)titlePos.X, (int)titlePos.Y, (int)(48 * titleScale), Color.White);

    // Draw menu options
    float startY = screenHeight * 0.5f;
    for (int i = 0; i < menuOptions.Length; i++)
    {
      Color textColor = (i == selectedOption) ? Color.Yellow : Color.LightGray;
      string option = (i == selectedOption) ? $"> {menuOptions[i]} <" : menuOptions[i];

      int textWidth = MeasureText(option, 24);
      Vector2 textPos = new Vector2((screenWidth - textWidth) / 2f, startY + i * 50);

      DrawText(option, (int)textPos.X, (int)textPos.Y, 24, textColor);
    }

    // Draw instructions
    string instruction = "Use UP/DOWN arrows to navigate, ENTER to select";
    int instrWidth = MeasureText(instruction, 16);
    DrawText(instruction, (screenWidth - instrWidth) / 2, screenHeight - 50, 16, Color.Gray);
  }

  public override void Unload()
  {
    Console.WriteLine("Menu scene unloaded");
    base.Unload();
  }
}