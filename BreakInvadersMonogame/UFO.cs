using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameLibrary;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace BreakInvadersMonogame
{

    class UFO : DrawableSprite
    {
        protected Texture2D i0, i1, i2, i3, i4, i5, i6, i7, i8, explode;
        protected float clock, countdown, rotationInterval, explosionInterval, countdownExplosion;
        public bool WasHit, Explode;

        public UFO(Game game) : base(game)
        {
            this.Direction = new Vector2(1, 0);
            this.Speed = 200;

            this.WasHit = false;
            this.Explode = false;
            this.rotationInterval = 0.075f;
            this.explosionInterval = 0.4f;
            this.clock = 0f;
            this.countdown = 0f;
            this.countdownExplosion = 0f;
            /*#if DEBUG
                        this.ShowMarkers = true;
            #endif//*/

        }

        protected override void LoadContent()
        {
            this.i0 = this.Game.Content.Load<Texture2D>("UFO 0");
            this.i1 = this.Game.Content.Load<Texture2D>("UFO 1");
            this.i2 = this.Game.Content.Load<Texture2D>("UFO 2");
            this.i3 = this.Game.Content.Load<Texture2D>("UFO 3");
            this.i4 = this.Game.Content.Load<Texture2D>("UFO 4");
            this.i5 = this.Game.Content.Load<Texture2D>("UFO 5");
            this.i6 = this.Game.Content.Load<Texture2D>("UFO 6");
            this.i7 = this.Game.Content.Load<Texture2D>("UFO 7");
            this.i8 = this.Game.Content.Load<Texture2D>("UFO 8");

            this.spriteTexture = i0;

            this.explode = this.Game.Content.Load<Texture2D>("Explode");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.DrawColor != Color.Red)
            {
                this.DrawColor = Color.Red;
            }//*/

            if (WasHit)
            {
                clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (countdownExplosion <= 0f)
                {
                    countdownExplosion = clock + explosionInterval;
                }

                if (clock >= countdownExplosion)
                {
                    Explosion();
                }
            }
            else
            {
                this.Location += this.Direction * this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000f;

                clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (countdown == 0f)
                {
                    countdown = clock + rotationInterval;
                }

                if (clock >= countdown)
                {
                    ChangeSprite();
                    countdown = clock + rotationInterval;
                }
            }

            base.Update(gameTime);
        }

        public virtual void ChangeSprite()
        {
            if (!WasHit)
            {
                if (this.spriteTexture == i0)
                {
                    this.spriteTexture = i1;
                }
                else if (this.spriteTexture == i1)
                {
                    this.spriteTexture = i2;
                }
                else if (this.spriteTexture == i2)
                {
                    this.spriteTexture = i3;
                }
                else if (this.spriteTexture == i3)
                {
                    this.spriteTexture = i4;
                }
                else if (this.spriteTexture == i4)
                {
                    this.spriteTexture = i5;
                }
                else if (this.spriteTexture == i5)
                {
                    this.spriteTexture = i6;
                }
                else if (this.spriteTexture == i6)
                {
                    this.spriteTexture = i7;
                }
                else if (this.spriteTexture == i7)
                {
                    this.spriteTexture = i8;
                }
                else// if (this.spriteTexture == i8)
                {
                    this.spriteTexture = i0;
                }
            }
            else
            {
                this.spriteTexture = explode;
            }
        }

        internal virtual void Hit()
        {
            if (!WasHit)
            {
                this.spriteTexture = explode;
                this.WasHit = true;
            }
        }

        internal protected void Explosion()
        {
            this.Enabled = false;
            this.Visible = false;
            this.Explode = true;
        }
    }
}
