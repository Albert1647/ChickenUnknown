using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : IGameObject {
		public float Speed;
		public float rotation;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsActive;
		public  bool IsWalking;
		public Vector2 Velocity;
		public int GRAVITY = 981;
		private int ChickenRadius;
		public Chicken(Texture2D ChickenTexture) : base(ChickenTexture){
			ChickenRadius = ChickenTexture.Width / 2;
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				Velocity.X = (float)Math.Cos(Angle) * Speed;
				Velocity.Y = (float)Math.Sin(Angle) * Speed;
				Acceleration.Y += GRAVITY;
				Velocity += Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				var oldPos = pos;
				pos += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				rotation = (float)Math.Atan2(oldPos.Y - pos.Y, oldPos.X - pos.X);
				DetectCollision();
			}
			if(IsWalking){
				pos.X -= 10;
				if(pos.X < 480){
					Singleton.Instance.ChickenList.RemoveAt(Singleton.Instance.ChickenList.IndexOf(this));
					Singleton.Instance.NumOfChicken += 1;
				}
			}
		}

		private void DetectCollision(){
			if(pos.X > 1920 || pos.X < 0 ||  pos.Y < 0|| pos.Y > UI.FLOOR_Y){
				IsActive = false;
				Singleton.Instance.IsShooting = false;
				pos = new Vector2(pos.X, UI.FLOOR_Y - ChickenRadius);
				IsWalking = true;
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			if(IsWalking){
				_spriteBatch.Draw(_texture, pos ,null, Color.White, 0f, getCenterOrigin(_texture), 1f, SpriteEffects.FlipHorizontally, 0);
			} else {
				_spriteBatch.DrawString(font, "Chicken X ? = " + pos.X , new Vector2(0,320), Color.Green);
				_spriteBatch.DrawString(font, "Chicken Y ? = " + pos.Y , new Vector2(0,340), Color.Green);
				_spriteBatch.DrawString(font, "Angle ? = " + Angle , new Vector2(0,360), Color.Green);
				_spriteBatch.Draw(_texture, pos ,null, Color.White, rotation, getCenterOrigin(_texture), 1f, isUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			}
		}
		public Vector2 getCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

		public bool isUpsideDown(){
			var tempAngle = 0.0;
			if(rotation < 0){
				tempAngle = (float)rotation *(-1);
			} else {
				tempAngle = rotation;
			}
			if(tempAngle > 1.6){
				return true;
			} else {
				return false;
			}
		}
    }

}