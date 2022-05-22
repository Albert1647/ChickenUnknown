using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ChickenUnknown.GameObjects;
using System;

namespace ChickenUnknown
{
    class PlayerUpgrade
    {
		// Player Upgrade Per Level
		public const float Attack = 1.07f;
		public const float PenetrationChance = 7f;
		public const float Luck = 0.08f;
		public const float Scale = 0.1f;
		public const float ChickenSpeed = 0.5f;
		public const int AddQuantity = 1;
		public const int Knockback = 1;
		public const float AddedExpPerLevel = 1.15f;
		public const float SpecailAbiltyDamage = 5;
		public const float SpecailAbiltyCooldown = 0.5f;
		public const float SpecailAbiltyAoE= 5f;
	}
}
