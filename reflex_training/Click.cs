using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reflex_training
{
    /// <summary>
    /// Class that defines Click object.
    /// </summary>
    class Click
    {
        /// <summary>
        /// X coordinate of click.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y coordinate of click.
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Stores if a target was hit.
        /// </summary>
        public bool Score { get; set; }
        /// <summary>
        /// Stores distance to nearest target if missed.
        /// </summary>
        public double Distance { get; set; } = Double.MaxValue;
        /// <summary>
        /// If hit, store how long target has been on board.
        /// </summary>
        public TimeSpan TargetLiveTime { get; set; }
        /// <summary>
        /// Stores when click was made.
        /// </summary>
        public TimeSpan ClickTime { get; set; }

        /// <summary>
        /// Constructor of the Click object.
        /// </summary>
        /// <param name="x">X coordinate of click</param>
        /// <param name="y">Y coordinate of click</param>
        /// <param name="time">Time when click has been made, relative to round start</param>
        public Click(int x, int y, TimeSpan time)
        {
            X = x;
            Y = y;
            ClickTime = time;
            Program.Debug(LogLevel.Info, "Click: {0}, {1}, time:{2}", x, y, time);
        }
    }
}
