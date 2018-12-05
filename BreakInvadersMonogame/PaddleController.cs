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
    class PaddleController
    {
        protected InputHandler input;
        protected Ball ball;
        public Vector2 Direction;

        public PaddleController(Game game, Ball ball)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
            this.ball = ball;   //need refernce to ball to be able to lanch ball could possibly use delegate here
        }

        public virtual void HandleInput(GameTime gametime)
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new upafet

            //No need to sum input only uses left and right
            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                this.Direction = new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                this.Direction = new Vector2(1, 0);
            }
            //TODO add mouse controll?

            //Up launches ball
            if (input.KeyboardState.WasKeyPressed(Keys.Up) || input.KeyboardState.WasKeyPressed(Keys.Space))
            {
                if (ball.State == BallState.Paddle) //Only Launch Ball is it's on paddle
                    this.ball.LaunchBall(gametime);
            }
        }

        public virtual bool PressX()
        {
            return input.KeyboardState.WasKeyPressed(Keys.X);
        }

        public virtual bool PressEnter()
        {
            return input.KeyboardState.WasKeyPressed(Keys.Enter);
        }
    }
}
