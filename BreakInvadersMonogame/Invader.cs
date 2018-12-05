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

    class Invader : DrawableSprite
    {
        protected Texture2D image1, image2, explode;
        public int Row, Column;
        protected float clock, countdown, interval;
        public bool WasHit, Explode;

        public Invader(Game game) : base(game)
        {
            this.WasHit = false;
            this.Explode = false;
            this.interval = 0.4f;
            this.clock = 0f;
            this.countdown = 0f;
/*#if DEBUG
            this.ShowMarkers = true;
#endif//*/

        }

        protected override void LoadContent()
        {
            this.explode = this.Game.Content.Load<Texture2D>("Explode");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.DrawColor != Color.LimeGreen)
            {
                this.DrawColor = Color.LimeGreen;
            }//*/

            if (WasHit)
            {
                clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (countdown == 0f)
                {
                    countdown = clock + interval;
                }

                if (clock >= countdown)
                {
                    Explosion();
                }
            }

            base.Update(gameTime);
        }

        public virtual void ChangeSprite()
        {
            if (!WasHit)
            {
                if (this.spriteTexture == image1)
                {
                    this.spriteTexture = image2;
                }
                else // if (this.SpriteTexture == image2)
                {
                    this.spriteTexture = image1;
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
                this.Enabled = false;
            }
        }

        internal protected void Explosion()
        {
            this.Visible = false;
            this.Explode = true;
        }
    }
}
