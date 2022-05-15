using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;


namespace ChickenUnknown.GameObjects {
    	class Swing : IGameObject {
        private Texture2D SwingTexture, StretchAreaTexture, ChickenTexture;
		private Chicken chicken; 
		private Vector2 OldChickenPos; 
		private float OldAimAngle;
        private float aimAngle;
		private Vector2 CENTER_OF_SWING = new Vector2(165, 475); 
		private int MAXSPEED = 1300;
		private int HITBOX;
		public Swing(Texture2D swingTexture, Texture2D chickenTexture, Texture2D stretchAreaTexture) : base(swingTexture){
            // IndicatorTexture = indicatorTexture;
            StretchAreaTexture = stretchAreaTexture;
			ChickenTexture = chickenTexture;
			HITBOX = chickenTexture.Width / 2;
		}
		
		public override void Update(GameTime gameTime) {
            if ((IsShootable() && IsMouseDown()) || Singleton.Instance.IsAiming) {
				aimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
				if (!Singleton.Instance.IsShooting) {
					Singleton.Instance.IsAiming = true;
					if(IsMouseUp()){
						Singleton.Instance.IsAiming = false;
						chicken = new Chicken(ChickenTexture) {
							pos = new Vector2(OldChickenPos.X, OldChickenPos.Y),
							Angle = OldAimAngle + MathHelper.Pi,
							rotation = OldAimAngle,
							Speed = (int)(MAXSPEED * (GetMouseStretchDistance() >= StretchAreaTexture.Width/2 ? StretchAreaTexture.Width/2 : GetMouseStretchDistance()) / (StretchAreaTexture.Width/2)),
							IsActive = true,
						};
						Singleton.Instance.IsShooting = true;
					}
				}
			}
			if (Singleton.Instance.IsShooting){
				// if shooting update logic in star
				chicken.Update(gameTime);
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(StretchAreaTexture, new Vector2(230, 475) ,null, Color.White, 0f, GetCenterOrigin(StretchAreaTexture), 1f, SpriteEffects.None, 0);
			if(!Singleton.Instance.IsShooting){
				_spriteBatch.Draw(ChickenTexture, new Vector2(230, 475) ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f, SpriteEffects.None, 0);
			} else {
				chicken.Draw(_spriteBatch, font);
			}

			if(Singleton.Instance.IsAiming && MouseIsOnStretchAreaTexture()){
				_spriteBatch.Draw(ChickenTexture, new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y) ,null, Color.White, aimAngle, GetCenterOrigin(ChickenTexture), 1f, IsUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
				SaveChickenPosAngle();
			} else if (Singleton.Instance.IsAiming && !MouseIsOnStretchAreaTexture()) {
				_spriteBatch.Draw(ChickenTexture, new Vector2(OldChickenPos.X, OldChickenPos.Y) ,null, Color.White, OldAimAngle, GetCenterOrigin(ChickenTexture), 1f, IsUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			} 
			// DrawLog(_spriteBatch,  font);
		}
		public void DrawLog(SpriteBatch _spriteBatch, SpriteFont font){
			_spriteBatch.DrawString(font, "IsAiming = " + Singleton.Instance.IsAiming, new Vector2(0,80), Color.Green);
            _spriteBatch.DrawString(font, "MouseUp = " + IsMouseUp(), new Vector2(0,120), Color.Green);
            _spriteBatch.DrawString(font, "Mouse In Stretch Area = " + MouseIsOnStretchAreaTexture(), new Vector2(0,140), Color.Green);
            _spriteBatch.DrawString(font, "Mouse on Kai ? = " + IsShootable(), new Vector2(0,160), Color.Green);
            _spriteBatch.DrawString(font, "AimAngle ? = " + OldAimAngle + MathHelper.Pi, new Vector2(0,300), Color.Green);
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
			var stretchAreaCenterPos = new Vector2(165, 475);
			var stretchAreaRadius = StretchAreaTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(mousePos.X - stretchAreaCenterPos.X, 2) + Math.Pow(mousePos.Y - stretchAreaCenterPos.Y, 2)) <= stretchAreaRadius;
		}
		public int GetMouseOnChickenDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			var chickenCenterPos = new Vector2(165, 475);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - chickenCenterPos.X, 2) + Math.Pow(mousePos.Y - chickenCenterPos.Y, 2));
		}

		public int GetMouseStretchDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			var stretchAreaCenterPos = new Vector2(165, 475);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - stretchAreaCenterPos.X, 2) + Math.Pow(mousePos.Y - stretchAreaCenterPos.Y, 2));
		}
    }
}