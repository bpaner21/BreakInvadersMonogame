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
    class BunkerManager : DrawableGameComponent
    {
        protected const float xOffset = 24f;
        protected const float yOffset = 8f;
        protected const float yPosition = 25f;

        public List<Bunker> Bunkers;
        protected List<Bunker> destroyedBunkers;

        public BunkerManager(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            LoadBunkers();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Bunker b in Bunkers)
            {
                b.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Bunker b in this.Bunkers)
            {
                b.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void LoadBunkers()
        {
            Bunkers = new List<Bunker>();
            destroyedBunkers = new List<Bunker>();

            Bunker b;
            float yOrigin = yOffset + (32f * yPosition);

            for (int x = 0; x < 4; ++x)
            {
                float xOrigin = xOffset + (32f * (3f + (5f * x)));
                Vector2 origin = new Vector2(xOrigin, yOrigin);
                b = new Bunker(this.Game, origin);
                b.Initialize();
                Bunkers.Add(b);
            }
        }
    }
}
