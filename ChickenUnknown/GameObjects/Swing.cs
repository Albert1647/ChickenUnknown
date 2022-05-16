using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;


namespace ChickenUnknown.GameObjects {
    	class Swing : IGameObject {
        private Texture2D StretchAreaTexture, ChickenTexture;
		private Vector2 OldChickenPos; 
		private float OldAimAngle;
        private float AimAngle;
		private Vector2 CENTER_OF_SWING = new Vector2(230, 475); 
		private int MAXSPEED = 1300;
		private int HITBOX;
		public static List<Chicken> ChickenList = new List<Chicken>();
		public static int NumOfChicken;
		public Swing(Texture2D swingTexture, Texture2D chickenTexture, Texture2D stretchAreaTexture) : base(swingTexture){
            // IndicatorTexture = indicatorTexture;
            StretchAreaTexture = stretchAreaTexture;
			ChickenTexture = chickenTexture;
			HITBOX = chickenTexture.Width / 2;
			NumOfChicken = 3;
		}
		
		public override void Update(GameTime gameTime) {
            if ((IsShootable() && IsMouseDown()) || Singleton.Instance.IsAiming) {
				AimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
				if(!Singleton.Instance.IsAiming){
					// play sound here : Start aim
				}
				if (NumOfChicken > 0) {
					Singleton.Instance.IsAiming = true;
					if(IsMouseUp()){
						Singleton.Instance.IsAiming = false;
						var chicken = new Chicken(ChickenTexture) {
							_pos = new Vector2(OldChickenPos.X, OldChickenPos.Y),
							Angle = OldAimAngle + MathHelper.Pi,
							Rotation = OldAimAngle,
							Speed = (int)(MAXSPEED * (GetMouseStretchDistance() >= StretchAreaTexture.Width/2 ? StretchAreaTexture.Width/2 : GetMouseStretchDistance()) / (StretchAreaTexture.Width/2)),
							IsActive = true,
						};
						ChickenList.Add(chicken);
						NumOfChicken -= 1;
					}
				}
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(StretchAreaTexture, CENTER_OF_SWING ,null, Color.White, 0f, GetCenterOrigin(StretchAreaTexture), 1f, SpriteEffects.None, 0);
			if(NumOfChicken > 0){
				if(!Singleton.Instance.IsAiming){
					_spriteBatch.Draw(ChickenTexture, CENTER_OF_SWING ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f, SpriteEffects.None, 0);
				} else if(Singleton.Instance.IsAiming && MouseIsOnStretchAreaTexture()){
					_spriteBatch.Draw(ChickenTexture, new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y) ,null, Color.White, AimAngle, GetCenterOrigin(ChickenTexture), 1f, IsUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
					SaveChickenPosAngle();
				} else if (Singleton.Instance.IsAiming && !MouseIsOnStretchAreaTexture()) {
					_spriteBatch.Draw(ChickenTexture, new Vector2(OldChickenPos.X, OldChickenPos.Y) ,null, Color.White, OldAimAngle, GetCenterOrigin(ChickenTexture), 1f, IsUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
				}
			}
			// Reloaded Chicken
			for(int i = 0; i < NumOfChicken; i++){
				_spriteBatch.Draw(ChickenTexture, new Vector2(130,650+(i * ChickenTexture.Height + 20)) ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f, SpriteEffects.None, 0);
			}

			DrawLog(_spriteBatch,  font);
		}
		public void DrawLog(SpriteBatch _spriteBatch, SpriteFont font){
			_spriteBatch.DrawString(font, "IsAiming = " + Singleton.Instance.IsAiming, new Vector2(0,80), Color.Green);
            _spriteBatch.DrawString(font, "MouseUp = " + IsMouseUp(), new Vector2(0,120), Color.Green);
            _spriteBatch.DrawString(font, "Mouse In Stretch Area = " + MouseIsOnStretchAreaTexture(), new Vector2(0,140), Color.Green);
            _spriteBatch.DrawString(font, "Mouse on Kai ? = " + IsShootable(), new Vector2(0,160), Color.Green);
            _spriteBatch.DrawString(font, "AimAngle ? = " + OldAimAngle + MathHelper.Pi, new Vector2(0,300), Color.Green);
            _spriteBatch.DrawString(font, "Mouse In Chicken Area = " + GetMouseOnChickenDistance(), new Vector2(0,340), Color.Green);
            _spriteBatch.DrawString(font, "Chicken Count = " + ChickenList.Count, new Vector2(0,380), Color.Green);
		}
        public bool IsClick(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released;
        }
		public bool IsMouseDown(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed;
        }
		public bool IsMouseUp(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Released;
        }

		public void SaveChickenPosAngle(){
			OldChickenPos =  new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			OldAimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
		}

		public bool IsUpsideDown(){
			var tempAngle = 0.0;
			if(OldAimAngle < 0){
				tempAngle = (float)OldAimAngle *(-1);
			} else {
				tempAngle = OldAimAngle;
			}
			if(tempAngle > 1.6){
				return true;
			} else {
				return false;
			}
		}

		public bool IsShootable(){
			var chickenRadius = ChickenTexture.Width/2;
			return GetMouseOnChickenDistance() < chickenRadius;
		}
		public void GetMouseInput(){
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
            Singleton.Instance.MouseCurrent = Mouse.GetState();
        }

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

		public bool MouseIsOnStretchAreaTexture(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			var stretchAreaRadius = StretchAreaTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2)) <= stretchAreaRadius;
		}
		
		public int GetMouseOnChickenDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}

		public int GetMouseStretchDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}

    }
}