
namespace EventConsole.Commands
{
        using System;

        public partial class HelpCommand : CommandBase
        {
                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command).ToLowerInvariant();
                        if (p0.Equals("h") | p0.Equals("help")) {
                                Program.DoInstructions();
                                return true;
                        }

                        return false;
                }

                public override void DoInstructions(string header)
                {
                        base.DoInstructions(header);

                        Console.WriteLine("'h' or 'help': Write out instructions");

                        WriteLine("Usage: 'h' -or-");
                        WriteLine("Usage: 'help'");
                }
        }
}
