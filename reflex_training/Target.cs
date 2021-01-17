using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reflex_training
{
    /// <summary>
    /// Class that defines Target object and it's methods.
    /// </summary>
    public class Target
    {
        /// <summary>
        /// Stores x coordinate of a target.
        /// </summary>
        public int x { get; private set; }
        /// <summary>
        /// Stires y coordinate of a target.
        /// </summary>
        public int y { get; private set; }
        private int prev_x, prev_y;
        double Size;
        double FinalSize = 0;
        double SizeDelta = 0; // change of object's size per tick
        bool Moving;
        bool Resizable;
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        
        /// <summary>
        /// Stores movement vector direction, in degrees.
        /// </summary>
        public int Direction { get; private set; }
        /// <summary>
        /// Stores movement speed of a target, in pixels per game tick.
        /// </summary>
        public int Speed { get; private set; }
        /// <summary>
        /// Stores how long the target is alive, in ticks.
        /// </summary>
        public int LifeTime { get; set; }
        /// <summary>
        /// Stores how long target should be alive until self destruction.
        /// </summary>
        public int TimeToDestroy { get; private set; }

        /// <summary>
        /// Constructor of the Target object.
        /// </summary>
        /// <param name="Moving">Set if the target should move</param>
        /// <param name="Resizable">Set if the target should change size</param>
        /// <param name="TimeToDestroy">Time in ticks to self-destroy the target</param>
        public Target(bool Moving, bool Resizable, int TimeToDestroy = 0)
        {
            this.Moving = Moving;
            this.TimeToDestroy = TimeToDestroy;

            do
            {
                x = rnd.Next(Program.gf.GetBoardSize().Width);
            } while (x < 50 || x > Program.gf.GetBoardSize().Width - 50);

            do
            {
                y = rnd.Next(Program.gf.GetBoardSize().Height);
            } while ((y < 50 || y > Program.gf.GetBoardSize().Height - 50) && y == x);

            do
            {
                Size = Convert.ToInt32(rnd.Next(100));
            } while (Size < 25);

            if (Moving)
            {
                RandomizeSpeed();
                RandomizeDirection();
            }

            if (Resizable && TimeToDestroy > 0)
            {
                this.Resizable = Resizable;
                do
                {
                    //FinalSize = Convert.ToInt32(rnd.Next(Convert.ToInt32(Size)));
                    FinalSize = 20;
                } while (FinalSize == Size);

                SizeDelta = (Size - FinalSize) / TimeToDestroy;
            }
            Program.Debug(LogLevel.Info, "Target: x:{0}, y:{1}, size:{2}, moving: {3}, speed: {4}, angle: {5}, final size: {6}, time to destroy: {7}", x, y, Size, this.Moving, Speed, Direction, FinalSize, TimeToDestroy);
        }

        /// <summary>
        /// Returns size of the target.
        /// </summary>
        /// <returns>Target's size</returns>
        public double GetSize()
        {
            return Size;
        }

        /// <summary>
        /// Returns if the target is set to moving.
        /// </summary>
        /// <returns></returns>
        public bool IsMoving()
        {
            return Moving;
        }

        /// <summary>
        /// Returns if the target is set to resize.
        /// </summary>
        /// <returns></returns>
        public bool IsResizable()
        {
            return Resizable;
        }

        /// <summary>
        /// Resize target.
        /// </summary>
        public void Resize()
        {
            if (Size > 20)
            {
                Size -= SizeDelta;
            }
        }

        /// <summary>
        /// Handles target's movement.
        /// </summary>
        /// <param name="max_x">Maximum x coordinate</param>
        /// <param name="max_y">Maximum y coordinate</param>
        public void Move(int max_x, int max_y)
        {
            if (x <= 0 || x >= (max_x - GetSize()) || y <= 0 || y >= (max_y - GetSize()))
            {
                CalculateCollision(max_x, max_y);
            }
            prev_x = x;
            prev_y = y;
            x = Convert.ToInt32(x + Speed * Math.Cos(Direction * Math.PI / 180));
            y = Convert.ToInt32(y + Speed * Math.Sin(Direction * Math.PI / 180));
            Program.Debug(LogLevel.Verbose, "Moving, prev: {0} {1}, next: {2} {3}", prev_x, prev_y, x, y);
        }

        /// <summary>
        /// Sets target's movement direction to the random value.
        /// </summary>
        public void RandomizeDirection()
        {
            Direction = rnd.Next(360);
        }

        /// <summary>
        /// Sets target's speed to the random value.
        /// </summary>
        public void RandomizeSpeed()
        {
            Speed = rnd.Next(30);
        }

        /// <summary>
        /// Handles collision detection and target behaviour when it happens. 
        /// </summary>
        /// <param name="max_x">Maximum x coordinate</param>
        /// <param name="max_y">Maximum y coordinate</param>
        public void CalculateCollision(int max_x, int max_y)
        {
            Program.Debug(LogLevel.Info, "\nCollision at x:{0}, y:{1}, angle:{2}", x, y, Direction);

            int b;
            if (x <= 0)
            {
                Program.Debug(LogLevel.Info, "Left wall contact");
                if (Direction >= 180 && Direction < 270)
                {
                    b = 270 - Direction;
                    Direction = 270 + b;
                }
                else if (Direction >= 90 && Direction < 180)
                {
                    b = Direction - 90;
                    Direction = 90 - b;
                }
            }
            else if (x >= max_x - GetSize())
            {
                Program.Debug(LogLevel.Info, "Right wall contact");
                if (Direction >= 270 && Direction <= 360)
                {
                    b = Direction - 270;
                    Direction = 270 - b;
                }
                else
                {
                    b = 90 - Direction;
                    Direction = 90 + b;
                }
            }

            else if (y <= 0)
            {
                Program.Debug(LogLevel.Info, "Top wall contact");
                if (Direction >= 180 && Direction < 270)
                {
                    b = Direction - 180;
                    Direction = 180 - b;
                }
                else if (Direction >= 270 && Direction < 360)
                {
                    b = 360 - Direction;
                    Direction = b;
                }
            }
            else if (y >= max_y - GetSize())
            {
                Program.Debug(LogLevel.Info, "Bottom wall contact");
                if (Direction >= 0 && Direction < 90)
                {
                    Direction = 360 - Direction;
                }
                else if (Direction >= 90 && Direction < 180)
                {
                    b = 180 - Direction;
                    Direction = 180 + b;
                }
            }
            Program.Debug(LogLevel.Info, "Bouncing back, angle:{0}\n", Direction);
            if (Direction >= 360)
            {
                Program.Debug(LogLevel.Error, "Target angle overflow!");
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns if target was hit by a click.
        /// </summary>
        /// <param name="x">X coordinate of the click</param>
        /// <param name="y">Y coordinate of the click</param>
        /// <returns>If target was hit or not</returns>
        public bool IsHit(int x, int y)
        {
            double x_center = this.x + (Size / 2);
            double y_center = this.y + (Size / 2);
            return Math.Sqrt((Math.Pow(x_center - x, 2) + Math.Pow(y_center - y, 2))) <= (Size / 2);
        }

        /// <summary>
        /// Calculates distance between click and target.
        /// </summary>
        /// <param name="x">X coordinate of the click</param>
        /// <param name="y">Y coordinate of the click</param>
        /// <returns>Distance to the target</returns>
        public double CalculateDistance(int x, int y)
        {
            return Math.Sqrt(Math.Pow(this.x - x, 2) + Math.Pow(this.y - y, 2)) - Size / 2.0;
        }
    }
}
