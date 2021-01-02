using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace reflex_training
{
    enum GameType
    {
        Standard,
        Casual
    }
    class Game
    {
        bool state;
        int hits = 0;
        int misses = 0;
        GameType type;
        TimeSpan FrameTime = TimeSpan.FromSeconds(1.0/30); // 30 frames per seconds - physics ticks as well
        List<Target> targets = new List<Target>();
        int spin_counter = 0;
        int max_x;
        int max_y;
        int TickCount;
        TimeSpan SecondCount;

        Stopwatch sw = new Stopwatch();

        public void GameLoop(object gf)
        {
            GameForm gf1 = (GameForm)gf;
            AddTarget(true);


            sw.Start();
            var last_time = sw.Elapsed;
            SecondCount = sw.Elapsed;
            var update_time = new TimeSpan(0);
            while (true)
            {
                var delta = sw.Elapsed - last_time;
                last_time += delta;
                update_time += delta;
                if((sw.Elapsed.Seconds - SecondCount.Seconds) >= 1)
                {
                    SecondCount = sw.Elapsed;
                    SecondTimer(); // execute method every one second
                }

                while(update_time > FrameTime)
                {
                    update_time -= FrameTime;
                    max_x = gf1.GetBoardSize().Width;
                    max_y = gf1.GetBoardSize().Height;
                    UpdateTargets(); // do some per frame target operations
                    UpdateBoard(gf1);
                    Program.Debug(LogLevel.Verbose, "UpdateTime: {0}", update_time);
                    spin_counter = 0;
                    TickCount++;
                }
                Thread.Sleep(1);
                spin_counter++;
                Program.Debug(LogLevel.Verbose, "SpinCounter: {0}", spin_counter); // how many waiting loops passed until new frame

                while (!state) // if paused, wait
                {
                    last_time = sw.Elapsed;
                    Thread.Sleep(10);
                }
            }
        }

        public List<Target> GetTargets()
        {
            return targets;
        }

        void UpdateBoard(GameForm form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new MethodInvoker(() => UpdateBoard(form)));
                return;
            }
            form.UpdateBoard();
        }

        void UpdateTargets()
        {
            foreach(Target t in GetTargets())
            {
                if(t.IsMoving())
                {
                    t.Move(max_x, max_y);
                }
                t.LifeTime++;
            }
        }

        void SecondTimer()
        {
            Program.Debug(LogLevel.Info, "Ticks per second: {0}", TickCount);
            TickCount = 0;
        }

        public void AddTarget(bool moving)
        {
            targets.Add(new Target(moving));
        }

        public void AddHit()
        {
            hits++;
            Program.Debug(LogLevel.Info, "Target hit, current score: {0}", hits);
        }

        public int GetHits()
        {
            return hits;
        }

        public void AddMiss()
        {
            misses++;
            Program.Debug(LogLevel.Info, "Miss, current misses: {0}", misses);
        }

        public int GetMisses()
        {
            return misses;
        }

        public void Start()
        {
            state = true;
        }

        public void Pause()
        {
            state = false;
        }

        public bool State()
        {
            return state;
        }
    }
}
