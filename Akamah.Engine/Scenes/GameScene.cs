namespace Akamah.Engine.Scenes;

/// <summary>
/// Main game scene with bouncing ball demo
/// </summary>
public class GameScene : Scene
{
  private Vector2 ballPosition;
  private Vector2 ballVelocity;
  private float ballRadius = 20f;
  private Color ballColor = Color.White;
  private int bounceCount = 0;

  public override void Initialize()
  {
    base.Initialize();
    Console.WriteLine("Game scene initialized");

    // Initialize ball at center of screen
    ballPosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
    ballVelocity = new Vector2(200f, 150f); // pixels per second
    bounceCount = 0;
  }

  public override void OnEnter()
  {
    Console.WriteLine("Entered game scene");
    // Reset ball position when entering scene
    ballPosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
    ballVelocity = new Vector2(200f, 150f);
    bounceCount = 0;
    ballColor = Color.White;
  }

  public override void OnExit()
  {
    Console.WriteLine("Exited game scene");
  }

  public override void HandleInput()
  {
    // Return to menu on ESC
    if (IsKeyPressed(KeyboardKey.Escape))
    {
      SceneManager.SwitchTo<MenuScene>();
    }

    // Reset ball position on SPACE
    if (IsKeyPressed(KeyboardKey.Space))
    {
      ballPosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
      ballVelocity = new Vector2(200f, 150f);
      bounceCount = 0;
      ballColor = Color.White;
    }

    // Change ball color on number keys
    if (IsKeyPressed(KeyboardKey.One)) ballColor = Color.Red;
    if (IsKeyPressed(KeyboardKey.Two)) ballColor = Color.Green;
    if (IsKeyPressed(KeyboardKey.Three)) ballColor = Color.Blue;
    if (IsKeyPressed(KeyboardKey.Four)) ballColor = Color.Yellow;
    if (IsKeyPressed(KeyboardKey.Five)) ballColor = Color.Magenta;
  }

  public override void Update(float deltaTime)
  {
    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Update ball position
    ballPosition += ballVelocity * deltaTime;

    // Bounce off screen edges
    bool bounced = false;
    if (ballPosition.X <= ballRadius || ballPosition.X >= screenWidth - ballRadius)
    {
      ballVelocity.X *= -1;
      bounced = true;
    }

    if (ballPosition.Y <= ballRadius || ballPosition.Y >= screenHeight - ballRadius)
    {
      ballVelocity.Y *= -1;
      bounced = true;
    }

    if (bounced)
    {
      bounceCount++;
      // Increase speed slightly on each bounce
      ballVelocity *= 1.02f;
    }

    // Keep ball within bounds
    ballPosition.X = Math.Clamp(ballPosition.X, ballRadius, screenWidth - ballRadius);
    ballPosition.Y = Math.Clamp(ballPosition.Y, ballRadius, screenHeight - ballRadius);
  }

  public override void Draw()
  {
    // Draw bouncing ball
    DrawCircleV(ballPosition, ballRadius, ballColor);

    // Draw ball trail effect
    float alpha = 0.3f;
    for (int i = 1; i <= 5; i++)
    {
      Vector2 normalizedVelocity = Vector2.Normalize(ballVelocity);
      Vector2 trailPos = ballPosition - (normalizedVelocity * i * 8);
      Color trailColor = ballColor;
      trailColor.A = (byte)(alpha * 255 / i);
      DrawCircleV(trailPos, ballRadius - i * 2, trailColor);
    }

    // Draw game UI
    DrawText("GAME SCENE", 10, 10, 24, Color.White);
    DrawText($"Bounces: {bounceCount}", 10, 40, 20, Color.LightGray);
    DrawText($"Speed: {ballVelocity.Length():F1}", 10, 65, 20, Color.LightGray);

    // Draw controls
    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    DrawText("ESC - Return to Menu", 10, screenHeight - 70, 16, Color.Gray);
    DrawText("SPACE - Reset Ball", 10, screenHeight - 50, 16, Color.Gray);
    DrawText("1-5 - Change Ball Color", 10, screenHeight - 30, 16, Color.Gray);

    // Draw FPS
    DrawText($"FPS: {GetFPS()}", screenWidth - 80, 10, 16, Color.Green);
  }

  public override void Unload()
  {
    Console.WriteLine("Game scene unloaded");
    base.Unload();
  }
}