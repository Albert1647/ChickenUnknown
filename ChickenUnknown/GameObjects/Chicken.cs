using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : IGameObject {
		public float Speed;
		public float rotation;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsActive;
		public Vector2 Velocity;
		public int GRAVITY = 981;
		private int chickenRadius;
		public Chicken(Texture2D ChickenTexture) : base(ChickenTexture){
			chickenRadius = ChickenTexture.Width / 2;
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
		}
		private void DetectCollision(){
			if(pos.X > 1920 || pos.X < 0 ||  pos.Y < 0|| pos.Y > 1080){
				IsActive = false;
				Singleton.Instance.IsShooting = false;
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.DrawString(font, "Chicken X ? = " + pos.X , new Vector2(0,320), Color.Green);
			_spriteBatch.DrawString(font, "Chicken Y ? = " + pos.Y , new Vector2(0,340), Color.Green);
			_spriteBatch.DrawString(font, "Angle ? = " + Angle , new Vector2(0,360), Color.Green);
			_spriteBatch.Draw(_texture, pos ,null, Color.White, rotation, getCenterOrigin(_texture), 1f, isUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
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