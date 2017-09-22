
namespace EventConsole.Commands
{
        using System;
        using System.Linq;
        using Microsoft.EntityFrameworkCore;

        public partial class EnumerateWagersCommand : CommandBase
        {
                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command);
                        if (p0.Equals("ew") || p0.Equals("enumerate-wagers")) {
                                _enumerate_wagers(command);
                                return true;
                        }

                        return false;
                }

                private void _enumerate_wagers(string command)
                {
                        if (!base.DoCommand(command))
                                return;

                        Program.GetReadOnlyContext(context => {

                                var p0 = context.WagerSet.Include(u => u.Wages).ToList();
                                var p1 = p0.GetEnumerator();

                                if (!p1.MoveNext())
                                        goto l_00;

                                var q0 = p1.Current;

                                Console.WriteLine($"Entity Id: {q0.Id.ToString()}");
                                Console.WriteLine($"Name     : {q0.Name}");
                                Console.WriteLine($"Wages    : {q0.Wages.Count}");

                                while (p1.MoveNext()) {

                                        q0 = p1.Current;

                                        Console.WriteLine(new string('-', 20));
                                        Console.WriteLine($"Entity Id: {q0.Id.ToString()}");
                                        Console.WriteLine($"Name     : {q0.Name}");
                                        Console.WriteLine($"Wages    : {q0.Wages.Count}");

                                }

                                l_00:

                                Console.WriteLine($"Total number of wagers in datastore: {p0.Count}");

                        });
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine("'ew' or 'enumerate-wagers': Enumerate wagers in datastore");

                        WriteLine("Usage: 'ew' -or-");
                        WriteLine("Usage: 'enumerate-wagers'");
                }
        }
}
