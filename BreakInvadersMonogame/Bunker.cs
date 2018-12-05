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
    class Bunker : DrawableGameComponent
    {
        public List<BunkerCell> BunkerCells;
        protected List<BunkerCell> destroyedBunkerCells;

        //GameConsole console;

        protected float xOrigin, yOrigin;

        public Bunker(Game game, Vector2 origin) : base(game)
        {
            BunkerCells = new List<BunkerCell>();
            destroyedBunkerCells = new List<BunkerCell>();
            xOrigin = origin.X;
            yOrigin = origin.Y;
        }

        public override void Initialize()
        {
            LoadBunker();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCollision();

            foreach (BunkerCell b in BunkerCells)
            {
                b.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(BunkerCell b in BunkerCells)
            {
                b.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        protected void LoadBunker()
        {
            BunkerCell b;
            for (int x = 0; x < 4; ++x)
            {
                for (int y = 0; y < 4; ++y)
                {
                    if (y == 0 && x == 0)
                    {
                        b = new BunkerRoofL(this.Game);
                    }
                    else if (y == 0 && x == 3)
                    {
                        b = new BunkerRoofR(this.Game);
                    }
                    else if (y == 3 && x == 1)
                    {
                        b = new BunkerBottomL(this.Game);
                    }
                    else if (y == 3 && x == 2)
                    {
                        b = new BunkerBottomR(this.Game);
                    }
                    else
                    {
                        b = new BunkerTile(this.Game);
                    }

                    b.Initialize();
                    b.Location = new Vector2(xOrigin + (x * 16), yOrigin + (y * 16));
                    BunkerCells.Add(b);
                }
            }
        }

        protected void UpdateCollision()
        {
            foreach (BunkerCell b in BunkerCells)
            {
                if (!b.Enabled)
                {
                    destroyedBunkerCells.Add(b);
                }
            }

            foreach (BunkerCell c in destroyedBunkerCells)
            {
                BunkerCells.Remove(c);
            }

            destroyedBunkerCells.Clear();
        }
    }
}
