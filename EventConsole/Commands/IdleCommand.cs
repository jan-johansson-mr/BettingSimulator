
namespace EventConsole.Commands
{
        public partial class IdleCommand : ICommandUnit
        {
                public bool DoCommand(string command)
                {
                        if (string.IsNullOrWhiteSpace(command))
                                return true;

                        return false;
                }

                public void DoInstructions(string header)
                { }
        }
}
