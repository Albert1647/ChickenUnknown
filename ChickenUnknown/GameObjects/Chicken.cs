using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Chicken : _GameObject {
		public Chicken(Texture2D ChickenTexture, Texture2D IndicatorTexture) : base(ChickenTexture){
			
		}
		
		public virtual void Update(GameTime gameTime) {

		}
		public virtual void Draw(SpriteBatch spriteBatch) {

		}
    }

}