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
        private SpriteFont Pixeloid;
        public Texture2D ExpBarRectangle, SwingTexture, 
                        NormalChickenTexture,SpecialChickenTexture,
                        NormalFlyChickenTexture,SpecialFlyChickenTexture,
                        NormalWalkChickenTexture,SpecialWalkChickenTexture,
                        AmountTexture,LuckTexture,ATKTexture,PenTexture,ScaleTexture,SpeedTexture,
                        ExplosionEffect,AbilityButton,AbilityButtonInactive,
                        StretchAreaTexture, NormalZombieTexture, TankZombieTexture, RunnerZombieTexture,
                        hud_chicken_conter, hud_level_bar, hud_pause,
                        bg, barricade, wall, Popup_levelup, Item_drop_frame,
                        Select_button, HpBarTexture,
                        ChestCloseTexture,ChestOpenTexture,
                        Lost,Win,
                        MainMenuButton,MainMenuHover,RetryButton,Retryhover;
        private bool    MouseOnMainMenuButton, MouseOnRetryButton, HoverMainMenu, HoverRetry;
        public Rectangle ExpBarRect;
        private Swing Swing;
        public List<Zombie> ZombieQueue = new List<Zombie>();
        public Color GrayBlack = new Color(55, 55, 55);
        public float ZombieSpawnTimer = 0f;
        public float ShowTime = 0f;
        public float SpawnTimer = 0f;
        public TimeSpan TimeSpan;
        public string Time;
        public String SelectPower;
		public int MaxHpWidth = 100, MaxExpWidth = 300;
        public ArrayList RandomPower = new ArrayList();  
        public String RandomedGacha;
        public List<String> SelectablePower;
        public List<Texture2D> ItemList;
        private bool CanSelectPower;
        private bool GachaIsRandom;
        private bool ChestIsOpen;
        private float ChestRotateTimer;
        private float ChestMaxRotateTimer = 1.5f;
        private float ChestRotation = 0f;
        private float ChestScale = 0f;
        public GameState _gameState;
        public static int SpawnLevel = 1;
        public static List<Zombie> ZombieList;
        public static PlayState _playState;
        public enum GameState{
            PLAYING, WINNING, LOSING
        }
        public enum PlayState{
            PLAYING, LEVELUP, GACHA
        }
        public void Initial() {
            _gameState = GameState.PLAYING;
            _playState = PlayState.PLAYING;
            Swing = new Swing(SwingTexture,StretchAreaTexture,
                        NormalChickenTexture,SpecialChickenTexture,
                        NormalFlyChickenTexture,SpecialFlyChickenTexture,
                        NormalWalkChickenTexture,SpecialWalkChickenTexture,
                        ExplosionEffect) {
                
            };
            ZombieList = new List<Zombie>();
            AddSpawnQueueZombie(Zombie.ZombieType.TANK,1);
            AddSpawnQueueZombie(Zombie.ZombieType.RUNNER,1);
            AddSpawnQueueZombie(Zombie.ZombieType.NORMAL,1);
            SpawnLevel += 1;
        }
        public override void LoadContent() {
            // Load Resource
            base.LoadContent();
            Pixeloid = Content.Load<SpriteFont>("Pixeloid");

            AbilityButton = Content.Load<Texture2D>("PlayScreen/boomButton_Ready");
            AbilityButtonInactive = Content.Load<Texture2D>("PlayScreen/boomButton_notReady");

            hud_chicken_conter = Content.Load<Texture2D>("PlayScreen/hud_chicken_counter");
            hud_level_bar = Content.Load<Texture2D>("PlayScreen/hud_level_bar");
            hud_pause = Content.Load<Texture2D>("PlayScreen/hud_pause");

            AmountTexture = Content.Load<Texture2D>("PlayScreen/+amount");
            LuckTexture = Content.Load<Texture2D>("PlayScreen/+lck");
            ATKTexture = Content.Load<Texture2D>("PlayScreen/+atk");
            PenTexture = Content.Load<Texture2D>("PlayScreen/+penetation");
            ScaleTexture = Content.Load<Texture2D>("PlayScreen/+scale");
            SpeedTexture = Content.Load<Texture2D>("PlayScreen/+speed");
            ItemList = new List<Texture2D>(){
                AmountTexture,
                LuckTexture,
                ATKTexture,
                PenTexture,
                ScaleTexture,
                SpeedTexture
            };
            SelectablePower = new List<String>()  {
                "Amount","Luck","Damage","Penetration","Scale","Speed"
            };

            SwingTexture = Content.Load<Texture2D>("PlayScreen/slingshot");
            StretchAreaTexture = Content.Load<Texture2D>("PlayScreen/StretchArea");
            NormalZombieTexture = Content.Load<Texture2D>("PlayScreen/zombie_nm");
            RunnerZombieTexture = Content.Load<Texture2D>("PlayScreen/zombie_runner");
            TankZombieTexture = Content.Load<Texture2D>("PlayScreen/zombie_tank");

            NormalChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_on_sling");
            NormalFlyChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_on_air");
            NormalWalkChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_run");
            SpecialChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_Boom_on_sling");
            SpecialFlyChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_Boom_on_air");
            SpecialWalkChickenTexture = Content.Load<Texture2D>("PlayScreen/chicken_Boom_run");
            ExplosionEffect = Content.Load<Texture2D>("PlayScreen/explosion");

            bg = Content.Load<Texture2D>("PlayScreen/bg");
            barricade = Content.Load<Texture2D>("PlayScreen/Barricade");
            wall = Content.Load<Texture2D>("PlayScreen/wall");
            Popup_levelup = Content.Load<Texture2D>("PlayScreen/levelup");

            Item_drop_frame = Content.Load<Texture2D>("PlayScreen/itemdrop_frame");
            ChestCloseTexture = Content.Load<Texture2D>("PlayScreen/treasure_box_close");
            ChestOpenTexture = Content.Load<Texture2D>("PlayScreen/treasure_box_opened");

            Select_button = Content.Load<Texture2D>("PlayScreen/levelup_select");

            //WIN-Lost
            Lost = Content.Load<Texture2D>("PlayScreen/alert_lose");
            Win = Content.Load<Texture2D>("PlayScreen/alert_win");
            MainMenuButton = Content.Load<Texture2D>("PlayScreen/button_main_menu");
            MainMenuHover = Content.Load<Texture2D>("PlayScreen/button_main_menu_hover");
            RetryButton = Content.Load<Texture2D>("PlayScreen/button_retry");
            Retryhover = Content.Load<Texture2D>("PlayScreen/button_retry_hover");

            InitializeExpBar();
            InitializeHpBar();
            Initial();
        }
        public void InitializeExpBar(){
            var width = 300;
            var height = 38;
            ExpBarRectangle = new Texture2D(GetGraphicsDeviceManager().GraphicsDevice, width, height);
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i)
                data[i] = Color.Orange;
            ExpBarRectangle.SetData(data);
            ExpBarRect = new Rectangle(0,0, width, height);
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
        public override void UnloadContent() {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime) {

            GetMouseInput();
            switch(_gameState){
                case GameState.PLAYING:
                    Cheater();
                    switch(_playState){
                        case PlayState.PLAYING:
                            SpawnTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            ZombieSpawnTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            ShowTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            if(Player.Instance.SpecailAbiltyCooldown > 0)
                                Player.Instance.SpecailAbiltyCooldown -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

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
                            CheckLosing();
                            CheckWinning();
                            if(MouseOnElement(70 - AbilityButton.Width / 2, 70+ AbilityButton.Width / 2, 290 - AbilityButton.Height/2,290 + AbilityButton.Height/2) && IsClick() && Player.Instance.SpecailAbiltyCooldown <= 0){
                                Player.Instance.IsUsingSpecialAbility = !Player.Instance.IsUsingSpecialAbility;
                            }
                        break;
                        case PlayState.LEVELUP:
                            
                            if(!CanSelectPower){
                                Random randomPower = new Random();
                                var allPower = new ArrayList()
                                {
                                    "Amount","Luck","Damage","Penetration","Scale","Speed"
                                };
                                var temp = allPower;
                                for ( int i=0 ; i < 3 ; i++) {
                                    int randomIndex = randomPower.Next(allPower.Count);
                                    RandomPower.Add(allPower[randomIndex]);
                                    temp.RemoveAt(randomIndex); 
                                }
                                CanSelectPower = true;
                            }
                            if(CanSelectPower){
                                //select power
                                if(MouseOnElement(387, 646, 753,833) && IsClick()){
                                    SelectPower = RandomPower[0].ToString();
                                    // SelectPower = "damage";
                                    CanSelectPower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }
                                if(MouseOnElement(826, 1092, 758,833) && IsClick()){
                                    SelectPower = RandomPower[1].ToString();
                                    // SelectPower = "penetrationChance";
                                    CanSelectPower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }
                                if(MouseOnElement(1274, 1540, 753,834) && IsClick()){
                                    SelectPower = RandomPower[2].ToString();
                                    // SelectPower = "chickenSpeed";
                                    CanSelectPower = false;
                                    UpdatePower(SelectPower);
                                    _playState = PlayState.PLAYING;
                                    RandomPower.Clear();
                                }
                            }
                        break;

                        case PlayState.GACHA:
                        if(!GachaIsRandom){
                            Random randomPower = new Random();
                            var allPower = new ArrayList()
                            {
                                "Amount","Luck","Damage","Penetration","Scale","Speed"
                            };
                            int randomIndex = randomPower.Next(allPower.Count);
                            RandomedGacha = allPower[randomIndex].ToString();
                            GachaIsRandom = true;
                        }
                        if(GachaIsRandom){
                            //Click Continue / OK
                            if(MouseOnElement(960 - ChestCloseTexture.Width/2,960 + ChestCloseTexture.Width/2 , 540 - ChestCloseTexture.Height/2,540 + ChestCloseTexture.Height/2) && IsClick()){
                                // SelectPower = "damage";
                                if(ChestIsOpen &&  MouseOnElement(960 - ChestCloseTexture.Width/2,960 + ChestCloseTexture.Width/2 , 540 - ChestCloseTexture.Height/2,540 + ChestCloseTexture.Height/2) && IsClick()){
                                // SelectPower = "damage";
                                    UpdatePower(RandomedGacha);
                                    ChestRotation = 0f;
                                    ChestScale = 0f;
                                    ChestRotateTimer = 0f;
                                    GachaIsRandom = false;
                                    ChestIsOpen = false;
                                    _playState = PlayState.PLAYING;
                                }
                                ChestIsOpen = true;
                            }
                        }
                        if(ChestIsOpen){
                            // ChestIsRotate = true;
                            if(ChestRotateTimer < ChestMaxRotateTimer){
                                ChestRotateTimer +=  (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                                ChestRotation = (float)(ChestRotateTimer/ChestMaxRotateTimer) * 36f;
                                ChestScale = (float)(ChestRotateTimer/ChestMaxRotateTimer);
                                // ChestIsRotate = false;
                            } else {
                                ChestRotation = 0f;
                                ChestScale = 1f;
                                // ChestRotateTimer = ChestMaxRotateTimer;
                            }
                        }
                        break;
                    }
                break;
                case GameState.LOSING:
                    //CheckMouseUI
                    //Retry
                    if(MouseOnElement(678, 678+204, 600,600+84)){
                        MouseOnRetryButton = true;
                        if(HoverRetry == false){
                            // HoverMenu.Play();
                            HoverRetry = true;
                        }
                        if(IsClick()){
                            // Click.Play();
                            ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.GameScreen);
                        }
                    } else {
                        MouseOnRetryButton = false;
                        HoverRetry = false;
                    }
                    //Main Menu
                    if(MouseOnElement(926, 926+316, 600,600+84)){
                        MouseOnMainMenuButton = true;
                        if(HoverMainMenu == false){
                            // HoverMenu.Play();
                            HoverMainMenu = true;
                        }
                        if(IsClick()){
                            // Click.Play();
                            ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                        }
                    } else {
                        MouseOnMainMenuButton = false;
                        HoverMainMenu = false;
                    }
                break;
                case GameState.WINNING:
                    //Main Menu
                    if(MouseOnElement(802, 802+316, 600,600+84)){
                        MouseOnMainMenuButton = true;
                        if(HoverMainMenu == false){
                            // HoverMenu.Play();
                            HoverMainMenu = true;
                        }
                        if(IsClick()){
                            // Click.Play();
                            ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                        }
                    } else {
                        MouseOnMainMenuButton = false;
                        HoverMainMenu = false;
                    }
                break;
            }
            
            base.Update(gameTime);
        }
        public void CheckLosing(){
            for(int i = 0; i < ZombieList.Count; i++)
                if(ZombieList[i]._pos.X < UI.FORT_X)
                    _gameState = GameState.LOSING;
        } 
        public void CheckWinning(){
            TimeSpan = TimeSpan.FromSeconds(ShowTime);
            if(TimeSpan.TotalMinutes > 15){
                _gameState = GameState.WINNING;
            }
        } 
        public override void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(bg, CenterElementWithHeight(bg,0) , Color.White);
            _spriteBatch.Draw(SwingTexture, new Rectangle(185, UI.FLOOR_Y-378-216, SwingTexture.Width, SwingTexture.Height), Color.White);
            if(Player.Instance.SpecailAbiltyCooldown < 0 && !Player.Instance.IsUsingSpecialAbility)
                _spriteBatch.Draw(AbilityButton, new Vector2(70, 290) ,null , Color.White, 0f, GetCenterOrigin(AbilityButton), 1f, SpriteEffects.None, 0); 
            else {
                Vector2 FontWidth =  Pixeloid.MeasureString(((int)Player.Instance.SpecailAbiltyCooldown).ToString());
                _spriteBatch.Draw(AbilityButtonInactive, new Vector2(70, 290) ,null , Color.White, 0f, GetCenterOrigin(AbilityButtonInactive), 1f, SpriteEffects.None, 0); 
                if(!Player.Instance.IsUsingSpecialAbility){
                    _spriteBatch.DrawString(Pixeloid, ((int)Player.Instance.SpecailAbiltyCooldown).ToString(), new Vector2(70, 290) , Color.White, 0f,new Vector2(FontWidth.X/2, FontWidth.Y/2), 1f, SpriteEffects.None, 0); 
                }
            }

            Swing.Draw(_spriteBatch, Pixeloid);
            for(int i = 0; i < Swing.ChickenList.Count ; i++)
				Swing.ChickenList[i].Draw(_spriteBatch, Pixeloid);
            if(Player.Instance.BarricadeHP > 0){
                _spriteBatch.Draw(barricade, new Rectangle(403, UI.FLOOR_Y - barricade.Height, barricade.Width, barricade.Height), Color.White);
            }
            for(int i = 0; i < ZombieList.Count ; i++)
				ZombieList[i].Draw(_spriteBatch, Pixeloid);
            _spriteBatch.Draw(wall, new Rectangle(173, UI.FLOOR_Y-378, wall.Width, wall.Height), Color.White);
            DrawLog(_spriteBatch);
            DrawHUD(_spriteBatch);
        }
        
        public void DrawLog(SpriteBatch _spriteBatch){
            _spriteBatch.DrawString(Pixeloid, "Hp : " + Player.Instance.BarricadeHP, new Vector2(UI.BARRICADE_X-100 ,800), GrayBlack);
            // _spriteBatch.DrawString(Pixeloid, "X = " + Singleton.Instance.MouseCurrent.X , new Vector2(0,0), Color.Black);
            // _spriteBatch.DrawString(Pixeloid, "Y = " + Singleton.Instance.MouseCurrent.Y, new Vector2(0, 20), Color.Black);
        }
        
        public void DrawHUD(SpriteBatch _spriteBatch){
            // _spriteBatch.Draw(bg, CenterElementWithHeight(bg,0) , Color.White);
            _spriteBatch.Draw(hud_chicken_conter, new Vector2(58, 95) ,null , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0); 

            _spriteBatch.DrawString(Pixeloid,"Score : " + Player.Instance.Score, new Vector2(58, 32), GrayBlack);
            _spriteBatch.DrawString(Pixeloid, Swing.NumOfChicken.ToString(), new Vector2(206, 113), GrayBlack);
            _spriteBatch.DrawString(Pixeloid,"Lv. : " + Player.Instance.Level, new Vector2(1241, 32), GrayBlack);
            Vector2 timeTextWidth = Pixeloid.MeasureString(""+ Time);
            _spriteBatch.DrawString(Pixeloid, "" + Time, new Vector2(UI.DIMENSION_X/2, 32) , GrayBlack, 0f,new Vector2(timeTextWidth.X/2, 0), 1f, SpriteEffects.None, 0);

            _spriteBatch.Draw(ExpBarRectangle, new Vector2(1484, 32) ,ExpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);        
            _spriteBatch.Draw(hud_level_bar, new Vector2(1484, 32) ,null , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0); 
            switch(_playState){
                case PlayState.LEVELUP:
                    _spriteBatch.Draw(Popup_levelup, new Vector2(288, 108),Color.White);
                    if(CanSelectPower){
                        _spriteBatch.Draw(ItemList[SelectablePower.IndexOf(RandomPower[0].ToString())], new Vector2(344, 372),Color.White);
                        _spriteBatch.Draw(ItemList[SelectablePower.IndexOf(RandomPower[1].ToString())], new Vector2(792, 372),Color.White);
                        _spriteBatch.Draw(ItemList[SelectablePower.IndexOf(RandomPower[2].ToString())], new Vector2(1240, 372),Color.White);
                        _spriteBatch.DrawString(Pixeloid,": " + RandomPower[0],new Vector2(344, 300),GrayBlack);
                        _spriteBatch.DrawString(Pixeloid,": " + RandomPower[1],new Vector2(792, 300),GrayBlack);
                        _spriteBatch.DrawString(Pixeloid,": " + RandomPower[2],new Vector2(1240, 300),GrayBlack);
                    }
                    _spriteBatch.Draw(Select_button, new Vector2(378, 753),Color.White);
                    _spriteBatch.Draw(Select_button, new Vector2(826, 753),Color.White);
                    _spriteBatch.Draw(Select_button, new Vector2(1274, 753),Color.White);
                break;
                case PlayState.GACHA:
                    _spriteBatch.Draw(Item_drop_frame, new Vector2(288, 108),Color.White);
                    if(ChestIsOpen){
                        // Texture2D Item = 
                        _spriteBatch.Draw(ChestOpenTexture, new Vector2(960, 540) ,null , Color.White, 0f, GetCenterOrigin(ChestOpenTexture), 1f, SpriteEffects.None, 0);     
                        _spriteBatch.Draw(ItemList[SelectablePower.IndexOf(RandomedGacha)], new Vector2(960-40, 540-100) ,null , Color.White, ChestRotation, GetCenterOrigin(ItemList[SelectablePower.IndexOf(RandomedGacha)]), ChestScale, SpriteEffects.None, 0);  
                    } else {
                        _spriteBatch.Draw(ChestCloseTexture, new Vector2(960, 540) ,null , Color.White, 0f, GetCenterOrigin(ChestOpenTexture), 1f, SpriteEffects.None, 0);     
                    }
                break;
            }
            switch(_gameState){
                case GameState.WINNING:
                    _spriteBatch.Draw(Win, new Vector2(744,372) , Color.White);
                    //Menu
                    if(MouseOnMainMenuButton) {
                        _spriteBatch.Draw(MainMenuHover, new Vector2(802,600) , Color.White);
                    }
                    else {
                        _spriteBatch.Draw(MainMenuButton, new Vector2(802,600) , Color.White);
                    }
                break;
                case GameState.LOSING:
                    _spriteBatch.Draw(Lost, new Vector2(636,372) , Color.White);
                    //Menu
                    if(MouseOnMainMenuButton) {
                        _spriteBatch.Draw(MainMenuHover, new Vector2(926,600) , Color.White);
                    }
                    else {
                        _spriteBatch.Draw(MainMenuButton, new Vector2(926,600), Color.White);
                    }
                    //Retry
                    if(MouseOnRetryButton) {
                        _spriteBatch.Draw(Retryhover, new Vector2(678,600),Color.White);
                    }
                    else {
                        _spriteBatch.Draw(RetryButton, new Vector2(678,600), Color.White);
                    }
                break;
                default:
                break;
            }
        }
        public void UpdateDisplayTime(){
            TimeSpan = TimeSpan.FromSeconds(ShowTime);
            Time = string.Format("{0:D2}:{1:D2}", //for example if you want Millisec => "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms"  ,t.Milliseconds
                TimeSpan.Minutes, 
                TimeSpan.Seconds);
        }
        public void UpdateExp(){
            ExpBarRect.Width = (int)(((float)(Player.Instance.Exp/Player.Instance.MaxExp))* MaxExpWidth);
            if (ExpBarRect.Width > MaxExpWidth) {
                ExpBarRect.Width = 0;
                Player.Instance.Level += 1;
                Player.Instance.Exp = 0;
                Player.Instance.MaxExp *= PlayerUpgrade.AddedExpPerLevel;
                _playState = PlayState.LEVELUP;
            }
        }
        public GraphicsDeviceManager GetGraphicsDeviceManager(){
            return Singleton.Instance.gdm;
        }
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
        public void Cheater(){
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.O) && Singleton.Instance.previousKB.IsKeyDown(Keys.O))             
                Player.Instance.Exp += 10; 
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.G) && Singleton.Instance.previousKB.IsKeyDown(Keys.G))             
                UpdatePower("scale");
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.Space) && Singleton.Instance.previousKB.IsKeyDown(Keys.Space))
                if(Player.Instance.SpecailAbiltyCooldown < 0)
                    Player.Instance.IsUsingSpecialAbility = !Player.Instance.IsUsingSpecialAbility; 
            if (Singleton.Instance.currentKB.IsKeyUp(Keys.C) && Singleton.Instance.previousKB.IsKeyDown(Keys.C))
                if(_playState == PlayState.GACHA){
                    _playState = PlayState.PLAYING;
                    ChestRotation = 0f;
                    ChestScale = 0f;
                    ChestRotateTimer = 0f;
                }
        }
        public void UpdatePower(String power){
            switch (power) {
                case "Amount":
                    Swing.NumOfChicken += PlayerUpgrade.AddQuantity;
                    break;
                case "Damage":
                    Player.Instance.Damage += PlayerUpgrade.Attack;
                    break;
                case "Luck":
                    if(Player.Instance.TreasureChestChance < 100)
                        Player.Instance.TreasureChestChance += PlayerUpgrade.Luck;
                    break;
                case "Scale":
                    Player.Instance.Scale += PlayerUpgrade.Scale;
                    Player.Instance.ChickenAddedHitBox = (NormalChickenTexture.Width / 2)* Player.Instance.Scale;
                    break;
                case "Penetration":
                    if(Player.Instance.PenetrationChance < 100)
                        Player.Instance.PenetrationChance += PlayerUpgrade.PenetrationChance;
                    break;
                case "Speed":
                    Player.Instance.ChickenSpeed += PlayerUpgrade.ChickenSpeed;
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
                case Zombie.ZombieType.RUNNER:
                    ZombieQueue.Add(new Zombie(RunnerZombieTexture, HpBarTexture, Zombie.ZombieType.RUNNER){
                        IsActive = true
                    });
                break;
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
                    break;
                    case 2:
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
                if(SpawnLevel <= 5){
                    SpawnLevel += 1;
                }
            }

        }

        public float ZombieSpawnRate(){
            switch(SpawnLevel - 1){
                case 1:
                return 2f;  
                case 2:
                return 2f;  
                case 3:
                return 2f;  
                case 4:
                return 2f;  
                case 5:
                return 2f;  
                default:
                return 2f;  
            }
        }
    }
}
