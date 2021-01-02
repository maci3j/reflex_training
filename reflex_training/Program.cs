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
        public static LogLevel LogLevel;
        static FileLogger fl;
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            fl = new FileLogger();
            LogLevel = LogLevel.Info;
            game = new Game();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            gf = new GameForm();
            Thread GameThread = new Thread(new ParameterizedThreadStart(game.GameLoop));
            GameThread.IsBackground = true;
            GameThread.Start(gf);
            game.Start();
            Application.Run(gf);
        }

        [Conditional("DEBUG")]
        public static void Debug(LogLevel level, string format, params object[] args)
        {
            if (level >= LogLevel)
            {
                Console.WriteLine(format, args);
                Task.Run(() => fl.WriteData("log.txt", format, args));
            }
        }
    }
}
