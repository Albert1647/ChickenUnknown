using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChickenUnknown.Screen;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace ChickenUnknown.GameObjects {
    	class Zombie : IGameObject {
		public int HitPoint;
		public int Hitbox;
		public bool IsActive;
		public bool IsHit;
		public float Speed;
		public ZombieType Type;
		public enum ZombieType{
			NORMAL, TANK
		}
		public Zombie(Texture2D ZombieTexture) : base(ZombieTexture){
			Hitbox = ZombieTexture.Height/2;
			_pos = new Vector2(1920, UI.FLOOR_Y - Hitbox);
			HitPoint = GetZombieHp();
			Speed = 0.5f;
		}
		
		public override void Update(GameTime gameTime) {
			if(IsActive){
				_pos.X -= Speed;
				CheckIsDead();
			}
		}
		public void Knockback(GameTime gameTime) {
			_pos.X -= 10;
		}
		public void CheckIsDead() {
			if(HitPoint <= 0){
				PlayScreen.ZombieList.RemoveAt(PlayScreen.ZombieList.IndexOf(this));
			}
		}
		
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
			_spriteBatch.DrawString(font, "HP = " + HitPoint , new Vector2(_pos.X, _pos.Y -_texture.Height / 2), Color.DarkRed, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
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