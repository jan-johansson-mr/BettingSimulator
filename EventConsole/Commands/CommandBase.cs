
namespace EventConsole.Commands
{
        using System;

        public partial class CommandBase : ICommandUnit
        {
                public virtual bool DoCommand(string command)
                {
                        return Program.CheckConnectionString();
                }

                public virtual void DoInstructions(string header = null)
                {
                        if (!string.IsNullOrWhiteSpace(header)) {
                                Console.WriteLine($"{new string(' ', 3)}{header}");
                        }
                }

                public void WriteLine(string line)
                {
                        Console.WriteLine($"{new string(' ', 3)}{line}");
                }
        }
}
