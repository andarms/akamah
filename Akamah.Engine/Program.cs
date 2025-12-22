using Akamah.Engine.Core.Engine;
using Akamah.Engine.Core.Scene;
using Akamah.Engine.Scenes;

SceneManager.AddScene(new GameScene());
SceneManager.SwitchTo<GameScene>();

var game = new Game();
game.Run();
