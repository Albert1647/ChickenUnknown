using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChickenUnknown.Screen;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Input;

namespace ChickenUnknown.GameObjects {
    	public class Zombie : IGameObject {
		public Texture2D HpTexture;
		public Rectangle HpBarRect;
		public float HP, MaxHp;
		public int ATK;
		public int GroundToOrigin;
		public bool IsActive;
		public bool IsHit;
		public float Speed;
		public bool IsEating;
		public int ExpReward;
		public float AttackCooldownTimer = 5f;
		public float AttackCooldown;
		
		public ZombieType Type;
		public enum ZombieType{
			NORMAL, TANK, RUNNER
		}
		public Zombie(Texture2D zombieTexture, Texture2D hpBarTexture, ZombieType type) : base(zombieTexture){
			GroundToOrigin = zombieTexture.Height/2;
			_pos = new Vector2(1920, UI.FLOOR_Y - GroundToOrigin);
			Type = type;
			HP = GetZombieHp();
			MaxHp = HP;
			ATK = GetZombieATK();
			ExpReward = GetZombieExpReward();
			Speed = GetZombieSpeed();
			AttackCooldown = 5f;
			HpTexture = hpBarTexture;
			HpBarRect = new Rectangle(0, 0, zombieTexture.Width, HpTexture.Height);
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				CheckIsDead();
				if(_pos.X < 480 && Player.Instance.BarricadeHP > 0){
					IsEating = true;
				} else {
					_pos.X -= Speed;
					IsEating = false;
				}
			}
			if(IsEating){
				AttackCooldownTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				if(AttackCooldownTimer > AttackCooldown){
					Player.Instance.BarricadeHP -= ATK;
					AttackCooldownTimer = 0;
				}
			}
			UpdateHp();
		}
		public void UpdateHp(){
	
			if (Singleton.Instance.currentKB.IsKeyUp(Keys.Q) && Singleton.Instance.previousKB.IsKeyDown(Keys.Q)) {                
				HP -= 10; 
			}
			HpBarRect.Width = (int)(((float)(HP / MaxHp)) * _texture.Width );
		}
		public void CheckIsDead() {
			if(HP <= 0){
				Player.Instance.Exp += ExpReward;
				RandomTreasureChest();
				PlayScreen.ZombieList.RemoveAt(PlayScreen.ZombieList.IndexOf(this));
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "HP = " + HP , new Vector2(_pos.X, _pos.Y - _texture.Height / 2), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "ATK = " + ATK , new Vector2(_pos.X, _pos.Y - _texture.Height / 2 + 20), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.Draw(HpTexture, new Vector2(_pos.X-_texture.Width / 2, _pos.Y-120), HpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);  
		}

		public void RandomTreasureChest(){
			Random random = new Random();
			float rand = random.Next(100);
			if(rand < Player.Instance.TreasureChestChance){
				PlayScreen._playState = PlayScreen.PlayState.GACHA;
			}
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }
		public int GetZombieHp(){
			var baseHp = 0;
			var levelHp = 0;
            switch(Type){
				case ZombieType.NORMAL:
					baseHp = 30;
					switch(PlayScreen.SpawnLevel){
						case 1:
						break;
						case 2:
						break;
						case 3:
							levelHp += 10;
						break;
						case 4:
							levelHp += 10;
						break;
						case 5:
							levelHp += 20;
						break;
						default:
						break;
					}
				break;
				case ZombieType.TANK:
					baseHp = 80;
					switch(PlayScreen.SpawnLevel){
						case 1:
						break;
						case 2:
						break;
						case 3:
							levelHp += 20;
						break;
						case 4:
							levelHp += 20;
						break;
						case 5:
							levelHp += 40;
						break;
						default:
						break;
					}
				break;
				default:
					baseHp = 20;
				break;
			}
			return baseHp + levelHp;
        }
		public int GetZombieExpReward(){
			var ExpReward = 0;
            switch(Type){
				case ZombieType.NORMAL:
					ExpReward = 25;
				break;
				case ZombieType.TANK:
					ExpReward = 60;
				break;
				case ZombieType.RUNNER:
					ExpReward = 40;
				break;
				default:
				break;
			}
			return ExpReward;
        }
		public int GetZombieATK(){
			var baseATK = 0;
            switch(Type){
				case ZombieType.NORMAL:
					baseATK = 10;
				break;
				case ZombieType.TANK:
					baseATK = 20;
				break;
				case ZombieType.RUNNER:
					baseATK = 15;
				break;
				default:
				break;
			}
			return baseATK;
        }
		public float GetZombieSpeed(){
			var Speed = 0f;
            switch(Type){
				case ZombieType.NORMAL:
					Speed = 2f;
				break;
				case ZombieType.TANK:
					Speed = 2f;
				break;
				case ZombieType.RUNNER:
					Speed = 2f;
				break;
				default:
				break;
			}
			return Speed;
        }
    }

}