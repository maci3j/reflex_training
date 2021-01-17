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
    /// <summary>
    /// GameType enumerator.
    /// </summary>
    enum GameType
    {
        None,
        Training,
        TimeTrial,
        ScoreTrial
    }

    /// <summary>
    /// Definition of the Game object.
    /// </summary>
    class Game
    {
        /// <summary>
        /// Time elapsed from start of the round.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
        /// <summary>
        /// Time length of the current frame.
        /// </summary>
        public TimeSpan ticktime { get; private set; }
        /// <summary>
        /// Mutex used to synchronize operations.
        /// </summary>
        public Mutex TargetsMutex = new Mutex(); // multithreaded operations on target list have to be synchronized to avoid data corruption
        /// <summary>
        /// Frame per second counter.
        /// </summary>
        public int Fps { get; private set; } = 30;

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
        TimeSpan TimeToEnd;
        List<int> ticktimes = new List<int>();
        bool MovingTargets;
        bool ResizableTargets;
        int TargetSize = 10;
        int TargetLifetime;
        bool HardPause;
        int StartingTargets;
        int TargetsGone;
        TimeSpan TargetAddTime;
        TimeSpan TargetAddDelta;
        GameForm gf1;

        Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Game constructor.
        /// </summary>
        /// <param name="type">Type of the game - see GameType</param>
        /// <param name="TimeToEnd">Round length</param>
        /// <param name="MovingTargets">Set if the targets should be moving</param>
        /// <param name="ResizableTargets">Set if the targets should change their sizes</param>
        /// <param name="TargetLifetime">Time to target self-destroy, in ticks</param>
        /// <param name="StartingTargets">Amout of targets created at start of the round</param>
        /// <param name="TargetAddTime">Time interval after which the new target will be created</param>
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

        /// <summary>
        /// Main loop of the game.
        /// </summary>
        /// <param name="gf">GameForm object</param>
        public void GameLoop(object gf)
        {
            gf1 = (GameForm)gf;
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

                if((sw.Elapsed - SecondCount).Seconds >= 1)
                {
                    if ((sw.Elapsed - SecondCount).Seconds < 2) // if more than 2 seconds elapsed, something gone wrong with time measurement - do not update then
                    {
                        SecondTimer(); // execute method every one second
                    }
                    SecondCount = sw.Elapsed;
                }

                if (type == GameType.ScoreTrial)
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

        /// <summary>
        /// Returns list of targets currently on board.
        /// </summary>
        /// <returns>List of targets</returns>
        public List<Target> GetTargets()
        {
            return targets;
        }

        /// <summary>
        /// Updates the board.
        /// </summary>
        /// <param name="form">Current GameForm</param>
        void UpdateBoard(GameForm form)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new MethodInvoker(() => UpdateBoard(form)));
                return;
            }
            form.UpdateBoard();
        }

        /// <summary>
        /// Makes per-game-tick targets update.
        /// </summary>
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
                if(t.LifeTime>t.TimeToDestroy && type == GameType.ScoreTrial)
                {
                    Program.Debug(LogLevel.Info, "Removing expired target, lifetime:{0}, timetodestroy: {1}", t.LifeTime, t.TimeToDestroy);
                    TargetsGone++;
                    ToBeRemoved.Add(t);
                }
                t.LifeTime++;
            }

            foreach (Target t in ToBeRemoved)
            {
                if ((targets.Count - ToBeRemoved.Count) <= 0 && type == GameType.ScoreTrial)
                {
                    EndGame();
                    break;
                }
                targets.Remove(t);
                if(type == GameType.TimeTrial)
                {
                    AddTarget();
                }
            }
            TargetsMutex.ReleaseMutex();
        }

        /// <summary>
        /// Method executed every second.
        /// </summary>
        void SecondTimer()
        {
            Fps = TickCount;
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
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

        /// <summary>
        /// Creates new target.
        /// </summary>
        public void AddTarget()
        {
            targets.Add(new Target(MovingTargets, ResizableTargets, TargetLifetime));
        }

        /// <summary>
        /// Adds hit made in current round.
        /// </summary>
        public void AddHit()
        {
            hits++;
            Program.Debug(LogLevel.Info, "Target hit, current score: {0}", hits);
        }

        /// <summary>
        /// Returns amount of hits made in current round.
        /// </summary>
        /// <returns>Number of hits</returns>
        public int GetHits()
        {
            return hits;
        }

        /// <summary>
        /// Adds miss made in current round.
        /// </summary>
        public void AddMiss()
        {
            misses++;
            Program.Debug(LogLevel.Info, "Miss, current misses: {0}", misses);
        }

        /// <summary>
        /// Returns amount of misses made in current round.
        /// </summary>
        /// <returns>Number of misses</returns>
        public int GetMisses()
        {
            return misses;
        }

        /// <summary>
        /// Resumes game after pause.
        /// </summary>
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

        /// <summary>
        /// Pauses game.
        /// </summary>
        public void Pause()
        {
            state = false;
            Program.Debug(LogLevel.Error, "Game paused");
        }

        /// <summary>
        /// Returns current game state.
        /// </summary>
        /// <returns>State of the game</returns>
        public bool Running()
        {
            return state;
        }

        /// <summary>
        /// Handles operations related with hit detection.
        /// </summary>
        /// <param name="x">X coordinate of click</param>
        /// <param name="y">Y coordinate of click</param>
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
                        click.TargetLiveTime = TimeSpan.FromMilliseconds(t.LifeTime*(1000/30.0));
                        targets.Remove(t);
                        AddHit();
                        if (type == GameType.TimeTrial || type == GameType.Training)
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

        /// <summary>
        /// Ends current round.
        /// </summary>
        void EndGame()
        {
            Pause();
            HardPause = true;
            Program.Debug(LogLevel.Error, "Game ended, hits: {0}, misses: {1}", clicks.Where(x => x.Score).Count(), clicks.Where(x => !x.Score).Count());
            String clicks_str = "";
            String clicks_stats = "";
            int index = 0;
            foreach(Click c in clicks)
            {
                index++;
                clicks_str += String.Format("Click x:{0}, y:{1}, hit: {2}, distance to nearest target: {3}, target lifetime: {4}\n",
                                                c.X,
                                                c.Y,
                                                c.Score,
                                                c.Distance,
                                                c.TargetLiveTime
                                           );
                clicks_stats += String.Format("Strzał#{0} x:{1}, y:{2}, trafienie: {3}{4}{5}\n",
                                                index,
                                                c.X,
                                                c.Y,
                                                (c.Score ? "Tak":"Nie"),
                                                (!c.Score ? String.Format(", dystans do najbliższego celu: {0}", c.Distance) : ""),
                                                (c.TargetLiveTime != TimeSpan.FromTicks(0) ? String.Format(", czas życia celu: {0}", c.TargetLiveTime) : "")
                                             );
            }
            Program.Debug(LogLevel.Error, clicks_str);
            FileLogger fl;
            fl = new FileLogger();
            string text = String.Format("Czas gry: {0}\nTrafienia: {1}\nChybienia: {2}\nCelność: {3}%",
                                            ElapsedTime,
                                            GetHits(),
                                            GetMisses(),
                                            ((Program.game.GetHits() != 0 || Program.game.GetMisses() != 0) ? 100 * Program.game.GetHits() / (Program.game.GetHits() + Program.game.GetMisses()) : 0)
                                        );
            Task.Run(() => fl.WriteData(String.Format("{0}.txt", DateTime.Now.ToString("dd_MM_yyyy_HH-mm-ss")),
                                            "Data: {0}\nTryb gry: {1}\n{2}{3}{4}{5}{6}{7}\n{8}",
                                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                                type.ToString(),
                                                String.Format("Poruszające się cele: {0}\n", MovingTargets ? "Tak" : "Nie"), 
                                                type == GameType.ScoreTrial ? (String.Format("Zmieniające wielkość cele: {0}\n", ResizableTargets ? "Tak" : "Nie")) : "", 
                                                type == GameType.ScoreTrial ? String.Format("Czas życia celu: {0}\n", TimeSpan.FromMilliseconds(TargetLifetime*(1000/30.0))) : "", 
                                                String.Format("Ilość celów na start: {0}\n", StartingTargets),
                                                type == GameType.ScoreTrial ? String.Format("Czas pojawiania się kolejnych celów: {0}\n", TargetAddTime) : "",
                                                text,
                                                clicks_stats
                                       ));

            var box = MessageBox.Show(text, "Koniec gry!", MessageBoxButtons.OK);
            if(box == DialogResult.OK)
            {
                gf1.ShowMenu();
            }
        }

        /// <summary>
        /// Game object destructor.
        /// </summary>
        ~Game()
        {
            Program.Debug(LogLevel.Error, "GameObject destroyed");
        }
    }
}
