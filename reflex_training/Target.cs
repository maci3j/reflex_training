using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reflex_training
{
    public class Target
    {
        public int x { get; private set; }
        public int y { get; private set; }
        private int prev_x, prev_y;
        int size;
        int TimeToDestroy;
        Boolean Moving;
        public int Direction { get; private set; } // movement vector direction, in degrees
        public int Speed { get; private set; } // speed of move, in pixels per one tick
        public int LifeTime { get; set; } // time, in ticks elapsed from target creation


        public Target(bool Moving)
        {
            this.Moving = Moving;
            Random rnd = new Random();
            do
            {
                x = rnd.Next(Program.gf.GetBoardSize().Width);
            } while (x < 50 || x > Program.gf.GetBoardSize().Width - 50);
            do
            {
                y = rnd.Next(Program.gf.GetBoardSize().Height);
            } while (y < 50 || y > Program.gf.GetBoardSize().Height - 50);
            do
            {
                size = rnd.Next(100);
            } while (size < 25);

            if (Moving)
            {
                RandomizeSpeed();
                RandomizeDirection();
            }
            Program.Debug(LogLevel.Info, "Target: x:{0}, y:{1}, size:{2}, moving: {3}, speed: {4}, angle: {5}", x, y, size, this.Moving, Speed, Direction);
        }

        public int GetSize()
        {
            return size;
        }

        public bool IsMoving()
        {
            return Moving;
        }

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

        public void RandomizeDirection()
        {
            Direction = new Random().Next(360);
        }

        public void RandomizeSpeed()
        {
            Speed = new Random().Next(30);
        }

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

        public bool IsHit(int x, int y)
        {
            double x_center = this.x + (size / 2);
            double y_center = this.y + (size / 2);
            return Math.Sqrt((Math.Pow(x_center - x, 2) + Math.Pow(y_center - y, 2))) <= (size / 2);
        }
    }
}
