using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ChickenUnknown.Managers;

namespace ChickenUnknown.Screen {
	class _GameScreen {

		protected ContentManager Content;

		public virtual void LoadContent() {
			Content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider,"Content");
		}
		public virtual void UnloadContent() {
			Content.Unload();
		}
		public virtual void Update(GameTime gameTime) {

		}
		public virtual void Draw(SpriteBatch spriteBatch) {

		}
	}
}
