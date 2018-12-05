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
    class InvaderShot : DrawableSprite
    {
        protected Texture2D image1, image2;
        protected const float speed = 500f;
        protected const int screenBottom = 1024;

        protected float clock; // in milliseconds
        protected float frame;
        protected const float interval = 0.03f;

        public InvaderShot(Game game, Vector2 origin) : base(game)
        {
            this.Location = origin;
            this.Speed = speed;
            this.Direction = new Vector2(0, 1);

            clock = 0;
            frame = 0;
        }

        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Invader Shot");
            this.image2 = this.Game.Content.Load<Texture2D>("Invader Shot 2");
            this.spriteTexture = image1;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.DrawColor != Color.White)
            {
                this.DrawColor = Color.White;
            }//*/

            FlipSprite(gameTime);

            this.Location += this.Direction * this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (this.Location.Y > screenBottom)
            {
                Hit();
            }

            base.Update(gameTime);
        }

        public void Hit()
        {
            this.Enabled = false;
            this.Visible = false;
        }
        protected void FlipSprite(GameTime gameTime)
        {
            clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frame == 0) // used at startup to begin timings
            {
                frame = clock;
            }

            if (clock >= frame) // invader sprites move every 2 seconds
            {
                if (this.spriteTexture == image1)
                {
                    this.spriteTexture = image2;
                }
                else
                {
                    this.spriteTexture = image1;
                }

                frame += (float)gameTime.ElapsedGameTime.TotalSeconds + interval;
            }
        }
    }
}
