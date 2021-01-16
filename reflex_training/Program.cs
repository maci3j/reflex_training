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
    enum LogLevel
    {
        Verbose,
        Info,
        Error
    }
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

        public static void StartGame(TimeSpan TimeToEnd, bool MovingTargets, bool ResizableTargets, int TargetLifetime, int StartingTargets, TimeSpan TargetAddTime)
        {
            if (GameThread != null)
            {
                if (GameThread.IsAlive)
                {
                    Program.Debug(LogLevel.Error, "GameThread aborted");
                    GameThread.Abort();
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
