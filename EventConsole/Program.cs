namespace EventConsole
{
        using System;
        using System.Collections.Generic;
        using EventConsole.Commands;
        using EventConsole.Model;
        using Microsoft.EntityFrameworkCore;

        class Program
        {
                static internal int _port = 48801;
                static internal string _host = "localhost";
                static internal string _connectionString;
                static readonly List<ICommandUnit> _commands = new List<ICommandUnit>();

                static void Main(string[] args)
                {
                        _commands.AddRange(new List<ICommandUnit> {
                                new IdleCommand(),
                                new HelpCommand(),
                                new SetConnectionStringCommand(),
                                new GenerateRunnersCommand(),
                                new GenerateWagersCommand(),
                                new EnumerateRunnersCommand(),
                                new EnumerateWagersCommand(),
                                new RaceCommand(),

                                // Uknown command
                                new UnknownCommand(),
                        });


                        Console.WriteLine("Welcome to betting simulation application");
                        Console.WriteLine("Enter 'h' or 'help' for help");
                        CheckConnectionString();
                        Console.WriteLine("Enter 'ctrl-z' or 'ctrl-c' to quit");

                        var p0 = (string) null;

                        Prompt();

                        while ((p0 = Console.ReadLine()?.TrimStart()) != null) {

                                try {

                                        foreach (var command in _commands)
                                                if (command.DoCommand(p0))
                                                        break;

                                } catch (Exception ex) {
                                        Console.WriteLine($"{ex.GetType().FullName}: {ex.Message}");
                                }

                                Prompt();
                        }
                }

                static void Prompt()
                {
                        Console.Write($"{DateTime.Now.ToString("s")} # ");
                }

                static internal bool CheckConnectionString()
                {
                        if (string.IsNullOrWhiteSpace(_connectionString)) {
                                Console.WriteLine("No connection string: use 'sc' or 'set-connection-string' to set database connection string");
                                return false;
                        }

                        return true;
                }

                static internal string GetHead(ref string command)
                {
                        var head = command.Split()[0];
                        command = command.Substring(head.Length).TrimStart();
                        return head;
                }

                static internal void DoInstructions()
                {
                        foreach (var command in _commands)
                                command.DoInstructions(null);

                        Console.WriteLine("Enter 'ctrl-z' or 'ctrl-c' to quit");
                }

                static internal void GetUpdateContext(Action<BettingContext> action)
                {
                        using (var p0 = new BettingContext(_connectionString))

                                try {
                                        p0.Database.OpenConnection();
                                        action.Invoke(p0);
                                        p0.SaveChanges();
                                } finally {
                                        p0.Database.CloseConnection();
                                }
                }

                static internal T GetUpdateContext<T>(Func<BettingContext, T> func)
                {
                        using (var p0 = new BettingContext(_connectionString))

                                try {
                                        p0.Database.OpenConnection();
                                        var q0 = func.Invoke(p0);
                                        p0.SaveChanges();
                                        return q0;
                                } finally {
                                        p0.Database.CloseConnection();
                                }
                }

                static internal void GetReadOnlyContext(Action<BettingContext> action)
                {
                        using (var p0 = new BettingContext(_connectionString))

                                try {
                                        p0.ChangeTracker.AutoDetectChangesEnabled = false;
                                        p0.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                        p0.Database.OpenConnection();
                                        action.Invoke(p0);
                                } finally {
                                        p0.Database.CloseConnection();
                                }
                }

                static internal T GetReadOnlyContext<T>(Func<BettingContext, T> func)
                {
                        using (var p0 = new BettingContext(_connectionString))

                                try {
                                        p0.ChangeTracker.AutoDetectChangesEnabled = false;
                                        p0.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                        p0.Database.OpenConnection();
                                        return func.Invoke(p0);
                                } finally {
                                        p0.Database.CloseConnection();
                                }
                }
        }
}
