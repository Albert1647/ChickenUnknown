using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ChickenUnknown.GameObjects;
using System;

namespace ChickenUnknown
{
    class Player
    {
		public int barricadeHP = 100;
		public int scale = 5;
		public int quantity = 5;
		public int cooldown = 5;
		public int damage = 5;
		public float Exp = 0, MaxExp = 150;
        private static Player instance;
		public static Player Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Player();
				}
				return instance;
			}
		}
	}
}
