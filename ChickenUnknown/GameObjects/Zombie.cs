using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChickenUnknown.Screen;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Input;

namespace ChickenUnknown.GameObjects {
    	class Zombie : IGameObject {
		public Texture2D HpTexture;
		public Rectangle HpBarRect;
		public float HP, MaxHp;
		public int Hitbox;
		public bool IsActive;
		public bool IsHit;
		public float Speed;
		public ZombieType Type;
		public enum ZombieType{
			NORMAL, TANK
		}
		public Zombie(Texture2D ZombieTexture, Texture2D HpBarTexture) : base(ZombieTexture){
			Hitbox = ZombieTexture.Height/2;
			_pos = new Vector2(1920, UI.FLOOR_Y - Hitbox);
			HP = GetZombieHp();
			MaxHp = HP;
			Speed = 0.5f;
			HpTexture = HpBarTexture;
			HpBarRect = new Rectangle(0, 0, ZombieTexture.Width, HpTexture.Height);
		}
		public override void Update(GameTime gameTime) {
			if(IsActive){
				_pos.X -= Speed;
				CheckIsDead();
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
				Singleton.Instance.Exp += 100;
				PlayScreen.ZombieList.RemoveAt(PlayScreen.ZombieList.IndexOf(this));
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "HP = " + HP , new Vector2(_pos.X, _pos.Y - _texture.Height / 2), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.Draw(HpTexture, new Vector2(_pos.X-_texture.Width / 2, _pos.Y-120), HpBarRect , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);  
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }
		public int GetZombieHp(){
            switch(Type){
				case ZombieType.NORMAL:
				return 100;
				case ZombieType.TANK:
				return 200;
				default:
				return 100;
			}
        }
    }

}