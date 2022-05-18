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
        private Texture2D StartButton, StartHover, SettingButton, SettingHover, ExitButton, ExitHover,
        BG,Title;
        private bool MouseOnStartButton, MouseOnSettingButton, MouseOnExitButton, HoverStart, HoverSetting, HoverExit;

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
            BG = Content.Load<Texture2D>("MenuScreen/draft_menu_bg");
            Title = Content.Load<Texture2D>("MenuScreen/draft_game_title");
            StartButton = Content.Load<Texture2D>("MenuScreen/draft_start_button");
            StartHover = Content.Load<Texture2D>("MenuScreen/draft_start_button_hover");
            SettingButton = Content.Load<Texture2D>("MenuScreen/draft_option_button");
            SettingHover = Content.Load<Texture2D>("MenuScreen/draft_option_button_hover");
            ExitButton = Content.Load<Texture2D>("MenuScreen/draft_exit_button");
            ExitHover = Content.Load<Texture2D>("MenuScreen/draft_exit_button_hover");
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
            CheckMouseUI();
            
			base.Update(gameTime);
		}
		public override void Draw(SpriteBatch _spriteBatch) {
            
            //BG
            _spriteBatch.Draw(BG, new Vector2(0,0) , Color.White);
            //Title
            _spriteBatch.Draw(Title, new Vector2(638,178) , Color.White);
            DarwButton(_spriteBatch);

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
        public void DarwButton(SpriteBatch _spriteBatch){
            // Swap Texture If mouseHover 
            //Start
            if(MouseOnStartButton) {
                _spriteBatch.Draw(StartHover, CenterElementWithHeight(StartHover,492) , Color.White);
            }
            else {
                _spriteBatch.Draw(StartButton, CenterElementWithHeight(StartButton,492) , Color.White);
            }
            //Setting
            if(MouseOnSettingButton) {
                _spriteBatch.Draw(SettingHover, CenterElementWithHeight(SettingHover,628) , Color.White);
            }
            else {
                _spriteBatch.Draw(SettingButton, CenterElementWithHeight(SettingButton,628) , Color.White);
            }
            //Exit
            if(MouseOnExitButton) {
                _spriteBatch.Draw(ExitHover, CenterElementWithHeight(SettingHover,764) , Color.White);
            }
            else {
                _spriteBatch.Draw(ExitButton, CenterElementWithHeight(SettingButton,764) , Color.White);
            }
        }
        public void CheckMouseUI(){
            //Start
            if(MouseOnElement(787, 787+349, 492,492+96)){
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
            //Setting
            if(MouseOnElement(787, 787+349, 628,628+96)){
                MouseOnSettingButton = true;
                if (HoverSetting == false)
                {
                    HoverSetting = true;
                }
                if (IsClick()){
                    // Singleton.Instance.ToggleFullscreen();
                    //ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.SettingScreen);
                }
            } else {
                MouseOnSettingButton = false;
                HoverSetting = false;
            }
            //Exit
            if(MouseOnElement(787, 787+349, 764,764+96)){
                MouseOnExitButton = true;
                if (HoverExit == false)
                {
                    HoverExit = true;
                }
                if (IsClick()){
                    //Exit game
                }
            } else {
                MouseOnExitButton = false;
                HoverExit = false;
            }
        }
	}
}
