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
        None,
        Training,
        TimeTrial,
        ScoreTrial
    }

    class Game
    {
        bool state;
        int hits = 0;
        int misses = 0;
        GameType type;
        TimeSpan FrameTime = TimeSpan.FromSeconds(1.0/30); // 30 frames per seconds - physics ticks as well
        List<Target> targets = new List<Target>();
        List<Click> clicks = new List<Click>();
        int spin_counter = 0;
        int max_x;
        int max_y;
        int TickCount;
        TimeSpan SecondCount;
        public TimeSpan ElapsedTime { get; private set; }
        TimeSpan TimeToEnd;
        public TimeSpan ticktime { get; private set; }
        List<int> ticktimes = new List<int>();
        public Mutex TargetsMutex = new Mutex(); // multithreaded operations on target list have to be synchronized to avoid data corruption
        public int Fps { get; private set; } = 30;
        bool MovingTargets;
        bool ResizableTargets;
        int TargetSize = 10;
        int TargetLifetime;
        bool HardPause;
        int StartingTargets;
        int TargetsGone;
        TimeSpan TargetAddTime;
        TimeSpan TargetAddDelta;

        Stopwatch sw = new Stopwatch();

        public Game(GameType type, TimeSpan TimeToEnd, bool MovingTargets, bool ResizableTargets, int TargetLifetime, int StartingTargets, TimeSpan TargetAddTime)
        {
            Program.Debug(LogLevel.Error, "Game initialized, type: {0}, TimeToEnd: {1}, MovingTargets: {2}, ResizableTargets: {3}, TargetLifetime: {4}, StartingTargets: {5}, TargetAddTime: {6}", 
                            type, TimeToEnd, MovingTargets, ResizableTargets, TargetLifetime, StartingTargets, TargetAddTime);
            this.type = type;
            this.TimeToEnd = TimeToEnd;
            this.MovingTargets = MovingTargets;
            this.ResizableTargets = ResizableTargets;
            this.TargetLifetime = TargetLifetime;
            this.StartingTargets = StartingTargets;
            this.TargetAddTime = TargetAddTime;

            if(type == GameType.ScoreTrial)
            {
                this.ResizableTargets = true;
            }
        }

        public void GameLoop(object gf)
        {
            GameForm gf1 = (GameForm)gf;
            Program.game.TargetsMutex.WaitOne();
            for(int i = 0; i<StartingTargets; i++)
            {
                AddTarget();
            }
            Program.game.TargetsMutex.ReleaseMutex();

            sw.Start();
            var last_time = sw.Elapsed;
            SecondCount = sw.Elapsed;
            TargetAddDelta = sw.Elapsed;
            var update_time = new TimeSpan(0);
            while (true)
            {
                var delta = sw.Elapsed - last_time;
                last_time += delta;
                update_time += delta;

                if((sw.Elapsed.Seconds - SecondCount.Seconds) >= 1)
                {
                    if ((sw.Elapsed.Seconds - SecondCount.Seconds) < 2) // if more than 2 seconds elapsed, something gone wrong with time measurement - do not update then
                    {
                        SecondTimer(); // execute method every one second
                    }
                    SecondCount = sw.Elapsed;
                }

                if (type == GameType.TimeTrial)
                {
                    if ((sw.Elapsed - TargetAddDelta) >= TargetAddTime)
                    {
                        AddTarget();
                        TargetAddDelta = sw.Elapsed;
                    }
                }

                while (update_time > FrameTime)
                {
                    var tickstart = sw.Elapsed;
                    update_time -= FrameTime;
                    max_x = gf1.GetBoardSize().Width;
                    max_y = gf1.GetBoardSize().Height;

                    UpdateTargets(); // do some per frame target operations
                    UpdateBoard(gf1); // flush the board

                    Program.Debug(LogLevel.Verbose, "UpdateTime: {0}", update_time);

                    spin_counter = 0;
                    TickCount++;
                    ticktime = sw.Elapsed - tickstart;
                    ticktimes.Add(ticktime.Milliseconds);
                    if (ticktime > FrameTime)
                        Program.Debug(LogLevel.Error, "Ticktime {0} ms exceeded frame time!", ticktime.Milliseconds);
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
            List<Target> ToBeRemoved = new List<Target>();
            TargetsMutex.WaitOne();
            foreach(Target t in targets)
            {
                if(t.IsMoving())
                {
                    t.Move(max_x, max_y);
                }
                if(t.IsResizable())
                {
                    t.Resize();
                }
                if(t.LifeTime>t.TimeToDestroy && type == GameType.TimeTrial)
                {
                    Program.Debug(LogLevel.Info, "Removing expired target, lifetime:{0}, timetodestroy: {1}", t.LifeTime, t.TimeToDestroy);
                    TargetsGone++;
                    ToBeRemoved.Add(t);
                }
                t.LifeTime++;
            }

            foreach (Target t in ToBeRemoved)
            {
                if ((targets.Count - ToBeRemoved.Count) <= 0 && type == GameType.TimeTrial)
                {
                    EndGame();
                    break;
                }
                targets.Remove(t);
                if(type == GameType.ScoreTrial)
                {
                    AddTarget();
                }
            }
            TargetsMutex.ReleaseMutex();
        }

        void SecondTimer()
        {
            Fps = TickCount;
            ElapsedTime += TimeSpan.FromSeconds(1);
            Program.Debug(LogLevel.Info, "Ticks per second: {0}", TickCount);
            Program.Debug(LogLevel.Info, "Average ticktime: {0} ms", ticktimes.Average());
            Program.Debug(LogLevel.Info, "Elapsed time: {0} seconds", ElapsedTime.Seconds);
            ticktimes.Clear();
            TickCount = 0;

            if(type == GameType.TimeTrial && ElapsedTime >= TimeToEnd && TimeToEnd.Ticks != 0)
            {
                EndGame();
            }
        }

        public void AddTarget()
        {
            targets.Add(new Target(MovingTargets, ResizableTargets, TargetLifetime));
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
            if (!HardPause)
            {
                state = true;
                SecondCount = sw.Elapsed;
                TargetAddDelta = sw.Elapsed;
                Program.Debug(LogLevel.Error, "Game started");
            }
            else
            {
                Program.Debug(LogLevel.Error, "Already finished game tried to be started!");
            }
        }

        public void Pause()
        {
            state = false;
            Program.Debug(LogLevel.Error, "Game paused");
        }

        public bool Running()
        {
            return state;
        }

        public void ClickHandler(int x, int y)
        {
            if (Running()) // click should be registered only when game is running
            {
                bool hit = false;
                Click click = new Click(x, y, sw.Elapsed);
                TargetsMutex.WaitOne();
                foreach (Target t in targets)
                {
                    Program.Debug(LogLevel.Info, "Hit: {0}", t.IsHit(x, y));
                    if (t.IsHit(x, y))
                    {
                        hit = true;
                        targets.Remove(t);
                        AddHit();
                        if (type == GameType.ScoreTrial || type == GameType.Training)
                        {
                            AddTarget();
                        }
                        click.Score = true;
                        click.Distance = 0;
                        break;
                    }
                    if (t.CalculateDistance(x, y) < click.Distance)
                    {
                        click.Distance = t.CalculateDistance(x, y);
                    }
                }
                TargetsMutex.ReleaseMutex();
                Program.Debug(LogLevel.Info, "Distance to closest target: {0}", click.Distance);
                clicks.Add(click);
                if(!hit)
                    AddMiss();
            }
        }

        void EndGame()
        {
            Pause();
            HardPause = true;
            Program.Debug(LogLevel.Error, "Game ended, hits: {0}, misses: {1}", clicks.Where(x => x.Score).Count(), clicks.Where(x => !x.Score).Count());

            foreach(Click c in clicks)
            {
                Program.Debug(LogLevel.Error, "Click x:{0}, y:{1}, hit: {2}, distance to nearest target: {3}, target lifetime: {4}", c.X, c.Y, c.Score, c.Distance, c.TargetLiveTime);
            }
        }

        ~Game()
        {
            Program.Debug(LogLevel.Error, "GameObject destroyed");
        }
    }
}
