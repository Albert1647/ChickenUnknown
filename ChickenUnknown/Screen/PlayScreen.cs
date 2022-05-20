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
        public Texture2D ExpBarRectangle, SwingTexture, ChickenTexture, StretchAreaTexture, NormalZombieTexture, TankZombieTexture,
                        bg, bg1, barricade, wall, Popup_levelup, Levelup_item1, Levelup_item2, 
                        Levelup_item3, Select_button, HpBarTexture;
        public Rectangle ExpBarRect;
        private Swing Swing;
        
        public List<Zombie> ZombieQueue = new List<Zombie>();
        public float ZombieSpawnTimer = 0f;
        public float ShowTime = 0f;
        public float SpawnTimer = 0f;
        public TimeSpan TimeSpan;
        public string answerTime;
        public bool lvlUp = false, LevelUp = false, ShowDialog, Random;
        public String SelectPower;
		public int Level = 0, MaxHpWidth = 100, MaxExpWidth = 300;
        public ArrayList RandomPower = new ArrayList();  
        public GameState _gameState;
        public PlayState _playState;
        private bool SelectablePower;
        public static int SpawnLevel = 1;
        public static List<Zombie> ZombieList;
        public enum GameState{
            PLAYING, WINNING, LOSING
        }
        public enum PlayState{
            PLAYING, LEVELUP, GACHA
        }
        public void Initial() {
            _gameState = GameState.PLAYING;
            _playState = PlayState.PLAYING;
            Swing = new Swing(SwingTexture, ChickenTexture, StretchAreaTexture) {
                
            };
            ZombieList = new List<Zombie>();
            AddSpawnQueueZombie(Zombie.ZombieType.TANK, 2);
            AddSpawnQueueZombie(Zombie.ZombieType.NORMAL, 15);
            SpawnLevel += 1;
        }
        public override void LoadContent() {
            // Load Resource
            base.LoadContent();
            Arial = Content.Load<SpriteFont>("Arial");

            SwingTexture = Content.Load<Texture2D>("PlayScreen/draft_slingshot");
            StretchAreaTexture = Content.Load<Texture2D>("PlayScreen/StretchArea");
            ChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken");
            NormalZombieTexture = Content.Load<Texture2D>("PlayScreen/draft_zombie_nm");
            TankZombieTexture = Content.Load<Texture2D>("PlayScreen/draft_zombie_tank");

            bg = Content.Load<Texture2D>("PlayScreen/draft_bg");
            bg1 = Content.Load<Texture2D>("PlayScreen/draft_ingame");
            barricade = Content.Load<Texture2D>("PlayScreen/draft_barricade");
            wall = Content.Load<Texture2D>("PlayScreen/draft_wall");
            Popup_levelup = Content.Load<Texture2D>("PlayScreen/draft_levelup");
            Levelup_item1 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item1");
            Levelup_item2 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item2");
            Levelup_item3 = Content.Load<Texture2D>("PlayScreen/draft_levelup_item3");
            Select_button = Content.Load<Texture2D>("PlayScreen/draft_levelup_select");
            InitializeExpBar();
            InitializeHpBar();
            Initial();
        }
        public override void UnloadContent() {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime) {

            switch(_gameState){
                case GameState.PLAYING:
                    switch(_playState){
                        case PlayState.PLAYING:
                        GetMouseInput();
                        SpawnTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        ZombieSpawnTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        ShowTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                        if(ZombieSpawnTimer >= ZombieSpawnRate()){
                            if(ZombieQueue.Count > 0){
                                ZombieList.Add(ZombieQueue[0]);   
                                ZombieQueue.RemoveAt(0);
                            }
                            ZombieSpawnTimer = 0;
                        }

                        CheckSpawnZombie();
                        Swing.Update(gameTime);
                        for(int i = 0; i < Swing.ChickenList.Count ; i++)
                            Swing.ChickenList[i].Update(gameTime);
                        

                        for(int i = 0; i < ZombieList.Count ; i++)
                            ZombieList[i].Update(gameTime);
                        //LevelupRandomPower
                        UpdateExp();
                        UpdateDisplayTime();
                    
                        if (Keyboard.GetState().IsKeyDown(Keys.T)) 
                            LevelUp = true;
                        if (Keyboard.GetState().IsKeyDown(Keys.U)) 
                            LevelUp = false;
                        break;
                        case PlayState.LEVELUP:
                            GetMouseInput();
                            if(!SelectablePower){
                                //random
                                // if(LevelUp) {
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
                                SelectablePower = true;
                            }
                            if(SelectablePower){
                                //select power
                                if(MouseOnElement(387, 646, 753,833) && IsClick()){
                                    // SelectPower = RandomPower[0].ToString();
                                    // UpdatePower(SelectPower);
                                    SelectPower = RandomPower[0].ToString();
                                    SelectablePower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }else if(MouseOnElement(826, 1092, 758,833) && IsClick()){
                                    SelectPower = RandomPower[1].ToString();
                                    SelectablePower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }else if(MouseOnElement(1274, 1540, 753,834) && IsClick()){
                                    SelectPower = RandomPower[2].ToString();
                                    SelectablePower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }
                            }
                            // if (Singleton.Instance.currentKB.IsKeyUp(Keys.M) && Singleton.Instance.previousKB.IsKeyDown(Keys.M)) {                
                            //     ShowDialog = false;
                            //     LevelUp = false;
                            //     Random = false;                
                            // }        
                        break;
                        case PlayState.GACHA:
                        break;
                    }
                break;
                case GameState.LOSING:

                break;
                case GameState.WINNING:

                break;
            }
            
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            DrawHUD(_spriteBatch);
            DrawLog(_spriteBatch);
            Swing.Draw(_spriteBatch, Arial);
            for(int i = 0; i < Swing.ChickenList.Count ; i++)
				Swing.ChickenList[i].Draw(_spriteBatch, Arial);
            for(int i = 0; i < ZombieList.Count ; i++)
				ZombieList[i].Draw(_spriteBatch, Arial);
        }
        
        public void DrawLog(SpriteBatch _spriteBatch){
            _spriteBatch.DrawString(Arial, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            _spriteBatch.DrawString(Arial, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Click " + IsClick(), new Vector2(0,40), Color.Black);
            _spriteBatch.DrawString(Arial, "Is Dragging " + IsDragging(), new Vector2(0,60), Color.Black);
            _spriteBatch.DrawString(Arial, "Time : " + answerTime, new Vector2(915,50), Color.Black);
            _spriteBatch.DrawString(Arial, "Level : " + Level, new Vector2(0,200), Color.Black);
            _spriteBatch.DrawString(Arial, "Exp : " + Player.Instance.Exp, new Vector2(0,220), Color.Black);
            _spriteBatch.DrawString(Arial, "MaxExpWidth : " + MaxExpWidth, new Vector2(0,240), Color.Black);
            _spriteBatch.DrawString(Arial, "MaxExp : " +Player.Instance.MaxExp , new Vector2(0,260), Color.Black);
            _spriteBatch.DrawString(Arial, "KUY : " +(float)(Player.Instance.Exp/Player.Instance.MaxExp)*MaxExpWidth , new Vector2(0,300), Color.Black);    
            _spriteBatch.DrawString(Arial, "MaxHpWidth : " + MaxHpWidth, new Vector2(300,400), Color.Black);

            _spriteBatch.DrawString(Arial, "ZombieInQueue : " + ZombieQueue.Count, new Vector2(1200,200), Color.Black);
            _spriteBatch.DrawString(Arial, "Time: " + SpawnTimer, new Vector2(1200,220), Color.Black);
            _spriteBatch.DrawString(Arial, "ZombieSpawnTimer: " + ZombieSpawnTimer, new Vector2(1200,240), Color.Black);
            _spriteBatch.DrawString(Arial, "Levelup : " + LevelUp, new Vector2(400,100), Color.Black);
            _spriteBatch.DrawString(Arial, "ShowDialog : " + ShowDialog, new Vector2(400,120), Color.Black);
            _spriteBatch.DrawString(Arial, "Random : " + Random, new Vector2(400,140), Color.Black);
            _spriteBatch.DrawString(Arial, "SelectPower : " + SelectPower, new Vector2(400,200), Color.Black);
            _spriteBatch.DrawString(Arial, "RandomArray : " + RandomPower.Count, new Vector2(400,220), Color.Black);
            // _spriteBatch.DrawString(Arial, "randomPower : " + RandomPower, new Vector2(400,160), Color.Black);
        }
        
        public void DrawHUD(SpriteBatch _spriteBatch){
            // _spriteBatch.Draw(bg, CenterElementWithHeight(bg,0) , Color.White);
            _spriteBatch.Draw(bg1, CenterElementWithHeight(bg1,0) , Color.White);
            _spriteBatch.Draw(wall, new Rectangle(173, UI.FLOOR_Y-378, wall.Width, wall.Height), Color.White);
            _spriteBatch.Draw(barricade, new Rectangle(403, UI.FLOOR_Y-108, barricade.Width, barricade.Height), Color.White);
            _spriteBatch.Draw(SwingTexture, new Rectangle(185, UI.FLOOR_Y-378-216, SwingTexture.Width, SwingTexture.Height), Color.White);
            _spriteBatch.Draw(ExpBarRectangle, new Vector2(100, 100) ,ExpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);        
            switch(_playState){
                case PlayState.LEVELUP:
                    _spriteBatch.Draw(Popup_levelup, new Vector2(288, 108),Color.White);
                    _spriteBatch.Draw(Levelup_item1, new Vector2(344, 372),Color.White);
                    _spriteBatch.Draw(Levelup_item2, new Vector2(792, 372),Color.White);
                    _spriteBatch.Draw(Levelup_item3, new Vector2(1240, 372),Color.White);
                    _spriteBatch.Draw(Select_button, new Vector2(378, 753),Color.White);
                    _spriteBatch.Draw(Select_button, new Vector2(826, 753),Color.White);
                    _spriteBatch.Draw(Select_button, new Vector2(1274, 753),Color.White);
                break;
                case PlayState.GACHA:
                break;
            }
        }

        public void AddSpawnQueueZombie(Zombie.ZombieType type, int amount){
            switch(type){
                case Zombie.ZombieType.NORMAL:
                for(int i = 0 ; i < amount ; i++)
                    ZombieQueue.Add(new Zombie(NormalZombieTexture, HpBarTexture, Zombie.ZombieType.NORMAL){
                        IsActive = true,
                    });
                break;
                case Zombie.ZombieType.TANK:
                    ZombieQueue.Add(new Zombie(TankZombieTexture, HpBarTexture, Zombie.ZombieType.TANK){
                        IsActive = true,
                    });
                break;
                // case Zombie.ZombieType.RUNNER:
                //     ZombieQueue.Add(new Zombie(NormalZombieTexture, HpBarTexture){
                //         IsActive = true
                //     });
                // break;
                default:
                break;
            }
        }

        public void CheckSpawnZombie(){
            // Default Is 180f - 3 minute
            var SpawnInterval = 10f;
            if(SpawnTimer >= SpawnInterval){
                switch(SpawnLevel){
                    case 1:
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,7);
                        // AddSpawnQueueZombie(Zombie.ZombieType.TANK,1);
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,7);
                    break;
                    case 2:
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,7);
                        // AddSpawnQueueZombie(Zombie.ZombieType.TANK,1);
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,7);
                    break;
                    case 3:
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,15);
                    break;
                    case 4:
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,15);
                    break;
                    case 5:
                        AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,25);
                    break;
                    default:
                    break;
                }
                SpawnTimer = 0;
                // MaxSpawnLevel = 5
                if(SpawnLevel < 5){
                    SpawnLevel += 1;
                }
            }

        }

        public float ZombieSpawnRate(){
            switch(SpawnLevel){
                case 1:
                return 5f;  
                case 2:
                return 5f;  
                case 3:
                return 5f;  
                case 4:
                return 5f;  
                case 5:
                return 5f;  
                default:
                return 5f;  
            }
        }
        public void UpdateDisplayTime(){
            TimeSpan = TimeSpan.FromSeconds(ShowTime);
            answerTime = string.Format("{0:D2}:{1:D2}", //for example if you want Millisec => "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms"  ,t.Milliseconds
                TimeSpan.Minutes, 
                TimeSpan.Seconds);
        }
        public void UpdateExp(){
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.O) && Singleton.Instance.previousKB.IsKeyDown(Keys.O))             
                Player.Instance.Exp += 50; 
            ExpBarRect.Width = (int)(((float)(Player.Instance.Exp/Player.Instance.MaxExp))* MaxExpWidth);
            if (ExpBarRect.Width > MaxExpWidth) {
                ExpBarRect.Width = 0;
                Level += 1;
                Player.Instance.Exp = 0;
                _playState = PlayState.LEVELUP;
            }
        }
        public void UpdatePower(String power){
            switch (power) {
                case "scale":
                    Player.Instance.scale += 1;
                    break;
                case "quantity":
                    Player.Instance.quantity += 1;
                    break;
                case "cooldown":
                    Player.Instance.cooldown += 1;
                    break;
                case "damage":
                    Player.Instance.damage += 1;
                    break;
            }
        }
        public void InitializeExpBar(){
            var width = 1000;
            var height = 20;
            ExpBarRectangle = new Texture2D(GetGraphicsDeviceManager().GraphicsDevice, width, height);
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i)
                data[i] = Color.Orange;
            ExpBarRectangle.SetData(data);
            ExpBarRect = new Rectangle(0,0,100,100);
        }
        public void InitializeHpBar(){
			int width = 100;
            int height = 5;
            
            HpBarTexture = new Texture2D(GetGraphicsDeviceManager().GraphicsDevice, width, height);
			
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i)
                data[i] = Color.DarkRed;
            HpBarTexture.SetData(data);
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
