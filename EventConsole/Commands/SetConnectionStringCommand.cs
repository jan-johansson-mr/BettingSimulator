
namespace EventConsole.Commands
{
        using System;
        using EventConsole.Model;

        public partial class SetConnectionStringCommand : CommandBase
        {
                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command).ToLowerInvariant();
                        if (p0.Equals("sc") || p0.Equals("set-connection-string")) {
                                using (var q0 = new BettingContext(command))
                                        q0.Database.EnsureCreated();
                                Program._connectionString = command;
                                return true;
                        }

                        return false;
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine("'sc' or 'set-connection-string': Set database connection string");

                        WriteLine("Usage: 'sc' <connection string> -or-");
                        WriteLine("Usage: 'set-connection-string' <connection string>");
                }
        }
}
