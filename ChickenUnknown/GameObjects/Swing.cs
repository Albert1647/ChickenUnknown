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
							NormalWalkChickenTexture,SpecialWalkChickenTexture,
							ExplosionEffect;
		private Vector2 OldChickenPos;
		private float OldAimAngle;
        private float AimAngle;
		private Vector2 CENTER_OF_SWING = new Vector2(230, 475); 
		private int MAXSPEED = 1300;
		private int HITBOX;
		public static List<Chicken> ChickenList = new List<Chicken>();
		public static int NumOfChicken;
		public Swing(Texture2D swingTexture,Texture2D stretchAreaTexture,
						Texture2D normalChickenTexture,Texture2D specialChickenTexture,
					  	Texture2D normalFlyChickenTexture,Texture2D specialFlyChickenTexture,
						Texture2D  normalWalkChickenTexture,Texture2D specialWalkChickenTexture,
						Texture2D explosionEffect) : base(swingTexture)
		{
            StretchAreaTexture = stretchAreaTexture;
			NormalChickenTexture = normalChickenTexture;
			NormalFlyChickenTexture = normalFlyChickenTexture;
			NormalWalkChickenTexture = normalWalkChickenTexture;
			SpecialWalkChickenTexture = specialWalkChickenTexture;
			SpecialChickenTexture = specialChickenTexture;
			SpecialFlyChickenTexture = specialFlyChickenTexture;
			ExplosionEffect = explosionEffect;
			HITBOX = normalChickenTexture.Width / 2;
			NumOfChicken = Player.Instance.StartQuantity;
		}
		
		public override void Update(GameTime gameTime) {
            if ((IsShootable() && IsMouseDown()) || Singleton.Instance.IsAiming) {
				AimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
				if(!Singleton.Instance.IsAiming){
					// play sound here : Start aim
				}
				if (NumOfChicken > 0) {
					Singleton.Instance.IsAiming = true;
					if(IsMouseUp()){
						Singleton.Instance.IsAiming = false;
						Chicken chicken;
						if(Player.Instance.IsUsingSpecialAbility){
							chicken = new Chicken(SpecialChickenTexture, SpecialWalkChickenTexture, SpecialFlyChickenTexture, ExplosionEffect) {
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
							Player.Instance.SpecailAbiltyCooldown = 5f;
						} else {
							chicken = new Chicken(NormalChickenTexture, NormalWalkChickenTexture, NormalFlyChickenTexture, ExplosionEffect) {
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
						NumOfChicken -= 1;
					}
				}
			}
		}
		public override void Draw(SpriteBatch _spriteBatch, SpriteFont font) {
			_spriteBatch.Draw(StretchAreaTexture, CENTER_OF_SWING ,null, Color.White, 0f, GetCenterOrigin(StretchAreaTexture), 1f, SpriteEffects.None, 0);
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
			// Reloaded Chicken
			for(int i = 0; i < NumOfChicken && i < 5; i++){
				_spriteBatch.Draw(NormalChickenTexture, new Vector2(130,650+(i * ChickenTexture.Height + 20)) ,null, Color.White, 0f, GetCenterOrigin(ChickenTexture), 1f + Player.Instance.Scale, SpriteEffects.None, 0);
			}

		}
		public bool IsMouseDown(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed;
        }
		public bool IsMouseUp(){
            return Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Released;
        }

		public void SaveChickenPosAngle(){
			OldChickenPos =  new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			OldAimAngle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - CENTER_OF_SWING.Y, Singleton.Instance.MouseCurrent.X - CENTER_OF_SWING.X);
		}

		public bool IsShootable(){
			var chickenRadius = NormalChickenTexture.Width/2;
			return GetMouseOnChickenDistance() < chickenRadius;
		}

		public Vector2 GetCenterOrigin(Texture2D texture){
            return new Vector2(texture.Width/2, texture.Height/2);
        }

		public bool MouseIsOnStretchAreaTexture(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			var stretchAreaRadius = StretchAreaTexture.Width/2;
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2)) <= stretchAreaRadius;
		}
		
		public int GetMouseOnChickenDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}

		public int GetMouseStretchDistance(){
			var mousePos = new Vector2(Singleton.Instance.MouseCurrent.X, Singleton.Instance.MouseCurrent.Y);
			return (int)Math.Sqrt(Math.Pow(mousePos.X - CENTER_OF_SWING.X, 2) + Math.Pow(mousePos.Y - CENTER_OF_SWING.Y, 2));
		}

    }
}