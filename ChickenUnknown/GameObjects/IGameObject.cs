using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace ChickenUnknown.GameObjects {
		// Class hold basic attribute of gameObject
    	public class IGameObject {
		public Texture2D _texture;
		public Vector2 _pos;
		
		public IGameObject(Texture2D texture) {
			_texture = texture; // initialise gameObject texture
			_pos = Vector2.Zero; // default location
		}
		
		public virtual void Update(GameTime gameTime) {

		}
		public virtual void Draw(SpriteBatch spriteBatch) {
		}
		public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font) {
		}
    }

}