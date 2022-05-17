using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ChickenUnknown.Screen;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : IGameObject {
		public float Speed;
		public float Rotation;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsActive;
		private  bool IsWalking;
		private Vector2 Velocity;
		private int GRAVITY = 981;
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
				var oldPos = _pos;
				_pos += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				Rotation = (float)Math.Atan2(oldPos.Y - _pos.Y, oldPos.X - _pos.X);
				DetectCollision();
			}
			if(IsWalking){
				_pos.X -= 10;
				if(_pos.X < 480){
					// IsWalking = false;
					Swing.ChickenList.RemoveAt(Swing.ChickenList.IndexOf(this));
					Swing.NumOfChicken += 1;
				}
			}
		}

		private void DetectCollision(){
			DetectZombieCollision();
			if(_pos.X > 1920 || _pos.X < 0 ||  _pos.Y < 0|| _pos.Y > UI.FLOOR_Y){
				IsActive = false;
				Singleton.Instance.IsShooting = false;
				_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius);
				IsWalking = true;
				ResetZombieHit();
			}
		}
		public void ResetZombieHit(){
			for(int i = 0; i < PlayScreen.ZombieList.Count; i++){
				PlayScreen.ZombieList[i].IsHit = false;
			}
		}
		private void DetectZombieCollision(){
			// float xMin,yMin,xMax,yMax;
			Vector2 pos;
			Texture2D texture;
			for(int i = 0; i < PlayScreen.ZombieList.Count; i++){
				if(PlayScreen.ZombieList[i].IsHit){
					continue;
				}
				pos = PlayScreen.ZombieList[i]._pos;
				texture = PlayScreen.ZombieList[i]._texture;
				// xMin = pos.X - texture.Width / 2;
				// yMin = pos.Y - texture.Height / 2;
				// xMax = pos.X + texture.Width / 2;
				// yMax = pos.Y + texture.Height / 2;
				// if(_pos.X > xMin && _pos.Y > yMin && _pos.Y < xMax && _pos.Y < yMax){
				// 	PlayScreen.ZombieList[i]._pos.X += 10;
				// 	PlayScreen.ZombieList[i].IsHit = true;
				// }

				// Three part Collision Check -> Head, Body, Leg (roughly)
				pos.Y -= (float)texture.Height / 2;
				if(IsCollsionZombie(_pos, _texture, pos, texture)){
					HitZombieAtIndex(i);
					continue;
				}
				pos.Y += (float)texture.Height / 2;
				if(IsCollsionZombie(_pos, _texture, pos, texture)){
					HitZombieAtIndex(i);
					continue;
				}
				pos.Y += (float)texture.Height / 2;
				if(IsCollsionZombie(_pos, _texture, pos, texture)){
					HitZombieAtIndex(i);
					continue;
				}
			}
		}
		
		public void HitZombieAtIndex(int index){
			PlayScreen.ZombieList[index]._pos.X += 10;
			PlayScreen.ZombieList[index].IsHit = true;
			PlayScreen.ZombieList[index].HP -= 10;
		}

		public bool IsCollsionZombie(Vector2 chicken, Texture2D chickenTexture, Vector2 zombie,Texture2D zombieTexture){
			var contactDistance = ChickenRadius + zombieTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(chicken.X - zombie.X, 2) + Math.Pow(chicken.Y - zombie.Y, 2)) <= contactDistance;
		}

		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			if(IsWalking){
				_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.FlipHorizontally, 0);
			} else {
				_spriteBatch.DrawString(font, "Chicken X ? = " + _pos.X , new Vector2(0,320), Color.Green);
				_spriteBatch.DrawString(font, "Chicken Y ? = " + _pos.Y , new Vector2(0,340), Color.Green);
				_spriteBatch.DrawString(font, "Angle ? = " + Angle , new Vector2(0,360), Color.Green);
				_spriteBatch.Draw(_texture, _pos ,null, Color.White, Rotation, GetCenterOrigin(_texture), 1f, isUpsideDown() ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			}
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

		public bool isUpsideDown(){
			var tempAngle = 0.0;
			if(Rotation < 0){
				tempAngle = (float)Rotation *(-1);
			} else {
				tempAngle = Rotation;
			}
			if(tempAngle > 1.6){
				return true;
			} else {
				return false;
			}
		}
    }

}