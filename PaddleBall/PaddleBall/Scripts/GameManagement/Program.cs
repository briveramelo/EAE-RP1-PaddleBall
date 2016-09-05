﻿using System;
using System.Diagnostics;

namespace PaddleBall {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = GameManager.Instance) {
                game.Run();
            }
        }
    }
#endif
}
