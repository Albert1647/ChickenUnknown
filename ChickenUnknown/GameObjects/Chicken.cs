using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : _GameObject {
		public float Speed;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsActive;
		public Vector2 Velocity;
		public int gravity = 981;
		private int chickenRadius;
		public Chicken(Texture2D ChickenTexture) : base(ChickenTexture){
			chickenRadius = ChickenTexture.Width / 2;
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				Velocity.X = (float)Math.Cos(Angle) * Speed;
				Velocity.Y = (float)Math.Sin(Angle) * Speed;
				Acceleration.Y += gravity;
				Velocity += Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				pos += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				DetectCollision();
			}
		}
		private void DetectCollision(){
			if(pos.X > 1920 || pos.X < 0 ||  pos.Y < 0|| pos.Y > 1080){
				IsActive = false;
				Singleton.Instance.IsShooting = false;
			}
		}
		public override void Draw(SpriteBatch _spriteBatch) {
			_spriteBatch.Draw(_texture, pos, Color.White);
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, pos, Color.White);
			_spriteBatch.DrawString(font, "Chicken X ? = " + pos.X , new Vector2(0,320), Color.Green);
			_spriteBatch.DrawString(font, "Chicken Y ? = " + pos.Y , new Vector2(0,340), Color.Green);
		}
    }

}