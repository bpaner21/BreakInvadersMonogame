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
    class Paddle : DrawableSprite
    {
        protected GameConsole console;

        public PaddleController controller;
        public Ball Ball;

        protected const float speed = 400f;

        protected Vector2 location;
        protected const float xOrigin = 32f * 12;
        protected const float yOrigin = 32f * 30f;

        
        public Paddle(Game game, Ball ball) : base(game)
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

            this.Ball = ball;

            this.location = new Vector2(xOrigin, yOrigin);

            this.Location = location;

            this.Speed = speed;

            controller = new PaddleController(game, ball);
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("Paddle");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.DrawColor != Color.White)
            {
                this.DrawColor = Color.White;
            }


            if (!Ball.Lose)
            {
                switch(Ball.State)
                {
                    case BallState.Paddle:
                        UpdateMoveBallWithPaddle();
                        break;

                    case BallState.Play:
                        UpdateCheckBallCollision();
                        break;
                }
            }
            else
            {
                UpdateMoveBallWithPaddle();
            }

            controller.HandleInput(gameTime);
            this.Location += controller.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            KeepPaddleOnScreen();

            base.Update(gameTime);
        }

        public virtual void SetInitialLocation()
        {
            this.Location = location;
        }

        protected void UpdateMoveBallWithPaddle()
        {
            Ball.Speed = 0;
            Ball.Direction = Vector2.Zero;
            Ball.Location = new Vector2(this.Location.X, this.Location.Y - Ball.SpriteTexture.Height - 2);
        }

        protected void UpdateCheckBallCollision()
        {
            // TODO bug where ball hits edge of paddle
            // bounces repeatedly off paddle until it hits opposite end

            //Ball Collsion
            if (this.Intersects(Ball))
            {
                this.Ball.Direction.Y *= Ball.GetReflectEntropy(); // functional, but boring
            }

        }

        protected void KeepPaddleOnScreen()
        {
            this.Location.X = MathHelper.Clamp(this.Location.X, (this.spriteTexture.Width / 2), this.Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2));
        }
    }
}
