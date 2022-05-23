using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ChickenUnknown.Screen;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : IGameObject {
		public Texture2D ChickenFlyTexture, ExplopsionEffect;
		public List<Texture2D> ChickenWalkTexture;
		public float Force;
		public float FlyingRotation;
		public float Angle;
		public Vector2 Acceleration;
		public  bool IsFlying;
		private  bool IsWalking;
		private Vector2 Velocity;
		private int GRAVITY = 981;
		public float ChickenRadius;
		public bool IsActive;
		public float ExplodeTimer = 0.4f;
		private Vector2 ExplosionPos;
		public SoundEffect ChickenBomb,ChickenSFX,Hitting;
		public bool IsSpecial = false;
		public bool IsExplode = false;
		public float AnimationTimer = 0f;
		public float AnimationPerFrame = 0.48f; // time per frame change
		public int TextureIndex = 0; //to swap texture
		public Chicken(Texture2D chickenTexture,List<Texture2D> chickenWalkTexture, Texture2D chickenFlyTexture, Texture2D explopsionEffect, List<SoundEffect> SFXChicken) : base(chickenTexture){
			ChickenRadius = (chickenTexture.Width) / 2;
			ChickenWalkTexture = chickenWalkTexture;
			ChickenFlyTexture = chickenFlyTexture;
			ExplopsionEffect = explopsionEffect;
			ChickenBomb = SFXChicken[0];
			ChickenSFX = SFXChicken[1];
			Hitting = SFXChicken[2];
		}
		public override void Update(GameTime gameTime) {
			if(IsExplode){
				ExplodeTimer -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				if(ExplodeTimer < 0){
					IsExplode = false;
					ExplodeTimer = 0.4f;
				}
			}
			if(IsFlying){
				// Physics
				Velocity.X = (float)Math.Cos(Angle) * Force;
				Velocity.Y = (float)Math.Sin(Angle) * Force;
				Acceleration.Y += GRAVITY;
				Velocity += Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				var oldPos = _pos; // save pos to calculate arc / draw chicken in circle
				_pos += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				// End Physics
				FlyingRotation = (float)Math.Atan2(oldPos.Y - _pos.Y, oldPos.X - _pos.X);
				DetectCollision();
			}
			if(IsWalking){
				AnimationTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				if(AnimationTimer > AnimationPerFrame){
					if(TextureIndex != ChickenWalkTexture.Count - 1){
						TextureIndex += 1;
					} else {
						TextureIndex = 0;
					}
					AnimationTimer = 0f;
				}
				// ChickenWalk to home
				_pos.X -= Player.Instance.ChickenSpeed;
				if(_pos.X < UI.BARRICADE_X){
					// Remove shooted chicken from screen
					Swing.ChickenList.RemoveAt(Swing.ChickenList.IndexOf(this));
					Swing.NumOfChicken += 1;
				}
			}
			
		}

		private void DetectCollision(){
			DetectZombieCollision();
			if(_pos.X > 1920 || _pos.X < 0 || _pos.Y > UI.FLOOR_Y){
				if(IsSpecial){
					// If Bomb hit groud - render bomb frame on ground
					ChickenBomb.Play();
					IsExplode = true;
					_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
					ExplosionPos = _pos;
					HitZombieWithAoE();
					IsFlying = false;
					IsWalking = true;
				}
				// make chicken walk on ground
				Hitting.Play();
				_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
				IsFlying = false;
				IsWalking = true;
				
				ResetZombieHit();
			}
		}
		// reset zombie hit after being hit - not best solution
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
					if(IsSpecial){
						// render bomb frame
						ExplosionPos = new Vector2(_pos.X,_pos.Y);
						IsExplode = true;
						PlayScreen.ZombieList[i].IsHit = true;
						IsFlying = false;
						_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
						IsWalking = true;
						ChickenBomb.Play();
						HitZombieWithAoE();
					} else {
						Hitting.Play();
						HitZombieAtIndex(i);
					}
					continue;
				}
				pos.Y += (float)texture.Height / 2;
				if(IsCollsionZombie(_pos, _texture, pos, texture)){
					if(IsSpecial){
						// render bomb frame
						ExplosionPos = new Vector2(_pos.X,_pos.Y);
						IsExplode = true;
						PlayScreen.ZombieList[i].IsHit = true;
						IsFlying = false;
						_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
						IsWalking = true;
						HitZombieWithAoE();
					} else {
						HitZombieAtIndex(i);
					}
					continue;
				}
				pos.Y += (float)texture.Height / 2;
				if(IsCollsionZombie(_pos, _texture, pos, texture)){
					if(IsSpecial){
						// render bomb frame
						ExplosionPos = new Vector2(_pos.X,_pos.Y);
						IsExplode = true;
						PlayScreen.ZombieList[i].IsHit = true;
						IsFlying = false;
						_pos = new Vector2(_pos.X, UI.FLOOR_Y - ChickenRadius - Player.Instance.ChickenAddedHitBox);
						IsWalking = true;
						HitZombieWithAoE();
					} else {
						HitZombieAtIndex(i);
					}
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
		// When Zombie Is Hit -> -HP, -POS X if knockback, RandomPen if any
		public void HitZombieAtIndex(int index){
			RandomPenetration(index);
			PlayScreen.ZombieList[index]._pos.X += Player.Instance.Knockback;
			PlayScreen.ZombieList[index].HP -= Player.Instance.Damage;
		}
		// hit logic When using bomb Ability
		public void HitZombieWithAoE(){
			for(int i = 0; i < PlayScreen.ZombieList.Count; i++){
				var pos = PlayScreen.ZombieList[i]._pos;
				var chickenPos = _pos;
				if(IsInAOERadius(chickenPos, pos)){
					PlayScreen.ZombieList[i].HP -= Player.Instance.SpecailAbiltyDamage;
				}
			}
		}
		// Calculate Distance from bomb
		// Optimize by using SquareDistance
		public bool IsInAOERadius(Vector2 chicken, Vector2 zombie){
			return (int)(Math.Pow(chicken.X - zombie.X, 2) + Math.Pow(chicken.Y - zombie.Y, 2)) <= Math.Pow(Player.Instance.SpecailAbilityAoE, 2);
		}

		// Calculate Distance Chicken to (every) zombie
		// Optimize by using SquareDistance
		public bool IsCollsionZombie(Vector2 chicken, Texture2D chickenTexture, Vector2 zombie,Texture2D zombieTexture){
			var contactDistance = ChickenRadius + Player.Instance.ChickenAddedHitBox + zombieTexture.Width/2;
			return (int)(Math.Pow(chicken.X - zombie.X, 2) + Math.Pow(chicken.Y - zombie.Y, 2)) <= Math.Pow(contactDistance, 2);
		}
		
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			if(IsWalking){
				_spriteBatch.Draw(ChickenWalkTexture[TextureIndex], _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f + Player.Instance.Scale, SpriteEffects.FlipHorizontally, 0);
				
			} else {
				_spriteBatch.Draw(ChickenFlyTexture, _pos ,null, Color.White, FlyingRotation + MathHelper.Pi, GetCenterOrigin(_texture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
			}
			if(IsExplode && ExplodeTimer > 0){
				_spriteBatch.Draw(ExplopsionEffect, ExplosionPos ,null, Color.White, 0f, GetCenterOrigin(ExplopsionEffect), 1f , SpriteEffects.None, 0);
			}
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }
    }

}