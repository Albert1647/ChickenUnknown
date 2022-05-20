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
		public int BarricadeHP = 100;
		public float PenetrationChance = 0f;
		public int Scale = 5;
		public int StartQuantity = 5;
		public int Knockback = 10;
		public int ChickenSpeed = 5;
		public int Damage = 5;
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
