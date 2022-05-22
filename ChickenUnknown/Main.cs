using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChickenUnknown.Managers;

namespace ChickenUnknown
{
    public class Main : Game
    {
        //private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public Main()
        {
            Singleton.Instance._graphics = new GraphicsDeviceManager(this);
            Singleton.Instance.gdm = Singleton.Instance._graphics;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Apply Game resolution
            Singleton.Instance._graphics.PreferredBackBufferWidth = (int)Singleton.Instance.Dimension.X;  // set this value to the desired width of your window
            Singleton.Instance._graphics.PreferredBackBufferHeight = (int)Singleton.Instance.Dimension.Y;   // set this value to the desired height of your window
            Singleton.Instance._graphics.ApplyChanges();

             Singleton.Instance._graphics.ToggleFullScreen();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
        }

        protected override void UnloadContent() {
			ScreenManager.Instance.UnloadContent();
		}

        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.previousKB = Singleton.Instance.currentKB;
            Singleton.Instance.currentKB = Keyboard.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Singleton.Instance.isExit){
                Exit();
            }

            // Update logic base on CurrentScreen
            ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // Draw Screen base on CurrentScreen
            ScreenManager.Instance.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        
    }
}
