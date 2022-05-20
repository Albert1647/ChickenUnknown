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
		public float FlyingRotation;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsFlying;
		private  bool IsWalking;
		private Vector2 Velocity;
		private int GRAVITY = 981;
		public float ChickenRadius;
		public bool IsActive;
		public Chicken(Texture2D ChickenTexture) : base(ChickenTexture){
			ChickenRadius = (ChickenTexture.Width) / 2;
		}
		public override void Update(GameTime gameTime) {
			if(IsFlying){
				Velocity.X = (float)Math.Cos(Angle) * Speed;
				Velocity.Y = (float)Math.Sin(Angle) * Speed;
				Acceleration.Y += GRAVITY;
				Velocity += Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				var oldPos = _pos;
				_pos += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				FlyingRotation = (float)Math.Atan2(oldPos.Y - _pos.Y, oldPos.X - _pos.X);
				DetectCollision();
			}
			if(IsWalking){
				_pos.X -= Player.Instance.ChickenSpeed;
				if(_pos.X < 480){
					// IsWalking = false;
					Swing.ChickenList.RemoveAt(Swing.ChickenList.IndexOf(this));
					Swing.NumOfChicken += 1;
				}
			}
		}

		private void DetectCollision(){
			DetectZombieCollision();
			if(_pos.X > 1920 || _pos.X < 0 || _pos.Y > UI.FLOOR_Y){
				IsFlying = false;
				_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
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

		public void RandomPenetration(int index){
			Random random = new Random();
			float rand = random.Next(100);
			if(rand < Player.Instance.PenetrationChance){
				// Penetration
				PlayScreen.ZombieList[index].IsHit = true;
			} else {
				// No Penetration
				PlayScreen.ZombieList[index].IsHit = true;
				// Start Walking Rightaway
				IsFlying = false;
				_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
				IsWalking = true;
				// let next chicken hit by reset all zombie isHit;
				ResetZombieHit();
			}
		}
		public void HitZombieAtIndex(int index){
			RandomPenetration(index);
			PlayScreen.ZombieList[index]._pos.X += Player.Instance.Knockback;
			PlayScreen.ZombieList[index].HP -= Player.Instance.Damage;
		}

		public bool IsCollsionZombie(Vector2 chicken, Texture2D chickenTexture, Vector2 zombie,Texture2D zombieTexture){
			var contactDistance = ChickenRadius + Player.Instance.ChickenAddedHitBox + zombieTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(chicken.X - zombie.X, 2) + Math.Pow(chicken.Y - zombie.Y, 2)) <= contactDistance;
		}

		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			if(IsWalking){
				_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f + Player.Instance.Scale, SpriteEffects.FlipHorizontally, 0);
				
			} else {
				_spriteBatch.DrawString(font, "Radius ? = " + Player.Instance.ChickenAddedHitBox , new Vector2(600,360), Color.Green);
				_spriteBatch.Draw(_texture, _pos ,null, Color.White, FlyingRotation + MathHelper.Pi, GetCenterOrigin(_texture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
			}
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }
    }

}