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
using System.Collections;

namespace ChickenUnknown.Screen {
    class PlayScreen : IGameScreen {
        private SpriteFont Arial;
        public Texture2D ExpBarRectangle, SwingTexture, ChickenTexture, StretchAreaTexture, ZombieTexture,
                        bg, bg1, barricade, wall, Popup_levelup, Levelup_item1, Levelup_item2, 
                        Levelup_item3, Select_button ;
        public Rectangle ExpBarRect;
        private Swing Swing;
        public static List<Zombie> ZombieList;
        public float Timer = 0f;
        public TimeSpan TimeSpan;
        public string answerTime;
        public bool lvlUp = false, LevelUp = false;
        public string[] canSelectPower = {};
        public String power_random_one , power_random_two , power_random_three,
                        selectPower;
        
        private int MaxWidth = 300, Level = 0;

        public void Initial() {
            Swing = new Swing(SwingTexture, ChickenTexture, StretchAreaTexture) {
                
            };
            ZombieList = new List<Zombie>();
        }
        public override void LoadContent() {
            // Load Resource
            base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");

            SwingTexture = Content.Load<Texture2D>("PlayScreen/draft_slingshot");
            StretchAreaTexture = Content.Load<Texture2D>("PlayScreen/StretchArea");
            ChickenTexture = Content.Load<Texture2D>("PlayScreen/draft_chicken");
            ZombieTexture = Content.Load<Texture2D>("PlayScreen/draft_zombie_nm");

            bg = Content.Load<Texture2D>("PlayScreen/draft_bg");
            bg1 = Content.Load<Texture2D>("PlayScreen/draft_ingame");
            barricade = Content.Load<Texture2D>("PlayScreen/draft_barricade");
            wall = Content.Load<Texture2D>("PlayScreen/draft_wall");
            Popup_levelup = Content.Load<Texture2D>("PlayScreen/draft_levelup");
            Levelup_item1 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item1");
            Levelup_item2 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item2");
            Levelup_item3 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item3");
            Select_button = Content.Load<Texture2D>("PlayScreen/draft_levelup_select");
            SetExpbar();
            Initial();
        }
        public override void UnloadContent() {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            GetMouseInput();

            Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;   

            if(Timer >= ZombieSpawnRate()){
                ZombieList.Add(new Zombie(ZombieTexture){
                    IsActive = true
                });
                Timer = 0;
            }

            for(int i = 0; i < ZombieList.Count ; i++){
				ZombieList[i].Update(gameTime);
			}
            
            Swing.Update(gameTime);
            UpdateExpBar(gameTime);
            UpdateDisplayTime();
            
            //LevelupRandomPower
            LevelupRandomPower();
            UpdateExpBar(gameTime);
            UpdateDisplayTime();
        
            if (Keyboard.GetState().IsKeyDown(Keys.T)) {
                LevelUp = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U)) {
                LevelUp = false;
            }
             
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            DrawGameElement(_spriteBatch);
            DrawLog(_spriteBatch);
            Swing.Draw(_spriteBatch, Arial);
            for(int i = 0; i < ZombieList.Count ; i++){
				ZombieList[i].Draw(_spriteBatch, Arial);
			}
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
            
            // _spriteBatch.Draw(bg, CenterElementWithHeight(bg,0) , Color.White);
            _spriteBatch.Draw(bg1, CenterElementWithHeight(bg1,0) , Color.White);
            _spriteBatch.Draw(wall, new Rectangle(173, UI.FLOOR_Y-378, wall.Width, wall.Height), Color.White);
            _spriteBatch.Draw(barricade, new Rectangle(403, UI.FLOOR_Y-108, barricade.Width, barricade.Height), Color.White);
            _spriteBatch.Draw(SwingTexture, new Rectangle(185, UI.FLOOR_Y-378-216, SwingTexture.Width, SwingTexture.Height), Color.White);
            _spriteBatch.Draw(ExpBarRectangle, new Vector2(100, 100) ,ExpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);        
            
            if (LevelUp) {
                _spriteBatch.Draw(Popup_levelup, new Vector2(288, 108),Color.White);
                _spriteBatch.Draw(Levelup_item1, new Vector2(344, 372),Color.White);
                _spriteBatch.Draw(Levelup_item2, new Vector2(792, 372),Color.White);
                _spriteBatch.Draw(Levelup_item3, new Vector2(1240, 372),Color.White);
                _spriteBatch.Draw(Select_button, new Vector2(378, 753),Color.White);
                _spriteBatch.Draw(Select_button, new Vector2(826, 753),Color.White);
                _spriteBatch.Draw(Select_button, new Vector2(1274, 753),Color.White);
            }
            
        }

        public float ZombieSpawnRate(){
            return 5f;
        }

        public void UpdateDisplayTime(){
            TimeSpan = TimeSpan.FromSeconds(Timer);
            answerTime = string.Format("{0:D2}:{1:D2}", //for example if you want Millisec => "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms"  ,t.Milliseconds
                TimeSpan.Minutes, 
                TimeSpan.Seconds);
        }
        public void UpdateExpBar(GameTime gameTime){
            
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
                    Singleton.Instance.scale += 1;
                    break;
                case "quantity":
                    Singleton.Instance.quantity += 1;
                    break;
                case "cooldown":
                    Singleton.Instance.cooldown += 1;
                    break;
                case "damage":
                    Singleton.Instance.damage += 1;
                    break;
            }
        }
        public void LevelupRandomPower(){
            var RandomPower = new ArrayList();
             //random
            if(lvlUp) {

                var allPower = new ArrayList()
                    {
                        "scale","quantity","cooldown","damage"
                    };

                var temp = allPower;  
                Random randomPower = new Random();
                for ( int i=0 ; i < 3 ; i++) {
                    int thisPower = randomPower.Next(temp.Count);
                    RandomPower.Add(temp[thisPower]);
                    temp.RemoveAt(thisPower); 
                }
            }
            //select power
            if(MouseOnElement(387, 387+253, 758,758+92)){
                selectPower = RandomPower[0].ToString();
                updatePower(selectPower);
            }else if(MouseOnElement(833, 833+253, 758,758+92)){
                selectPower = RandomPower[1].ToString();
                updatePower(selectPower);
            }else if(MouseOnElement(1282, 1282+253, 758,758+92)){
                selectPower = RandomPower[2].ToString();
                updatePower(selectPower);
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
