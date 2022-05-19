using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChickenUnknown.Managers;
using ChickenUnknown.GameObjects;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ChickenUnknown.Screen {
	class SettingScreen : IGameScreen {
        private SpriteFont Arial;

        public Texture2D ArrowLeft,ArrowRight,SettingBar,SettingClose,SettingDraft,SettingFrame,SettingPointer;
        private SoundEffect Click,HoverMenu;

        private Song ThemeSong;

        private bool  HoverButton1,HoverButton2,HoverButton3,HoverButton4,CloseButton;
        
        public void Initial() {

		}
		public override void LoadContent() {
			base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");

            ArrowLeft = Content.Load<Texture2D>("SettingScreen/setting_arrow_left");
            ArrowRight = Content.Load<Texture2D>("SettingScreen/setting_arrow_right");
            SettingBar = Content.Load<Texture2D>("SettingScreen/setting_bar");
            SettingClose = Content.Load<Texture2D>("SettingScreen/setting_close");
            SettingDraft = Content.Load<Texture2D>("SettingScreen/setting_draft");
            SettingFrame = Content.Load<Texture2D>("SettingScreen/setting_frame");
            SettingPointer = Content.Load<Texture2D>("SettingScreen/setting_pointer");
            //Sound
            ThemeSong = Content.Load<Song>("Sound/ThemeTest");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume=0.2f;
            MediaPlayer.Play(ThemeSong);
            Click = Content.Load<SoundEffect>("Sound/Click");
            HoverMenu = Content.Load<SoundEffect>("Sound/MenuSelect");

            Initial();
		}
        
		public override void Update(GameTime gameTime) {
            // Save Current Mouse Position
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
            Singleton.Instance.MouseCurrent = Mouse.GetState();
            ButtonsUpdate();

			base.Update(gameTime);
		}
		public override void Draw(SpriteBatch _spriteBatch) {
            DrawHUD(_spriteBatch);
            DrawLog(_spriteBatch);
		}
        
        public void DrawHUD(SpriteBatch _spriteBatch){
        _spriteBatch.Draw(SettingDraft, CenterElementWithHeight(SettingDraft,0) , Color.White);
        
        //_spriteBatch.Draw(SettingFrame, CenterElementWithHeight(SettingFrame,0) , Color.White);
        _spriteBatch.Draw(SettingBar, new Rectangle(611, 328, SettingBar.Width, SettingBar.Height), Color.White);  
        _spriteBatch.Draw(ArrowLeft, new Rectangle(555, 314, ArrowLeft.Width, ArrowLeft.Height), Color.White);  
        _spriteBatch.Draw(ArrowRight, new Rectangle(1309, 314, ArrowRight.Width, ArrowRight.Height), Color.White);
        _spriteBatch.Draw(SettingBar, new Rectangle(611, 497, SettingBar.Width, SettingBar.Height), Color.White);  
        _spriteBatch.Draw(ArrowLeft, new Rectangle(555, 483, ArrowLeft.Width, ArrowLeft.Height), Color.White);  
        _spriteBatch.Draw(ArrowRight, new Rectangle(1309, 483, ArrowRight.Width, ArrowRight.Height), Color.White);
        _spriteBatch.Draw(SettingClose, new Rectangle(836, 903, SettingClose.Width, SettingClose.Height), Color.White);
        }

        public void DrawLog(SpriteBatch _spriteBatch){
            _spriteBatch.DrawString(Arial, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(Arial, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Click " + IsClick(), new Vector2(0,40), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Dragging " + IsDragging(), new Vector2(0,60), Color.Black);
            _spriteBatch.DrawString(Arial, "SFXVolume = " + Singleton.Instance.SFXVolume, new Vector2(0, 80), Color.Black);
            _spriteBatch.DrawString(Arial, "MusicVolume = " + Singleton.Instance.MusicVolume , new Vector2(0,100), Color.Black);

        }
        public void ButtonsUpdate(){
            if(MouseOnElement(555, 555+56, 314,314+56)){
                if(HoverButton1 == false){
                    HoverMenu.Play();
                    HoverButton1 = true;
                }
                if(IsClick()){
                    Click.Play();
                    if(Singleton.Instance.SFXVolume>0){
                    Singleton.Instance.SFXVolume-=5;
                     if(Singleton.Instance.SFXVolume<0){Singleton.Instance.SFXVolume=0;}
                    }
                    SoundEffect.MasterVolume = ((float)(Singleton.Instance.SFXVolume)/100);
                }
            } else {
                HoverButton1 = false;
            }
            if(MouseOnElement(1309, 1309+56, 314,314+56)){
                if(HoverButton2 == false){
                    HoverMenu.Play();
                    HoverButton2 = true;
                }
                if(IsClick()){
                    Click.Play();
                    if(Singleton.Instance.SFXVolume<100){
                    Singleton.Instance.SFXVolume+=5;
                     if(Singleton.Instance.SFXVolume>100){Singleton.Instance.SFXVolume=100;}
                    }
                    SoundEffect.MasterVolume = ((float)(Singleton.Instance.SFXVolume)/100);
                }
            } else{
                HoverButton2 = false;
            }
            if(MouseOnElement(555, 555+56, 483,483+56)){
                if(HoverButton3 == false){
                    HoverMenu.Play();
                    HoverButton3 = true;
                }
                if(IsClick()){
                    Click.Play();
                    if(Singleton.Instance.MusicVolume>0){
                    Singleton.Instance.MusicVolume-=5;
                     if(Singleton.Instance.MusicVolume<0){Singleton.Instance.MusicVolume=0;}
                    }
                    MediaPlayer.Volume=((float)(Singleton.Instance.MusicVolume)/100)*0.5f;
                }
            } else {
                HoverButton3 = false;
            }
            if(MouseOnElement(1309, 1309+56, 483,483+56)){
                if(HoverButton4 == false){
                    HoverMenu.Play();
                    HoverButton4 = true;
                }
                if(IsClick()){
                    Click.Play();
                    if(Singleton.Instance.MusicVolume<100){
                    Singleton.Instance.MusicVolume+=5;
                     if(Singleton.Instance.MusicVolume>100){Singleton.Instance.MusicVolume=100;}
                    }
                    MediaPlayer.Volume=((float)(Singleton.Instance.MusicVolume)/100)*0.5f;
                }
            } else {
                HoverButton4 = false;
            }
            if(MouseOnElement(836, 836+248, 903,903+248)){
                if(CloseButton == false){
                    HoverMenu.Play();
                    CloseButton = true;
                }
                if(IsClick()){
                    Click.Play();
                    MediaPlayer.Pause();
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                }
            } else {
                CloseButton = false;
            }

        }
        // if mouse on specify 'location/position'
        public bool MouseOnElement(int x1, int x2, int y1, int y2){
            return (Singleton.Instance.MouseCurrent.X > x1 && Singleton.Instance.MouseCurrent.Y > y1) && (Singleton.Instance.MouseCurrent.X < x2 && Singleton.Instance.MouseCurrent.Y < y2);
        }
        // helper function to shorten singleton calling
         public bool IsClick(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released;
        }
        public bool IsDragging(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed;
        }
        // CenterElement at specify height
        public Vector2 CenterElementWithHeight(Texture2D element,int height){
            return new Vector2(Singleton.Instance.Dimension.X / 2 - (element.Width / 2) ,height );
        }
	}
}
