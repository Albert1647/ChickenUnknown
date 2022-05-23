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
		// Player Stat
		public int Level = PlayerStart.Level;
		public int BarricadeHP = PlayerStart.BarricadeHP;
		public float PenetrationChance = PlayerStart.PenetrationChance;
		public float TreasureChestChance = PlayerStart.TreasureChestChance;
		public float ChickenAddedHitBox = PlayerStart.ChickenAddedHitBox;
		public float Scale = PlayerStart.Scale;
		public int StartQuantity = PlayerStart.StartQuantity;
		public int Knockback = PlayerStart.Knockback;
		public float ChickenSpeed = PlayerStart.ChickenSpeed;
		public float Damage = PlayerStart.Damage;
		public bool IsUsingSpecialAbility = PlayerStart.IsUsingSpecialAbility;
		public float SpecailAbiltyDamage = PlayerStart.SpecailAbiltyDamage;
		public float SpecailAbiltyCooldown = PlayerStart.SpecailAbiltyCooldown;
		public float SpecailAbiltyMaxCooldown = PlayerStart.SpecailAbiltyMaxCooldown;
		public float SpecailAbilityAoE = PlayerStart.SpecailAbilityAoE;
		public float Exp = PlayerStart.Exp;
		public float MaxExp = PlayerStart.MaxExp;
		public int Score = PlayerStart.Score;
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
