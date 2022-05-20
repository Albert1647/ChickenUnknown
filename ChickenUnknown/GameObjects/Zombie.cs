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
		public int Hitbox;
		public bool IsActive;
		public bool IsHit;
		public float Speed;
		public bool IsEating;
		public ZombieType Type;
		public enum ZombieType{
			NORMAL, TANK, RUNNER
		}
		public Zombie(Texture2D zombieTexture, Texture2D hpBarTexture, ZombieType type) : base(zombieTexture){
			Hitbox = zombieTexture.Height/2;
			_pos = new Vector2(1920, UI.FLOOR_Y - Hitbox);
			Type = type;
			HP = GetZombieHp();
			MaxHp = HP;
			ATK = GetZombieATK();
			Speed = 0.35f;
			HpTexture = hpBarTexture;
			HpBarRect = new Rectangle(0, 0, zombieTexture.Width, HpTexture.Height);
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				CheckIsDead();
				if(_pos.X < 480){
					IsEating = true;
				} else {
					_pos.X -= Speed;
				}
			}
			if(IsEating){
				Player.Instance.barricadeHP -= ATK;
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
				Player.Instance.Exp += 100;
				PlayScreen.ZombieList.RemoveAt(PlayScreen.ZombieList.IndexOf(this));
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "HP = " + HP , new Vector2(_pos.X, _pos.Y - _texture.Height / 2), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "ATK = " + ATK , new Vector2(_pos.X, _pos.Y - _texture.Height / 2 + 20), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.Draw(HpTexture, new Vector2(_pos.X-_texture.Width / 2, _pos.Y-120), HpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);  
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
    }

}