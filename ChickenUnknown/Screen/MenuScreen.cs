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
        private SoundEffect Click,HoverMenu;
        private bool MouseOnStartButton, MouseOnSettingButton, MouseOnExitButton, HoverStart, HoverSetting, HoverExit;
        public void Initial() {

		}
		public override void LoadContent() {
			base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");
            BG = Content.Load<Texture2D>("MenuScreen/bg");
            Title = Content.Load<Texture2D>("MenuScreen/logo");
            StartButton = Content.Load<Texture2D>("MenuScreen/start");
            StartHover = Content.Load<Texture2D>("MenuScreen/start_hover");
            SettingButton = Content.Load<Texture2D>("MenuScreen/setting");
            SettingHover = Content.Load<Texture2D>("MenuScreen/setting_hover");
            ExitButton = Content.Load<Texture2D>("MenuScreen/exit");
            ExitHover = Content.Load<Texture2D>("MenuScreen/exit_hover");
            //Sound
            Click = Content.Load<SoundEffect>("Sound/Click");
            HoverMenu = Content.Load<SoundEffect>("Sound/MenuSelect");
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
            _spriteBatch.Draw(BG, new Vector2(0,0) , Color.White);
            _spriteBatch.Draw(Title, new Vector2(638,178) , Color.White);
            DrawButton(_spriteBatch);
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
        public void DrawButton(SpriteBatch _spriteBatch){
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
                    HoverMenu.Play();
                    HoverStart = true;
                }
                if(IsClick()){
                    Click.Play();
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
                {   HoverMenu.Play();
                    HoverSetting = true;
                }
                if (IsClick()){
                    Click.Play();
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.SettingScreen);
                }
            } else {
                MouseOnSettingButton = false;
                HoverSetting = false;
            }
            //Exit
            if(MouseOnElement(787, 787+349, 764,764+96)){
                MouseOnExitButton = true;
                if (HoverExit == false)
                {   HoverMenu.Play();
                    HoverExit = true;
                }
                if (IsClick()){
                    Click.Play();
                    Singleton.Instance.isExit = true;
                }
            } else {
                MouseOnExitButton = false;
                HoverExit = false;
            }
        }
	}
}
