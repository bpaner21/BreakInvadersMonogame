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
    enum BallState { Paddle, Play }

    class Ball : DrawableSprite
    {
        public bool Lose;
        protected const float initialSpeed = 360; // for gameplay
        //protected const float initialSpeed = 700; // for testing
        protected Vector2 launchDirection;

        protected const int ceiling = (32 * 2);
        protected const int floor = (32 * 31);

        public BallState State { get; set; }

        protected Random r = new Random();

        protected GameConsole console;
        public Ball(Game game) : base(game)
        {
            this.State = BallState.Paddle; // start with Ball on paddle

            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            if (console == null)
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);
            }

/*#if DEBUG
            this.ShowMarkers = true;
#endif//*/

            Lose = false;

            this.Speed = initialSpeed;
            this.launchDirection = new Vector2(1, -2);

            this.Enabled = true;
            this.Visible = true;

        }


        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("Ball");
            SetInitialLocation();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.DrawColor != Color.White)
            {
                this.DrawColor = Color.White;
            }//*/

            if (!Lose)
            {
                switch (this.State)
                {
                    case BallState.Paddle:
                        // Paddle update
                        // TODO once paddle is implemented
                        break;

                    case BallState.Play:
                        // Playing update
                        UpdateBall(gameTime);
                        break;
                }//*/

                //UpdateBall(gameTime);

                this.Direction.Normalize();
            }
            else
            {
                Loss();
            }

            base.Update(gameTime);
        }


        public void SetInitialLocation()
        {
            this.Location = new Vector2(384, 940);
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = initialSpeed;
            this.Direction = launchDirection;
            this.State = BallState.Play;
            this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        private void UpdateBall(GameTime gameTime)
        {
            this.Location += this.Direction * this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000f;

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X - this.spriteTexture.Width < 0))
            {
                this.Direction.X *= GetReflectEntropy();
            }
            //bottom Miss
            if (this.Location.Y + this.spriteTexture.Height > floor)
            {
                /*this.Direction.Y *= -1;
                 // TODO Lose life
                 console.GameConsoleWrite("Should lose life here!!!");//*/

                //this.Direction.Y *= -1;

                --GameManager.Lives;

                this.resetBall(gameTime);
            }

            //Top
            if (this.Location.Y - this.spriteTexture.Width < ceiling)
            {
                //this.resetBall(gameTime);
                this.Direction.Y *= GetReflectEntropy();
            }
        }

        private void resetBall(GameTime gameTime)
        {
            //--GameManager.Lives;
            this.Speed = 0;
            this.State = BallState.Paddle;
            this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
        }

        public void Reflect(Invader invader)
        {

            if (this.Location.Y < invader.Location.Y)//below
            {
                if (this.Location.X < invader.Location.X) //left
                {
                    console.GameConsoleWrite("Above and Left");
                }
                else // right
                {
                    console.GameConsoleWrite("Above and Right");
                }
            }
            else // above
            {
                if (this.Location.X < invader.Location.X) //left
                {
                    console.GameConsoleWrite("Below and Left");
                }
                else // right
                {
                    console.GameConsoleWrite("Below and Right");
                }
            }

            if (Math.Abs(this.Location.Y - invader.Location.Y) > Math.Abs(this.Location.X - invader.Location.X - ((invader.spriteTexture.Width - invader.spriteTexture.Height) / 2))) // hit the top or bottom
            {
                this.Direction.Y *= GetReflectEntropy();
            }
            else // hit the side
            {
                this.Direction.X *= GetReflectEntropy();
            }

            /*if (this.Origin.Y > invader.Origin.Y + (invader.spriteTexture.Width / 2f) || this.Origin.Y < invader.Origin.Y - (invader.spriteTexture.Width / 2f))
            {
                if (this.Origin.X > invader.Origin.X - (invader.spriteTexture.Width / 2f) && this.Origin.X < invader.Origin.X + (invader.spriteTexture.Width / 2f))
                {
                    this.Direction.X *= -1;
                }
            }
            if (this.Origin.X > invader.Origin.X + (invader.spriteTexture.Width / 2f) || this.Origin.X < invader.Origin.X - (invader.spriteTexture.Width / 2f))
            {
                if ((this.Origin.Y > invader.Origin.Y - (invader.spriteTexture.Width / 2f) && this.Origin.Y < invader.Origin.Y + (invader.spriteTexture.Width / 2f)))
                {
                    this.Direction.Y *= -1;
                }
            }//*/

            //this.Direction.Y *= GetReflectEntropy();
            //console.GameConsoleWrite("Reflected");
        }

        public void Reflect(UFO ufo)
        {

            if (this.Location.Y < ufo.Location.Y)//below
            {
                if (this.Location.X < ufo.Location.X) //left
                {
                    console.GameConsoleWrite("Above and Left");
                }
                else // right
                {
                    console.GameConsoleWrite("Above and Right");
                }
            }
            else // above
            {
                if (this.Location.X < ufo.Location.X) //left
                {
                    console.GameConsoleWrite("Below and Left");
                }
                else // right
                {
                    console.GameConsoleWrite("Below and Right");
                }
            }

            if (Math.Abs(this.Location.Y - ufo.Location.Y) > Math.Abs(this.Location.X - ufo.Location.X - 18f)) // hit the top or bottom
            {
                this.Direction.Y *= GetReflectEntropy();
            }
            else // hit the side
            {
                this.Direction.X *= GetReflectEntropy();
            }

            /*if (this.Origin.Y > invader.Origin.Y + (invader.spriteTexture.Width / 2f) || this.Origin.Y < invader.Origin.Y - (invader.spriteTexture.Width / 2f))
            {
                if (this.Origin.X > invader.Origin.X - (invader.spriteTexture.Width / 2f) && this.Origin.X < invader.Origin.X + (invader.spriteTexture.Width / 2f))
                {
                    this.Direction.X *= -1;
                }
            }
            if (this.Origin.X > invader.Origin.X + (invader.spriteTexture.Width / 2f) || this.Origin.X < invader.Origin.X - (invader.spriteTexture.Width / 2f))
            {
                if ((this.Origin.Y > invader.Origin.Y - (invader.spriteTexture.Width / 2f) && this.Origin.Y < invader.Origin.Y + (invader.spriteTexture.Width / 2f)))
                {
                    this.Direction.Y *= -1;
                }
            }//*/

            //this.Direction.Y *= GetReflectEntropy();
            //console.GameConsoleWrite("Reflected");
        }

        public float GetReflectEntropy()
        {
            return -1;// + ((r.Next(-1, 2) - 1) * 0.1f);
        }

        protected void Loss()
        {
            this.State = BallState.Paddle;
            this.Enabled = false;
            this.Visible = false;
        }
    }
}