using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace reflex_training
{
    /// <summary>
    /// Enumerator defining debug message levels.
    /// </summary>
    enum LogLevel
    {
        Verbose,
        Info,
        Error
    }

    /// <summary>
    /// Static program definition class.
    /// </summary>
    static class Program
    {
        public static Game game;
        public static GameForm gf;
        static Thread GameThread;
        public static LogLevel LogLevel;
        static FileLogger fl;
        public static GameType SelectedType;
        /// <summary>
        /// Application entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogLevel = LogLevel.Info;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            gf = new GameForm();
            StartGame(new TimeSpan(0), false, false, 0, 0 , new TimeSpan(0));
            gf.ShowMenu();
            Application.Run(gf);
        }

        /// <summary>
        /// Debug messages handler function.
        /// It serves debug to the console and writes it to the file.
        /// </summary>
        /// <param name="level">Level of the message, see LogLevel</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [Conditional("DEBUG")]
        public static void Debug(LogLevel level, string format, params object[] args)
        {
            if (level >= LogLevel)
            {
                fl = new FileLogger();
                Console.WriteLine(format, args);
                Task.Run(() => fl.WriteData("log.txt", format, args));
            }
        }

        /// <summary>
        /// Main round starting procedure.
        /// </summary>
        /// <param name="TimeToEnd">Time after round ends</param>
        /// <param name="MovingTargets">Set if targets should be moving</param>
        /// <param name="ResizableTargets">Set if targets should change their sizes</param>
        /// <param name="TargetLifetime">Time to targets self-destruction</param>
        /// <param name="StartingTargets">Amount of targets at start of the round</param>
        /// <param name="TargetAddTime">Time interval after which the new target will be created</param>
        public static void StartGame(TimeSpan TimeToEnd, bool MovingTargets, bool ResizableTargets, int TargetLifetime, int StartingTargets, TimeSpan TargetAddTime)
        {
            Program.Debug(LogLevel.Error, "StartGame called");
            if (GameThread != null)
            {
                if (GameThread.IsAlive && GameThread.ThreadState != System.Threading.ThreadState.AbortRequested)
                {
                    Program.Debug(LogLevel.Error, "GameThread aborted");
                    try
                    {
                        GameThread.Abort();
                    }
                    catch(ThreadAbortException e)
                    {

                        Program.Debug(LogLevel.Error, "Therad abort exception: {0}", e);
                        Thread.ResetAbort();
                    }
                    GameThread = null;
                }
            }
            game = new Game(SelectedType, TimeToEnd, MovingTargets, ResizableTargets, TargetLifetime, StartingTargets, TargetAddTime);
            GameThread = new Thread(new ParameterizedThreadStart(game.GameLoop));
            GameThread.IsBackground = true;
            GameThread.Start(gf);
            game.Start();
        }
    }
}
