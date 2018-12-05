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
    class SmallInvader : Invader
    {
        public SmallInvader(Game game) : base(game)
        {

        }
        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Small Invader 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Small Invader 2");
            this.explode = this.Game.Content.Load<Texture2D>("Explode");
            this.SpriteTexture = image1;
            base.LoadContent();
        }
    }
}
