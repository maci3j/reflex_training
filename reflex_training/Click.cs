using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reflex_training
{
    class Click
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Score { get; set; }
        public double Distance { get; set; } = Double.MaxValue; // if missed, store distance to closest target
        public TimeSpan TargetLiveTime { get; set; } // if hit, store how long target has been hanging around
        public TimeSpan ClickTime { get; set; } // holds when click was done

        public Click(int x, int y, TimeSpan time)
        {
            X = x;
            Y = y;
            ClickTime = time;
            Program.Debug(LogLevel.Info, "Click: {0}, {1}, time:{2}", x, y, time);
        }
    }
}
