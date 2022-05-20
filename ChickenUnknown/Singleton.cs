using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ChickenUnknown.GameObjects;
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
		//Exit in Menu
		public bool isExit = false;
		public GraphicsDeviceManager gdm;
        public KeyboardState currentKB, previousKB;
		//
		public GraphicsDeviceManager _graphics;
		//chicken status
		public int MusicVolume = 50,SFXVolume = 50;

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
