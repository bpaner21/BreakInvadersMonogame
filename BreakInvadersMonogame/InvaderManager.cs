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
    class InvaderManager : DrawableGameComponent
    {
        public List<Invader> Invaders { get; private set; }
        public List<InvaderShot> InvaderShots;
        public List<UFO> ufo;

        protected List<Invader> destroyedInvaders;
        protected List<InvaderShot> removedShots;
        protected List<UFO> destroyedUFO;


        protected const int hardCodedLeftMargin = 16 + (32 * 1);
        protected const int hardCodedTopMargin = 16 + (32 * 3) + 8;

        protected const int xBuffer = 56;
        protected const int yBuffer = 8;

        protected float clock; // in milliseconds
        protected float frame;
        protected float interval;

        // use these variables for actual gameplay
        protected const float initialIntervval = 1.5f;
        protected const float levelDecrement = 0.05f;
        protected const float intervalDecrement = 0.15f;
        protected const float minimumInterval = 0.15f;
        //*/

        /*// use these variables for testing code
        protected const float initialIntervval = 0.5f;
        protected const float intervalDecrement = 0.1f;
        protected const float minimumInterval = 0.1f;
        //*/

        public int Level;

        protected float direction;

        protected int row;

        protected bool down;

        protected int moveDown;

        public Ball Ball;

        public bool Reflected;

        public bool Lose;

        public bool Clear;

        protected const int maxShots = 5;

        protected const int shotChance = 5;

        protected const float invaderThreshold = 32f * 25f;

        protected Random r = new Random();

        protected GameConsole console;

        public InvaderManager(Game game, Ball ball) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            if (console == null)
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);
            }

            this.Invaders = new List<Invader>();
            this.destroyedInvaders = new List<Invader>();

            this.InvaderShots = new List<InvaderShot>();
            this.removedShots = new List<InvaderShot>();

            this.ufo = new List<UFO>();
            this.destroyedUFO = new List<UFO>();

            clock = 0;
            frame = 0;

            this.Ball = ball;
            Lose = false;
            Reflected = false;
            Clear = false;
            this.Level = 0;
        }

        public override void Initialize()
        {
            LoadLevel();

            /*UFO u = new UFO(this.Game);
            u.Location = new Vector2((32f * -1f), 16f + (32f * 3f));
            u.Initialize();
            ufo.Add(u);// for testing purposes */ 

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            CheckLoss();

            this.Reflected = false;

            if (!Lose)
            {
                if (Ball.State == BallState.Play)
                {
                    clock += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (frame == 0) // used at startup to begin timings
                    {
                        frame = clock;
                    }

                    if (clock >= frame) // invader sprites move every 2 seconds
                    {
                        Shoot();
                        UpdateMovement();

                        frame += (float)gameTime.ElapsedGameTime.TotalSeconds + interval;
                    }
                }
            }

            foreach(Invader i in Invaders)
            {
                i.Update(gameTime);
            }

            foreach (InvaderShot j in InvaderShots)
            {
                j.Update(gameTime);
            }

            foreach (UFO k in ufo)
            {
                k.Update(gameTime);
            }

            UpdateCollision(gameTime);
            UpdateRemove();

            if (!Lose)
            {
                if (Invaders.Count == 0)
                {
                    Ball.Location = new Vector2(384, 960);
                    Ball.State = BallState.Paddle;
                    //ball.Speed += 10;
                    Clear = true;

                    ++Level;
                    ClearShots();
                    ClearInvaders();
                    LoadLevel();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (InvaderShot j in this.InvaderShots)
            {
                j.Draw(gameTime);
            }
            foreach (Invader i in this.Invaders)
            {
                i.Draw(gameTime);
            }//*/
            foreach (UFO k in ufo)
            {
                k.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void LoadLevel()
        {
            interval = initialIntervval - (Level * levelDecrement);
            row = 5;
            direction = 1;
            down = false;
            moveDown = 0;

            Invader i;
            for (int x = 11; x > 0; --x)
            {
                for (int y = 5; y > 0; --y)
                {
                    if (y > 3)
                    {
                        i = new LargeInvader(this.Game);
                    }
                    else if (y > 1)
                    {
                        i = new MediumInvader(this.Game);
                    }
                    else
                    {
                        i = new SmallInvader(this.Game);
                    }
                    i.Initialize();
                    i.Column = x;
                    i.Row = y;
                    i.Location = new Vector2(hardCodedLeftMargin + (x * xBuffer), hardCodedTopMargin + (y * i.SpriteTexture.Height + (y * yBuffer )));
                    Invaders.Add(i);
                }
            }
        }

        protected void UpdateMovement()
        {
            this.console.GameConsoleWrite("Interval =  " + interval.ToString());
            this.console.GameConsoleWrite("Level =  " + Level.ToString());

            foreach (Invader i in Invaders)
            {
                i.ChangeSprite();

                if (row == 5 && (i.Location.X >= (768 - 64)  || i.Location.X <= 64) && !down)
                {
                    down = true;
                    direction *= -1;
                    break;
                }
            }

            if (moveDown == 5)
            {
                down = false;
                moveDown = 0;
                interval -= intervalDecrement;
                if (interval < minimumInterval)
                {
                    interval = minimumInterval;
                }
            }

            if (down)
            {
                MoveDown();
                ++moveDown;
            }
            else
            {
                MoveHorizontal();
            }

            CycleRow();
        }

        protected void MoveHorizontal()
        {
            foreach (Invader i in Invaders)
            {
                if (i.Row == row && !i.WasHit)
                {
                    i.Location.X += (direction * xBuffer);
                }
            }
        }

        protected void MoveDown()
        {
            foreach (Invader i in Invaders)
            {
                if (i.Row == row && !i.WasHit)
                {
                    i.Location.Y += (yBuffer * 2f);
                }
            }
        }

        protected void CycleRow() // cycles row between 0 - 4 for the sake of moving invaders
        {
            if (row != 1)
            {
                --row;
            }
            else // if (row == 4)
            {
                row = 5;
            }
        }

        protected void UpdateCollision(GameTime gameTime)
        {
            foreach (Invader i in Invaders)
            {
                if (!i.Explode)
                {
                    if (i.Intersects(Ball))
                    {
                        if (!Reflected && !i.WasHit)
                        {
                            GameManager.Score += 100;
                            Ball.Reflect(i);
                            this.Reflected = true;
                        }

                        i.Hit();
                    }
                }
                else
                {
                    destroyedInvaders.Add(i);
                }
            }

            foreach (InvaderShot j in InvaderShots)
            {
                if (!j.Enabled) // used for cleaning up shots that have gone below the screen
                {
                    removedShots.Add(j);
                }
            }

            foreach (UFO k in ufo)
            {
                if (!k.Explode)
                {
                    if (k.Intersects(Ball))
                    {
                        if (!Reflected && !k.WasHit)
                        {
                            int points = r.Next(0, 3);
                            if (points == 0)
                            {
                                GameManager.Score += 1000;
                            }
                            else if (points == 1)
                            {
                                GameManager.Score += 500;
                            }
                            else // if (points == 2)
                            {
                                GameManager.Score += 2000;
                            }

                            Ball.Reflect(k);
                            this.Reflected = true;
                        }

                        k.Hit();
                    }

                    if (k.Location.X > 1000)
                    {
                        k.Visible = false;
                        k.Enabled = false;
                        destroyedUFO.Add(k);
                    }
                }
                else
                {
                    destroyedUFO.Add(k);
                }
            }
        }

        protected void UpdateRemove()
        {
            foreach (Invader i in destroyedInvaders)
            {
                Invaders.Remove(i);
            }

            destroyedInvaders.Clear();

            foreach (InvaderShot j in removedShots)
            {
                InvaderShots.Remove(j);
            }

            removedShots.Clear();

            foreach (UFO k in destroyedUFO)
            {
                ufo.Remove(k);
            }

            destroyedUFO.Clear();
        }
        
        protected void Shoot()
        {
            int shot = r.Next(0, shotChance);

            if (shot == 0 && InvaderShots.Count < 5)
            {//*/
                int location = r.Next(0, Invaders.Count);
                InvaderShot i = new InvaderShot(this.Game, Invaders[location].Location);
                i.Initialize();
                InvaderShots.Add(i);
            }

            int uChance = r.Next(0, 48);

            if (uChance == 1 && ufo.Count < 1)
            {
                Mothership();
            }
        }

        public void ClearShots()
        {
            foreach(InvaderShot i in InvaderShots)
            {
                removedShots.Add(i);
            }

            foreach (InvaderShot j in removedShots)
            {
                InvaderShots.Remove(j);
            }

            removedShots.Clear();
        }

        public void ClearInvaders()
        {
            foreach (Invader i in Invaders)
            {
                destroyedInvaders.Add(i);
            }

            foreach (Invader j in destroyedInvaders)
            {
                Invaders.Remove(j);
            }

            destroyedInvaders.Clear();

            foreach (UFO k in ufo)
            {
                destroyedUFO.Add(k);
            }

            foreach (UFO l in destroyedUFO)
            {
                ufo.Remove(l);
            }

            destroyedUFO.Clear();
        }

        protected void CheckLoss()
        {
            foreach(Invader i in Invaders)
            {
                if (i.Location.Y >= invaderThreshold)
                {
                    GameManager.Lives = -1;
                    Lose = true;
                    break;
                }
            }

            if (Lose)
            {
                ClearInvaders();
                ClearShots();
            }
        }

        protected void Mothership()
        {
            UFO u = new UFO(this.Game);
            u.Location = new Vector2((32f * -5f), 16f + (32f * 3f));
            u.Initialize();
            ufo.Add(u);
        }
    }
}
