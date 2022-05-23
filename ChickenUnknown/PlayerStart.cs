using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ChickenUnknown.GameObjects;
using System;

namespace ChickenUnknown
{
    class PlayerStart
    {
		// Player on start / on reset when change screen
		public const int Level = 1;
		public const int BarricadeHP = 200;
		public const float PenetrationChance = 1f;
		public const float TreasureChestChance = 2f;
		public const float ChickenAddedHitBox = 0f;
		public const float Scale = 0f;
		public const int StartQuantity = 2;
		public const int Knockback = 0;
		public const float ChickenSpeed = 5f;
		public const float Damage = 8;
		public const bool IsUsingSpecialAbility = false;
		public const float SpecailAbiltyDamage = 50;
		public const float SpecailAbiltyCooldown = 60f;
		public const float SpecailAbiltyMaxCooldown = 60f;
		public const float SpecailAbilityAoE = 400f;
		public const float Exp = 0;
		public const float MaxExp = 150;
		public const int Score = 0;
	}
}
