using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChickenUnknown.GameObjects;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using ChickenUnknown.Managers;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ChickenUnknown.Screen {
    class GameScreen : _GameScreen {
        private SpriteFont Arial;
        public Texture2D _rectTexture, ChickenTexture, IndicatorTexture;
        public Rectangle rect;
        private Chicken chicken;

        public float Timer = 0f;
        public TimeSpan TimeSpan;
        string answerTime;

        public void Initial() {
            // Instantiate gun on start GameScreen 
            // chicken = new Chicken(ChickenTexture, IndicatorTexture){
            //     pos = new Vector2(Singleton.Instance.Dimension.X / 2 - GunTexture.Width / 2, 700 - GunTexture.Height),
            // };
        }
        public override void LoadContent() {
            // Load Resource
            base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");
            var width = 1000;
            var height = 20;
            _rectTexture = new Texture2D(getGraphicDevice().GraphicsDevice, width, height);
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i)
                data[i] = Color.Orange;
            _rectTexture.SetData(data);

            rect = new Rectangle(0,0,100,100);
            Initial();
        }
        public override void UnloadContent() {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime) {
             base.Update(gameTime);
             Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
             Singleton.Instance.MouseCurrent = Mouse.GetState();
             rect = new Rectangle(0,0,100 + (int)Timer * 5,100);
             //Time
             Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
             TimeSpan = TimeSpan.FromSeconds(Timer);
             answerTime = string.Format("{0:D2}:{1:D2}:{2:D2}", //for example if you want Millisec => "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms"  ,t.Milliseconds
                TimeSpan.Hours, 
                TimeSpan.Minutes, 
                TimeSpan.Seconds);
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.DrawString(Arial, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(Arial, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Click " + IsClick(), new Vector2(0,40), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Dragging " + IsDragging(), new Vector2(0,60), Color.Black);
            _spriteBatch.DrawString(Arial, "Time : " + answerTime, new Vector2(0,80), Color.Black);
            _spriteBatch.Draw(_rectTexture, new Vector2(100, 100) ,rect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public GraphicsDeviceManager getGraphicDevice(){
            return Singleton.Instance.gdm;
        }
        public bool MouseOnTexture(int StartX, int StartY, Texture2D texture){
            return (Singleton.Instance.MouseCurrent.X > StartX && Singleton.Instance.MouseCurrent.Y > StartY) && (Singleton.Instance.MouseCurrent.X < StartX + texture.Width && Singleton.Instance.MouseCurrent.Y < StartY + texture.Height);
        }
         public bool MouseOnElement(int x1, int x2, int y1, int y2){
            return (Singleton.Instance.MouseCurrent.X > x1 && Singleton.Instance.MouseCurrent.Y > y1) && (Singleton.Instance.MouseCurrent.X < x2 && Singleton.Instance.MouseCurrent.Y < y2);
        }
        public bool IsClick(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released;
        }
        public bool IsDragging(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed;
        }
        public Vector2 CenterElementWithHeight(Texture2D element,int height){
            return new Vector2(Singleton.Instance.Dimension.X / 2 - (element.Width / 2) ,height );
        }
    }
}
