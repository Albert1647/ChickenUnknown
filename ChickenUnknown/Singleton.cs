using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace ChickenUnknown
{
    class Singleton
    {
		// Game Resolution
        public Vector2 Dimension = new Vector2(1920 , 1080);
		public MouseState MousePrevious, MouseCurrent;
		private Random random = new Random();
		public GraphicsDeviceManager gdm;
        private static Singleton instance;
		public static Singleton Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Singleton();
				}
				return instance;
			}
		}
	}
}
