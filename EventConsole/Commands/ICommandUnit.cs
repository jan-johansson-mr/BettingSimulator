
namespace EventConsole.Commands
{
        using System;
        using System.Collections.Generic;
        using System.Text;

        public partial interface ICommandUnit
        {
                bool DoCommand(string command);
                void DoInstructions(string header);
        }
}
