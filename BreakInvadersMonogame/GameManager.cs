﻿using System;
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

/*

Summary: This class handles the record keeping for this game, keeping track of lives, points, and levels cleared.
         It also resets the position of the invaders, balls, and paddles each time a life is lost or a level is cleared.
         Because of this
         At the beginning of each level, the invaders should not move until the ball is first launched, and the invaders
         stop moving each time the ball is reset.
         If there is enough time, it will also display a message, with instructions at the beginning of the game, as well
         as a pause function and reset function.

         On its own, this class should display
         1. The number of lives remaining
         2. The score
         
         Conditions for losing a life:
         1. The Ball falls below the bottom edge of the screen
         2. A Laser generated by the InvaderManager collides with the Paddle
         3. An Invader collides with a Bunker, or if the Bunkers are all destroyed, an Invader reaches a specific vertical threshold
*/

namespace BreakInvadersMonogame
{
    class GameManager : DrawableGameComponent
    {
        protected SpriteFont font;
        protected SpriteFont gameOver;

        public static int Lives;
        public static int Score;
        protected int hiScore;

        protected Texture2D life;

        protected Texture2D line;

        protected Vector2 ceilingStart, ceilingEnd, bottomStart, bottomEnd, leftStart, leftEnd, invaderStart, invaderEnd;
        protected const float ceilingY = 2;
        protected const float bottomY = 31;

        protected GameConsole console;

        protected SpriteBatch spriteBatch;

        protected Vector2 hiScoreLocation;
        protected Vector2 scoreLocation;
        protected Vector2 instructionLocation;
        protected Vector2 livesLocation;
        protected Vector2 creditsLocation;
        protected Vector2 gameOverLocation;
        protected Vector2 restartLocation;

        protected bool flash;

        protected float clock; // in milliseconds
        protected float frame;
        protected const float interval = 0.9f;
        
        protected bool lose;

        protected Paddle paddle;
        protected InvaderManager invaderManager;
        protected BunkerManager bunkerManager;
        protected Ball ball;
        protected PaddleController input;

        protected int credits;

        public GameManager (Game game, Paddle paddle, InvaderManager invaderManager, BunkerManager bunkerManager) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            if (console == null)
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);
            }

            this.paddle = paddle;
            this.ball = paddle.Ball;
            this.input = paddle.controller;
            this.invaderManager = invaderManager;
            this.bunkerManager = bunkerManager;
            
            credits = 0;
            hiScore = 10000000;
            //hiScore = 1000; // for testing

            flash = false;
            clock = 0;
            frame = 0;

            livesLocation = new Vector2(6f + (32f * 1f), 6f + (32f * 31f));
            hiScoreLocation = new Vector2(6f + (32f * 18f), 10f);
            scoreLocation = new Vector2(6f + (32f * 1f), 10f);
            creditsLocation = new Vector2(6f + (32f * 18f), 6f + (32f * 31f));
            gameOverLocation = new Vector2(2f + (32f * 6.5f), 2f + (32f * 13f));
            instructionLocation = new Vector2(2f + (32f * 4.78f), 2f + (32f * 13f));
            restartLocation = new Vector2((32f * 8f), 2f + (32f * 15f));

            ceilingStart = new Vector2(0, 32 * ceilingY);
            ceilingEnd = new Vector2(768, 32 * ceilingY);

            bottomStart = new Vector2(0, 32 * bottomY);
            bottomEnd = new Vector2(768, 32 * bottomY);

            leftStart = new Vector2(1, 32 * ceilingY);
            leftEnd = new Vector2(1, 32 * bottomY);

            invaderStart = new Vector2(0, 32f * 25f);
            invaderEnd = new Vector2(768, 32f * 25f);

            SetupNewGame();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Cornerstone");
            gameOver = this.Game.Content.Load<SpriteFont>("GameOver");
            life = this.Game.Content.Load<Texture2D>("Ball");
            line = this.Game.Content.Load<Texture2D>("Dot (use for drawing refrenece lines)");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // for testing purposes
            /*if (input.PressX())
            {
                --Lives;
            }//*/

            if (Score > hiScore)
            {
                hiScore = Score;
            }

            SetFlash(gameTime);
            CheckLives();

            if (!lose)
            {
                UpdateShotCollisionCheck();
                LevelCleared();
            }
            else
            {
                Reset();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            if (this.console.ConsoleState == GameConsole.GameConsoleState.Open)
            {
                for (int y = 1; y < 32; ++y)
                {
                    DrawLine(new Vector2(0, 32 * y), new Vector2(775, 32 * y), Color.Green);
                }

                for (int x = 1; x < 24; ++x)
                {
                    DrawLine(new Vector2(32 * x, 0), new Vector2(32 * x, 1100), Color.Green);
                }

                DrawLine(invaderStart, invaderEnd, Color.White);
            }//*/

            DrawLine(ceilingStart, ceilingEnd, Color.White);
            DrawLine(bottomStart, bottomEnd, Color.White);
            DrawLine(leftStart, leftEnd, Color.White);
            DrawLine(ceilingEnd, bottomEnd, Color.White);

            for (int i = 0; i < Lives; ++i)
            {
                spriteBatch.Draw(life, new Rectangle((102 + (24 * i)), 9 + (32 * 31), life.Width, life.Height), Color.White);
            }

            spriteBatch.DrawString(font, "Lives: ", livesLocation, Color.White);
            spriteBatch.DrawString(font, string.Format("hi score:\n{0:D9}", hiScore), hiScoreLocation, Color.White);
            spriteBatch.DrawString(font, string.Format("Score:\n{0:D7}", Score), scoreLocation, Color.White);
            if (flash)
            {
                spriteBatch.DrawString(font, string.Format("Credit  {0:D2}", credits), creditsLocation, Color.White);

                if(!lose && ball.State == BallState.Paddle)
                {
                    spriteBatch.DrawString(font, "press space or up arrow to launch ball\n left and right arrows to move paddle", instructionLocation, Color.White);
                }

                if (lose)
                {
                    spriteBatch.DrawString(gameOver, "game over", gameOverLocation, Color.White);
                    spriteBatch.DrawString(font, "press enter to restart", restartLocation, Color.White);
                }
            }

            spriteBatch.End();
        }

        protected void SetupNewGame()
        {
            Lives = 3;
            Score = 0;
            this.lose = false;
        }

        protected void Reset()
        {
            if (input.PressEnter())
            {
                SetupNewGame();
                
                this.ball.Visible = false;
                this.ball.Enabled = false;
                this.ball = new Ball(this.Game);
                this.Game.Components.Add(ball);

                this.paddle.Visible = false;
                this.paddle.Enabled = false;
                this.paddle = new Paddle(this.Game, ball);
                this.Game.Components.Add(paddle);
                this.paddle.SetInitialLocation();

                this.invaderManager.Visible = false;
                this.invaderManager.Enabled = false;
                this.invaderManager = new InvaderManager(this.Game, ball);
                this.Game.Components.Add(invaderManager);

                this.bunkerManager.Visible = false;
                this.bunkerManager.Enabled = false;
                this.bunkerManager = new BunkerManager(this.Game);
                this.Game.Components.Add(bunkerManager);
            }
        }

        protected void UpdateShotCollisionCheck()
        {
            foreach (InvaderShot i in invaderManager.InvaderShots)
            {
                foreach (Bunker b in bunkerManager.Bunkers) 
                {
                    foreach (BunkerCell c in b.BunkerCells)
                    {
                        if (i.Intersects(c))
                        {
                            c.Hit();
                            i.Hit();
                        }
                    }
                }

                if (i.Intersects(paddle))
                {
                    foreach (InvaderShot j in invaderManager.InvaderShots)
                    {
                        j.Hit();
                    }
                    --Lives;
                    this.paddle.SetInitialLocation();
                    this.paddle.Ball.State = BallState.Paddle;
                }
            }
        }

        protected void SetFlash(GameTime gameTime)
        {
            clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frame == 0) // used at startup to begin timings
            {
                frame = clock;
            }

            if (clock >= frame) // invader sprites move every 2 seconds
            {
                flash = !flash;

                frame += (float)gameTime.ElapsedGameTime.TotalSeconds + interval;
            }
        }

        private void LevelCleared()
        {
            if (invaderManager.Clear)
            {
                this.invaderManager.Clear = false;
                this.bunkerManager.LoadBunkers();
            }
        }

        protected void CheckLives()
        {
            if (Lives < 0)
            {
                lose = true;
                invaderManager.Lose = true;
                ball.Lose = true;
            }
        }

        protected void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float length = Vector2.Distance(start, end);

            spriteBatch.Draw(line, new Rectangle((int)start.X, (int)start.Y, (int)length, 1), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
