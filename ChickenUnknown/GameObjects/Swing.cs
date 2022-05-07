using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;


namespace ChickenUnknown.GameObjects {
    	class Swing : _GameObject {
        private Texture2D SwingTexture;
		private Texture2D IndicatorTexture;
		private Chicken chicken; // chicken on swing
        private float aimAngle;
		public Swing(Texture2D swingTexture, Texture2D indicatorTexture, Texture2D chickenTexture) : base(swingTexture){
            SwingTexture = swingTexture;
            IndicatorTexture = indicatorTexture;

		}
		
		public virtual void Update(GameTime gameTime) {
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
			Singleton.Instance.MouseCurrent = Mouse.GetState();
            if (IsShootable()) {
				// calculate gun aim angle
				aimAngle = (float)Math.Atan2((pos.Y + _texture.Height / 2) - Singleton.Instance.MouseCurrent.Y, (pos.X + _texture.Width / 2) - Singleton.Instance.MouseCurrent.X);
				// shooting
				if (!Singleton.Instance.IsShooting && IsClick()) {

				}
			}
		}
		public virtual void Draw(SpriteBatch spriteBatch) {

		}
        public bool IsClick(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released;
        }

		public bool IsShootable(){
			return (Singleton.Instance.MouseCurrent.Y < 625 
			&& Singleton.Instance.MouseCurrent.X > 0 
			&& Singleton.Instance.MouseCurrent.Y > 0 
			&& Singleton.Instance.MouseCurrent.X < Singleton.Instance.Dimension.X
			// && Singleton.Instance.MouseCurrent.Y < Singleton.Instance.Dimension.Y
			);
		}
    }

}