using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ChickenUnknown.GameObjects {
    	class Zombie : IGameObject {
		public Zombie(Texture2D ZombieTexture) : base(ZombieTexture){
			
		}
		
		public virtual void Update(GameTime gameTime) {

		}
		
		public virtual void Draw(SpriteBatch spriteBatch) {

		}
    }

}