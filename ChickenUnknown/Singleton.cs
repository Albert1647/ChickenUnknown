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
		public bool IsShooting;
		public bool IsAiming;
		public GraphicsDeviceManager gdm;
		public bool gamePaused = false;
        public KeyboardState currentKB, previousKB;

		//chicken status
		public int scale = 5;
		public int quantity = 5;
		public int cooldown = 5;
		public int damage = 5;
		
		public float Exp = 0, MaxExp = 150;
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
