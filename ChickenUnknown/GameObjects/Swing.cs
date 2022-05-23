using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;


namespace ChickenUnknown.GameObjects {
    	class Swing : IGameObject {
        private Texture2D StretchAreaTexture,
							NormalChickenTexture,SpecialChickenTexture,
							NormalFlyChickenTexture,SpecialFlyChickenTexture,
							NormalWalkChickenTexture,NormalWalkChickenTexture2,SpecialWalkChickenTexture,
							ExplosionEffect;

		public SoundEffect ChickenSFX,Stretch;
		private Vector2 OldChickenPos;
		private float OldAimAngle;
        private float AimAngle;
		private Vector2 CENTER_OF_SWING = new Vector2(230, 475); 
		private int MAXSPEED = 1300;
		public static List<Chicken> ChickenList = new List<Chicken>();
		public static List<SoundEffect> SFXChicken;
		public static int NumOfChicken;
		public Swing(Texture2D swingTexture,Texture2D stretchAreaTexture,
						List<Texture2D> ChickenTextureList,
						Texture2D explosionEffect,SoundEffect stretch,List<SoundEffect> SFXZombie,List<SoundEffect> SFXchicken) : base(swingTexture)
		{
            StretchAreaTexture = stretchAreaTexture;
			NormalChickenTexture = ChickenTextureList[0];
			NormalFlyChickenTexture = ChickenTextureList[1];
			NormalWalkChickenTexture = ChickenTextureList[2];
			NormalWalkChickenTexture2 = ChickenTextureList[3];
			SpecialChickenTexture = ChickenTextureList[4];
			SpecialFlyChickenTexture = ChickenTextureList[5];
			SpecialWalkChickenTexture = ChickenTextureList[6];
			ExplosionEffect = explosionEffect;
			NumOfChicken = Player.Instance.StartQuantity; // initial Start quantity
			Stretch = stretch;
			SFXChicken=SFXchicken;
			ChickenSFX=SFXChicken[1];
		}
		
		public override void Update(GameTime gameTime) {
            if ((IsShootable() && IsMouseDown()) || Singleton.Instance.IsAiming) {
				// chicken(on mouse) aim to center of swing
				AimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
				if(!Singleton.Instance.IsAiming){
					// play sound here : Start aim
					if (NumOfChicken > 0) 
						Stretch.Play();
				}
				if (NumOfChicken > 0) {
					Singleton.Instance.IsAiming = true;
					if(IsMouseUp()){
						ChickenSFX.Play();
						Singleton.Instance.IsAiming = false;
						Chicken chicken;
						
						if(Player.Instance.IsUsingSpecialAbility){
							List<Texture2D> WalkTexture = new List<Texture2D>(){
								NormalWalkChickenTexture,
								NormalWalkChickenTexture2
							};
							chicken = new Chicken(SpecialChickenTexture, WalkTexture, SpecialFlyChickenTexture, ExplosionEffect, SFXChicken) {
								_pos = new Vector2(OldChickenPos.X, OldChickenPos.Y),
								Angle = OldAimAngle + MathHelper.Pi,
								FlyingRotation = OldAimAngle,
								Speed = (int)(MAXSPEED * (GetMouseStretchDistance() >= StretchAreaTexture.Width/2 ? StretchAreaTexture.Width/2 : GetMouseStretchDistance()) / (StretchAreaTexture.Width/2)),
								IsFlying = true,
								IsActive = true,
								IsSpecial = true
							};
							ChickenList.Add(chicken);
							Player.Instance.IsUsingSpecialAbility = false;
							Player.Instance.SpecailAbiltyCooldown = Player.Instance.SpecailAbiltyMaxCooldown;
						} else {
							List<Texture2D> WalkTexture = new List<Texture2D>(){
								NormalWalkChickenTexture,
								NormalWalkChickenTexture2
							};
							chicken = new Chicken(NormalChickenTexture, WalkTexture, NormalFlyChickenTexture, ExplosionEffect, SFXChicken) {
								_pos = new Vector2(OldChickenPos.X, OldChickenPos.Y),
								Angle = OldAimAngle + MathHelper.Pi,
								FlyingRotation = OldAimAngle,
								Speed = (int)(MAXSPEED * (GetMouseStretchDistance() >= StretchAreaTexture.Width/2 ? StretchAreaTexture.Width/2 : GetMouseStretchDistance()) / (StretchAreaTexture.Width/2)),
								IsFlying = true,
								IsActive = true,
								IsSpecial = false
							};
							ChickenList.Add(chicken);
						}
						// Substract Available Chicken
						NumOfChicken -= 1;
					}
				}
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			// Stretch area not in design
			// _spriteBatch.Draw(StretchAreaTexture, CENTER_OF_SWING ,null, Color.White, 0f, GetCenterOrigin(StretchAreaTexture), 1f, SpriteEffects.None, 0);
			Texture2D ChickenTexture = Player.Instance.IsUsingSpecialAbility ? SpecialChickenTexture : NormalChickenTexture;
			if(NumOfChicken > 0){
				if(!Singleton.Instance.IsAiming){
					_spriteBatch.Draw(ChickenTexture, CENTER_OF_SWING ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
				} else if(Singleton.Instance.IsAiming && MouseIsOnStretchAreaTexture()){
					_spriteBatch.Draw(ChickenTexture, new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y) ,null, Color.White, AimAngle + MathHelper.Pi, GetCenterOrigin(ChickenTexture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
					SaveChickenPosAngle();
				} else if (Singleton.Instance.IsAiming && !MouseIsOnStretchAreaTexture()) {
					_spriteBatch.Draw(ChickenTexture, new Vector2(OldChickenPos.X, OldChickenPos.Y) ,null, Color.White, OldAimAngle + MathHelper.Pi, GetCenterOrigin(ChickenTexture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
				}
			}
			// ChickenQueue
			for(int i = 0; i < NumOfChicken - 1 && i < 5; i++)
				_spriteBatch.Draw(NormalChickenTexture, new Vector2(130,650+(i * ChickenTexture.Height + 20)) ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
			

		}
		private bool IsMouseDown(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed;
        }
		private bool IsMouseUp(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Released;
        }

		private void SaveChickenPosAngle(){
			OldChickenPos =  new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			OldAimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
		}

		private bool IsShootable(){
			var chickenRadius = NormalChickenTexture.Width/2;
			return GetMouseOnChickenDistance() < chickenRadius;
		}

		private Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

		private bool MouseIsOnStretchAreaTexture(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			var stretchAreaRadius = StretchAreaTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2)) <= stretchAreaRadius;
		}
		
		private int GetMouseOnChickenDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}
		
		private int GetMouseStretchDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}

    }
}