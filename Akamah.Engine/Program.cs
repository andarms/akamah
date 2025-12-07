using Akamah.Engine.Core;
using Akamah.Engine.Scenes;

SceneManager.AddScene(new GameScene());
SceneManager.SwitchTo<GameScene>();

var game = new Game();
game.Run();
