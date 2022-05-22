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
        public Vector2 Dimension = new Vector2(1920 , 1080); // Game Resolution
		public MouseState MousePrevious, MouseCurrent;
		public bool IsAiming;
		public bool isExit = false; //Exit in Menu
		public GraphicsDeviceManager gdm;
        public KeyboardState currentKB, previousKB;
		public GraphicsDeviceManager _graphics;
		//Volume
		public float MusicVolume = 50,SFXVolume = 50;
		public bool BGMStart=false;
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
