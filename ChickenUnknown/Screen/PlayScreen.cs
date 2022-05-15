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
    class PlayScreen : IGameScreen {
        private SpriteFont Arial;
        public Texture2D ExpBarRectangle, SwingTexture, ChickenTexture, StretchAreaTexture, ZombieTexture,
                        bg, bg1, barricade, slingshot, wall;
        public Rectangle ExpBarRect;
        private Chicken chicken;
        private Swing swing;
        private Zombie zombie;
        // private Zombie zombie;
        public float Timer = 0f;
        public TimeSpan TimeSpan;
        string answerTime;
        public bool lvlup = false; 
        public string[] allPower = {"scale","quantity","cooldown","damage"};
        public string[] canSelectPower = {};
        public String power_random_one , power_random_two , power_random_three,
                        selectPower;
        
        private int MaxWidth = 300, Level = 0;

        public void Initial() {
            swing = new Swing(SwingTexture, ChickenTexture, StretchAreaTexture) {
                
            };
            zombie = new Zombie(ZombieTexture) {
                
            };
        }
        public override void LoadContent() {
            // Load Resource
            base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");

            SwingTexture = Content.Load<Texture2D>("PlayScreen/draft_slingshot");
            StretchAreaTexture = Content.Load<Texture2D>("PlayScreen/draft_slingshot_stretcharea");
            ChickenTexture = Content.Load<Texture2D>("PlayScreen/draft_chicken");
            ZombieTexture = Content.Load<Texture2D>("PlayScreen/draft_zombie_nm");

            bg = Content.Load<Texture2D>("PlayScreen/draft_bg");
            bg1 = Content.Load<Texture2D>("PlayScreen/draft_ingame");
            barricade = Content.Load<Texture2D>("PlayScreen/draft_barricade");
            wall = Content.Load<Texture2D>("PlayScreen/draft_wall");
            SetExpbar();
            Initial();
        }
        public override void UnloadContent() {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            GetMouseInput();   
             
            swing.Update(gameTime);
            //  zombie.Update(gameTime);
            UpdateExpBar(gameTime);
            UpdateDisplayTime();
            
            //LevelupRandomPower
            LevelupRandomPower();
            //select power
            if(MouseOnElement(387, 387+253, 758,758+92)){
                selectPower = power_random_one;
                updatePower(selectPower);
            }else if(MouseOnElement(833, 833+253, 758,758+92)){
                selectPower = power_random_two;
                updatePower(selectPower);
            }else if(MouseOnElement(1282, 1282+253, 758,758+92)){
                selectPower = power_random_three;
                updatePower(selectPower);
            }

            UpdateExpBar(gameTime);
            UpdateDisplayTime();
             
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            DrawGameElement(_spriteBatch);
            DrawLog(_spriteBatch);
            swing.Draw(_spriteBatch, Arial);
            //  zombie.Update(gameTime);
        }

        public void DrawLog(SpriteBatch _spriteBatch){
            _spriteBatch.DrawString(Arial, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(Arial, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Click " + IsClick(), new Vector2(0,40), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Dragging " + IsDragging(), new Vector2(0,60), Color.Black);
            _spriteBatch.DrawString(Arial, "Time : " + answerTime, new Vector2(915,50), Color.Black);
            _spriteBatch.DrawString(Arial, "Level : " + Level, new Vector2(0,200), Color.Black);
            _spriteBatch.DrawString(Arial, "Exp : " + Singleton.Instance.Exp, new Vector2(0,220), Color.Black);
            _spriteBatch.DrawString(Arial, "MaxWidth : " + MaxWidth, new Vector2(0,240), Color.Black);
            _spriteBatch.DrawString(Arial, "MaxExp : " +Singleton.Instance.MaxExp , new Vector2(0,260), Color.Black);
            _spriteBatch.DrawString(Arial, "RectWidth : " +ExpBarRect.Width , new Vector2(0,280), Color.Black);
            _spriteBatch.DrawString(Arial, "KUY : " +(float)(Singleton.Instance.Exp/Singleton.Instance.MaxExp)*MaxWidth , new Vector2(0,300), Color.Black);
        }

        public void DrawGameElement(SpriteBatch _spriteBatch){
            _spriteBatch.Draw(bg, CenterElementWithHeight(bg,0) , Color.White);
            // _spriteBatch.Draw(bg1, CenterElementWithHeight(bg1,0) , Color.White);
            _spriteBatch.Draw(wall, new Rectangle(173, UI.FLOOR_Y-378, wall.Width, wall.Height), Color.White);
            _spriteBatch.Draw(barricade, new Rectangle(403, UI.FLOOR_Y-108, barricade.Width, barricade.Height), Color.White);
            _spriteBatch.Draw(SwingTexture, new Rectangle(185, UI.FLOOR_Y-378-216, SwingTexture.Width, SwingTexture.Height), Color.White);
            _spriteBatch.Draw(ExpBarRectangle, new Vector2(100, 100) ,ExpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        public void UpdateDisplayTime(){
            TimeSpan = TimeSpan.FromSeconds(Timer);
            answerTime = string.Format("{0:D2}:{1:D2}", //for example if you want Millisec => "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms"  ,t.Milliseconds
                TimeSpan.Minutes, 
                TimeSpan.Seconds);
        }
        public void UpdateExpBar(GameTime gameTime){
            Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.O) && Singleton.Instance.previousKB.IsKeyDown(Keys.O)) {                
                Singleton.Instance.Exp += 50; 
            }
            ExpBarRect.Width = (int)(((float)(Singleton.Instance.Exp/Singleton.Instance.MaxExp))* MaxWidth);
            if (ExpBarRect.Width > MaxWidth) {
                ExpBarRect.Width = 0;
                Level += 1;
                Singleton.Instance.Exp = 0;
            }
        }
        public void updatePower(String power){
            switch (power) {
                case "scale":
                    Singleton.Instance.scale =+ 1;
                    break;
                case "quantity":
                    Singleton.Instance.quantity =+ 1;
                    break;
                case "cooldown":
                    Singleton.Instance.cooldown =+ 1;
                    break;
                case "damage":
                    Singleton.Instance.damage =+ 1;
                    break;
            }
        }
        public void LevelupRandomPower(){
             //random
             if(lvlup){
                Random rand = new Random();
                
                int [] temp = {};
                int index;
                bool uniqueValue = true;

                for (int i = 0; i < 3;) {
                    index = rand.Next(allPower.Length);
                    //unique value
                    if(temp[0] != index && temp[1] != index){
                        uniqueValue = true;
                    }
                    //store power
                    if(uniqueValue = true){
                        canSelectPower[i] = allPower[index];
                        temp[i] = index;
                        i++;
                        uniqueValue = false;
                    }
                }
                power_random_one = canSelectPower[0];
                power_random_two = canSelectPower[1];
                power_random_three = canSelectPower[2];
             }
        }
        public void SetExpbar(){
            var width = 1000;
            var height = 20;
            ExpBarRectangle = new Texture2D(GetGraphicsDeviceManager().GraphicsDevice, width, height);
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i)
                data[i] = Color.Orange;
            ExpBarRectangle.SetData(data);
            ExpBarRect = new Rectangle(0,0,100,100);
        }
        
        public GraphicsDeviceManager GetGraphicsDeviceManager(){
            return Singleton.Instance.gdm;
        }
        // Center Origin -> to rotatable
        public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

        public void GetMouseInput(){
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
            Singleton.Instance.MouseCurrent = Mouse.GetState();
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
