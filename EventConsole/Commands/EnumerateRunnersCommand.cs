
namespace EventConsole.Commands
{
        using System;
        using System.Linq;
        using Microsoft.EntityFrameworkCore;

        public partial class EnumerateRunnersCommand : CommandBase
        {
                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command);
                        if (p0.Equals("er") || p0.Equals("enumerate-runners")) {
                                _enumerate_runners(command);
                                return true;
                        }

                        return false;
                }

                private void _enumerate_runners(string command)
                {
                        if (!base.DoCommand(command))
                                return;

                        Program.GetReadOnlyContext(context => {

                                var p0 = context.RunnerSet.Include(u => u.Events).ToList();
                                var p1 = p0.GetEnumerator();

                                if (!p1.MoveNext())
                                        goto l_00;

                                var q0 = p1.Current;

                                Console.WriteLine($"Entity Id: {q0.Id.ToString()}");
                                Console.WriteLine($"Name     : {q0.Name}");
                                Console.WriteLine($"Events   : {q0.Events.Count}");

                                while (p1.MoveNext()) {

                                        q0 = p1.Current;

                                        Console.WriteLine(new string('-', 20));
                                        Console.WriteLine($"Entity Id: {q0.Id.ToString()}");
                                        Console.WriteLine($"Name     : {q0.Name}");
                                        Console.WriteLine($"Events   : {q0.Events.Count}");

                                }

                                l_00:

                                Console.WriteLine($"Total number of runners in datastore: {p0.Count}");

                        });
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine("'er' or 'enumerate-runners': Enumerate runners in datastore");

                        WriteLine("Usage: 'er' -or-");
                        WriteLine("Usage: 'enumerate-runners'");
                }
        }
}
