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
		public int Level;
		public int BarricadeHP = 200;
		public float PenetrationChance = 10f;
		public float TreasureChestChance = 2f;
		public float ChickenAddedHitBox = 0f;
		public float Scale = 0f;
		public int StartQuantity = 3;
		public int Knockback = 0;
		public float ChickenSpeed = 5f;
		public int Damage = 10;
		public bool IsUsingSpecialAbility = false;
		public float SpecailAbiltyDamage = 100;
		public float SpecailAbiltyCooldown = 5f;
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
