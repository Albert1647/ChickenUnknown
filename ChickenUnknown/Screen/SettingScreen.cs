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
        private SpriteFont Pixeloid;

        public Texture2D ArrowLeft,ArrowRight,SettingBar,SettingClose,SettingCloseHover,BG,SettingFrame,SettingPointer;
        private SoundEffect Click,HoverMenu;

        public double PostSFXVol,PostMusicVol;
        public int PosPointerSFX,PosPointerMusic,TempMouseCurrentX,NewMouseCurrentX;

        private bool  HoverButton1,HoverButton2,HoverButton3,HoverButton4,CloseButton,IsDragPointer1,IsDragPointer2;

        public float tempsfx,tempmusic;
        
        public void Initial() {

		}
		public override void LoadContent() {
			base.LoadContent();
            Pixeloid = Content.Load<SpriteFont>("Pixeloid");

            ArrowLeft = Content.Load<Texture2D>("SettingScreen/setting_arrow_left");
            ArrowRight = Content.Load<Texture2D>("SettingScreen/setting_arrow_right");
            SettingBar = Content.Load<Texture2D>("SettingScreen/setting_bar");
            SettingClose = Content.Load<Texture2D>("SettingScreen/setting_close");
            SettingCloseHover = Content.Load<Texture2D>("SettingScreen/close_hover");
            BG = Content.Load<Texture2D>("SettingScreen/bg");
            SettingFrame = Content.Load<Texture2D>("SettingScreen/setting_frame");
            SettingPointer = Content.Load<Texture2D>("SettingScreen/setting_pointer");
            //Sound
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
            //DrawLog(_spriteBatch);
		}
        
        public void DrawHUD(SpriteBatch _spriteBatch){
            _spriteBatch.Draw(BG, CenterElementWithHeight(BG,0) , Color.White);
            
            _spriteBatch.Draw(SettingFrame, CenterElementWithHeight(SettingFrame,86) , Color.White);
            _spriteBatch.DrawString(Pixeloid, "Sound Effect Volume : " +(int)(Singleton.Instance.SFXVolume) , new Vector2(555,246), Color.Black);
            _spriteBatch.Draw(SettingBar, new Rectangle(611, 328, SettingBar.Width, SettingBar.Height), Color.White);  
            _spriteBatch.Draw(ArrowLeft, new Rectangle(565, 314, ArrowLeft.Width, ArrowLeft.Height), Color.White);  
            _spriteBatch.Draw(ArrowRight, new Rectangle(1309, 314, ArrowRight.Width, ArrowRight.Height), Color.White);
            PostSFXVol=(Singleton.Instance.SFXVolume)*6.98;
            PosPointerSFX=((int)(PostSFXVol-14))+611;
            _spriteBatch.Draw(SettingPointer, new Rectangle(PosPointerSFX, 314, SettingPointer.Width, SettingPointer.Height), Color.White);
            _spriteBatch.DrawString(Pixeloid, "Music Volume : " + (int)(Singleton.Instance.MusicVolume) , new Vector2(555,415), Color.Black);
            _spriteBatch.Draw(SettingBar, new Rectangle(611, 497, SettingBar.Width, SettingBar.Height), Color.White);  
            _spriteBatch.Draw(ArrowLeft, new Rectangle(565, 483, ArrowLeft.Width, ArrowLeft.Height), Color.White);  
            _spriteBatch.Draw(ArrowRight, new Rectangle(1309, 483, ArrowRight.Width, ArrowRight.Height), Color.White);
            PostMusicVol=Singleton.Instance.MusicVolume*6.98;
            PosPointerMusic=((int)(PostMusicVol-14))+611;
            _spriteBatch.Draw(SettingPointer, new Rectangle(PosPointerMusic, 483, SettingPointer.Width, SettingPointer.Height), Color.White);
            if(CloseButton!){_spriteBatch.Draw(SettingCloseHover, new Rectangle(836, 780, SettingClose.Width, SettingClose.Height), Color.White);
            }else{_spriteBatch.Draw(SettingClose, new Rectangle(836, 780, SettingClose.Width, SettingClose.Height), Color.White);}
        }

        public void DrawLog(SpriteBatch _spriteBatch){
            //_spriteBatch.DrawString(Pixeloid, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            //_spriteBatch.DrawString(Pixeloid, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "Is Click " + IsClick(), new Vector2(0,40), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "Is Dragging " + IsDragging(), new Vector2(0,60), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "SFXVolume = " + Singleton.Instance.SFXVolume, new Vector2(0, 80), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "MusicVolume = " + Singleton.Instance.MusicVolume , new Vector2(0,100), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "PosPointerSFX = " + PosPointerSFX , new Vector2(0,120), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "tempMouseCurrentX = " + TempMouseCurrentX , new Vector2(0,160), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "tempsfx = " + tempsfx , new Vector2(0,180), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "NewMouseCurrentX = " + NewMouseCurrentX , new Vector2(0,200), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "IsDragPointer = " + IsDragPointer1 , new Vector2(0,220), Color.Black);
        }
        public void ButtonsUpdate(){
            if(MouseOnElement(565, 565+56, 314,314+56)){
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
            if(MouseOnElement(565, 565+56, 483,483+56)){
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
            if(MouseOnElement(836, 836+248, 780,780+248)){
                if(CloseButton == false){
                    HoverMenu.Play();
                    CloseButton = true;
                }
                if(IsClick()){
                    Click.Play();
                    //MediaPlayer.Pause();
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                }
            } else {
                CloseButton = false;
            }
            if(MouseOnElement(PosPointerSFX, PosPointerSFX+28, 314,314+55)||MouseOnElement(611,1309,328,356)||IsDragPointer1){
                //OnPointer1=true;
                if(IsClick()){
                    if(IsDragPointer1 == false){
                    TempMouseCurrentX=Singleton.Instance.MouseCurrent.X;
                    Click.Play();
                    IsDragPointer1 = true;
                }


                }else {
                //IsDragPointer = false;
            }     
                if(IsDragPointer1&&IsDragging()){
                    NewMouseCurrentX=Singleton.Instance.MouseCurrent.X;
                    tempsfx=(NewMouseCurrentX-TempMouseCurrentX)-14;
                    tempsfx=(float)(((double)(((TempMouseCurrentX+tempsfx)-611)+14))/6.98);
                    if(tempsfx<0){tempsfx=0;}else if(tempsfx>100){tempsfx=100;}
                    Singleton.Instance.SFXVolume=tempsfx;
                    SoundEffect.MasterVolume = ((float)(Singleton.Instance.SFXVolume)/100);
                    }else {
                    IsDragPointer1 = false;
            }    

            } else {
               //OnPointer1 = false;
            }
            if(MouseOnElement(PosPointerMusic, PosPointerMusic+28, 483,483+55)||MouseOnElement(611,1309,497,525)||IsDragPointer2){
                //OnPointer1=true;
                if(IsClick()){
                    if(IsDragPointer2 == false){
                    TempMouseCurrentX=Singleton.Instance.MouseCurrent.X;
                    Click.Play();
                    IsDragPointer2 = true;
                }


                }else {
                //IsDragPointer = false;
            }     
                if(IsDragPointer2&&IsDragging()){
                    NewMouseCurrentX=Singleton.Instance.MouseCurrent.X;
                    tempmusic=(NewMouseCurrentX-TempMouseCurrentX)-14;
                    tempmusic=(float)(((double)(((TempMouseCurrentX+tempmusic)-611)+14))/6.98);
                    if(tempmusic<0){tempmusic=0;}else if(tempmusic>100){tempmusic=100;}
                    Singleton.Instance.MusicVolume=tempmusic;
                    MediaPlayer.Volume=((float)(Singleton.Instance.MusicVolume)/100)*0.5f;
                    }else {
                    IsDragPointer2 = false;
            }    

            } else {
               //OnPointer1 = false;
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
