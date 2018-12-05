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
    abstract class BunkerCell: DrawableSprite
    {
        protected Texture2D image1, image2;
        protected bool hit;

        protected GameConsole console;
        public BunkerCell(Game game) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            if (console == null)
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);
            }
            /*#if DEBUG
                        this.ShowMarkers = true;
            #endif//*/

            hit = false;
        }

        public virtual void Hit()
        {
            if (!hit)
            {
                hit = true;
                this.spriteTexture = image2;
            }
            else
            {
                this.Enabled = false;
                this.Visible = false;
            }
        }
    }

    class BunkerTile : BunkerCell
    {
        public BunkerTile(Game game) : base(game) { }

        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Bunker 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Bunker 2");
            this.spriteTexture = image1;

            base.LoadContent();
        }
    }

    class BunkerRoofL : BunkerCell
    {
        public BunkerRoofL(Game game) : base(game) { }
        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Bunker Roof Left 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Bunker Roof Left 2");
            this.spriteTexture = image1;
            //this.spriteTexture = image2;

            base.LoadContent();
        }
    }

    class BunkerRoofR : BunkerCell
    {
        public BunkerRoofR(Game game) : base(game) { }
        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Bunker Roof Right 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Bunker Roof Right 2");
            this.spriteTexture = image1;
            //this.spriteTexture = image2;

            base.LoadContent();
        }
    }

    class BunkerBottomL : BunkerCell
    {
        public BunkerBottomL(Game game) : base(game) { }
        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Bunker Bottom Left 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Bunker Bottom Left 2");
            this.spriteTexture = image1;
            //this.spriteTexture = image2;

            base.LoadContent();
        }
    }

    class BunkerBottomR : BunkerCell
    {
        public BunkerBottomR(Game game) : base(game) { }
        protected override void LoadContent()
        {
            this.image1 = this.Game.Content.Load<Texture2D>("Bunker Bottom Right 1");
            this.image2 = this.Game.Content.Load<Texture2D>("Bunker Bottom Right 2");
            this.spriteTexture = image1;
            //this.spriteTexture = image2;

            base.LoadContent();
        }
    }
}
