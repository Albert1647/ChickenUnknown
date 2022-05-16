using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Zombie : IGameObject {
		public int Hitpoint;
		public int Hitbox;
		public bool IsActive;
		public Zombie(Texture2D ZombieTexture) : base(ZombieTexture){
			Hitbox = ZombieTexture.Height/2;
			_pos = new Vector2(1920, UI.FLOOR_Y - Hitbox);
		}
		
		public override void Update(GameTime gameTime) {
			if(IsActive){
				_pos.X -= 0.5f;
			}
		}
		
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(_texture, _pos ,null, Color.White, 0f, GetCenterOrigin(_texture), 1f, SpriteEffects.None, 0);
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }
    }

}