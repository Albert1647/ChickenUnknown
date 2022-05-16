using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChickenUnknown.Managers;
using ChickenUnknown.GameObjects;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ChickenUnknown.Screen {
	class MenuScreen : IGameScreen {
        private SpriteFont Arial;
        private Texture2D StartButton, StartHover, CollectionButton, CollectionHover;
        private bool MouseOnStartButton, MouseOnCollectionButton, HoverStart, HoverCollection;

        private enum GameState{
            PLAYING, WINNING, LOSING
        }
        
        private enum PlayingState{
            PLAYING, LEVELUP, GACHA
        }

        public void Initial() {

		}
		public override void LoadContent() {
			base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");
            StartButton = Content.Load<Texture2D>("MenuScreen/start_button");
            StartHover = Content.Load<Texture2D>("MenuScreen/start_button_hover");
            CollectionButton = Content.Load<Texture2D>("MenuScreen/collection_button");
            CollectionHover = Content.Load<Texture2D>("MenuScreen/collection_button_hover");
            Initial();
		}
		public override void UnloadContent() {
			base.UnloadContent();
		}
		public override void Update(GameTime gameTime) {
            // Save Current Mouse Position
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
            Singleton.Instance.MouseCurrent = Mouse.GetState();

           	// Check mouse on UI
            if(MouseOnElement(760, 760+400, 510,510+60)){
                MouseOnStartButton = true;
                if(HoverStart == false){
                    HoverStart = true;
                }
                if(IsClick()){
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.GameScreen);
                }
            } else {
                MouseOnStartButton = false;
                HoverStart = false;
            }
            if(MouseOnElement(760, 760+400, 600,600+60)){
                MouseOnCollectionButton = true;
                if (HoverCollection == false)
                {
                    HoverCollection = true;
                }
                if (IsClick()){
                    // Singleton.Instance.ToggleFullscreen();
                    //ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.SettingScreen);
                }
            } else {
                MouseOnCollectionButton = false;
                HoverCollection = false;
            }

			base.Update(gameTime);
		}
		public override void Draw(SpriteBatch _spriteBatch) {
            
            // Swap Texture If mouseHover 
            if(MouseOnStartButton) {
                _spriteBatch.Draw(StartHover, CenterElementWithHeight(StartHover,510) , Color.White);
            }
            else {
                _spriteBatch.Draw(StartButton, CenterElementWithHeight(StartButton,510) , Color.White);
            }
            if(MouseOnCollectionButton) {
                _spriteBatch.Draw(CollectionHover, CenterElementWithHeight(CollectionHover,600) , Color.White);
            }
            else {
                _spriteBatch.Draw(CollectionButton, CenterElementWithHeight(CollectionButton,600) , Color.White);
            }
            _spriteBatch.DrawString(Arial, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(Arial, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Click " + Singleton.Instance._graphics.IsFullScreen, new Vector2(0,40), Color.Black);

		}
        
        // if mouse on specify 'location/position'
        public bool MouseOnElement(int x1, int x2, int y1, int y2){
            return (Singleton.Instance.MouseCurrent.X > x1 && Singleton.Instance.MouseCurrent.Y > y1) && (Singleton.Instance.MouseCurrent.X < x2 && Singleton.Instance.MouseCurrent.Y < y2);
        }
        // helper function to shorten singleton calling
         public bool IsClick(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released;
        }
        // CenterElement at specify height
        public Vector2 CenterElementWithHeight(Texture2D element,int height){
            return new Vector2(Singleton.Instance.Dimension.X / 2 - (element.Width / 2) ,height );
        }
	}
}
