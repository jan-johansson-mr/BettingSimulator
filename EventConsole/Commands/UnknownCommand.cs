
namespace EventConsole.Commands
{
        using System;

        public partial class UnknownCommand : ICommandUnit
        {
                public bool DoCommand(string command)
                {
                        Console.WriteLine($"Unknown command: '{command}'");
                        Console.WriteLine("Enter 'h' or 'help' for help");
                        return true;
                }

                public void DoInstructions(string header)
                { }
        }
}
