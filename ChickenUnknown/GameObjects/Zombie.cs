using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using ChickenUnknown.Screen;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Input;

namespace ChickenUnknown.GameObjects {
    	public class Zombie : IGameObject {
		public Texture2D HpTexture;
		public Rectangle HpBarRect;
		public SoundEffect ChickenBomb,ChickenSFX,Stretch,Hitting,ZombieBiting,ZombieDie,ZombieSpawn;
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
		public float AnimationTimer = 0f;
		public float AnimationPerFrame = 0.48f;
		public bool SwitchAnimation;
		private int hpYDiff;
		public ZombieType Type;
		List<Texture2D> ZombieTextureList;
		public enum ZombieType{
			NORMAL, TANK, RUNNER
		}
		public Zombie(List<Texture2D> zombieTextureList, Texture2D hpBarTexture, ZombieType type,List<SoundEffect> SFXZombie) : base(zombieTextureList[0]){
			ZombieTextureList = zombieTextureList;
			GroundToOrigin = zombieTextureList[0].Height/2; // use to spawn up to ground
			_pos = new Vector2(1920, UI.FLOOR_Y - GroundToOrigin);
			Type = type;
			HP = GetZombieHp(); // zombie type dependent
			MaxHp = HP;
			ATK = GetZombieATK(); // zombie type dependent
			ExpReward = GetZombieExpReward(); // zombie type dependent
			Speed = GetZombieSpeed(); // zombie type dependent
			AttackCooldown = 5f; // initial 5f to start attack immediately then reset cooldown
			HpTexture = hpBarTexture; 
			HpBarRect = new Rectangle(0, 0, zombieTextureList[0].Width, HpTexture.Height);
			// random hp bar position
			Random rand = new Random();
			hpYDiff = rand.Next(20);
			//Sound
			ZombieBiting = SFXZombie[0];
			ZombieDie = SFXZombie[1];
			ZombieSpawn = SFXZombie[2];
			IsActive = true;
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				CheckIsDead();
				if(_pos.X < 480 && Player.Instance.BarricadeHP > 0){
					IsEating = true; // -Player Barricade HP
				} else {
					_pos.X -= Speed; // Walk to player
					IsEating = false;
				}
			}
			if(IsEating){
				AttackCooldownTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
				if(AttackCooldownTimer > AttackCooldown){
					ZombieBiting.Play();
					Player.Instance.BarricadeHP -= ATK;
					AttackCooldownTimer = 0;
				}
			}
			AnimationTimer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
			if(AnimationTimer > AnimationPerFrame){
				SwitchAnimation = !SwitchAnimation;
				AnimationTimer = 0f;
			}
			UpdateHpBar();
		}
		public void UpdateHpBar(){
			HpBarRect.Width = (int)(((float)(HP / MaxHp)) * _texture.Width );
		}
		public void CheckIsDead() {
			if(HP <= 0){
				ZombieDie.Play();
				Player.Instance.Exp += ExpReward;
				Player.Instance.Score += 100;
				RandomTreasureChest();
				PlayScreen.ZombieList.RemoveAt(PlayScreen.ZombieList.IndexOf(this));
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			if(!SwitchAnimation)
				_spriteBatch.Draw(ZombieTextureList[0], _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
				else
				_spriteBatch.Draw(ZombieTextureList[1], _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			Random rand = new Random();
			_spriteBatch.Draw(HpTexture, new Vector2(_pos.X-_texture.Width / 2, _pos.Y-120-hpYDiff), HpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);  
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
				case ZombieType.RUNNER:
					baseHp = 40;
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
					Speed = 0.7f;
				break;
				case ZombieType.TANK:
					Speed = 0.4f;
				break;
				case ZombieType.RUNNER:
					Speed = 1.2f;
				break;
				default:
				break;
			}
			return Speed;
        }
    }

}