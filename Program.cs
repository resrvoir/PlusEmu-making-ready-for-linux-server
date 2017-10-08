﻿using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using ConsoleWriter;
using Quasar.Core;
using log4net;
using log4net.Config;
using Quasar.Utilities;

namespace Quasar
{
    public class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        private static readonly ILog log = LogManager.GetLogger("Quasar.Program");

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        public static void Main(string[] Args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            XmlConfigurator.Configure();

            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            QuasarEnvironment.Initialize();

            while (true)
            {
                Console.CursorVisible = true;
                if (Logging.DisabledState)
                    Console.Write("Emulator »");

                ConsoleCommandHandler.InvokeCommand(Console.ReadLine());
                continue;
            }
        }


        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            var e = (Exception) args.ExceptionObject;
            Logging.LogCriticalException("Kritieke systeembestanden fout: " + e);
            QuasarEnvironment.PerformShutDown();
        }

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);
    }
}