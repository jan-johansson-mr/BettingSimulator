
namespace EventConsole.Commands
{
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using EventConsole.Model.Entity;

        public partial class GenerateRunnersCommand : CommandBase
        {
                private const int _max_runners = 1000;

                private static string[] _first = {
                        "Walker",
                        "Richelle",
                        "Andera",
                        "Trinidad",
                        "Zaida",
                        "Kenny",
                        "Waylon",
                        "Nida",
                        "Annita",
                        "Odessa",
                        "Theressa",
                        "Dakota",
                        "Blair",
                        "Kelle",
                        "Sharyn",
                        "Doretta",
                        "Shin",
                        "Carolee",
                        "Stepanie",
                        "Wendie",
                        "Analisa",
                        "Nickole",
                        "Giuseppina",
                        "Elisha",
                        "Chanelle",
                        "King",
                        "Luvenia",
                        "Danyelle",
                        "Lucien",
                        "Shay",
                        "Simone",
                        "Lavone",
                        "Sari",
                        "Maxima",
                        "Tamika",
                        "Maynard",
                        "Samara",
                        "Aileen",
                        "Ronny",
                        "Shawanda",
                        "Lucia",
                        "Loida",
                        "Elanor",
                        "Martine",
                        "Sabrina",
                        "Monique",
                        "Stella",
                        "Divina",
                        "Lyman",
                };

                private static string[] _second = {
                        "Marni",
                        "Bridgette",
                        "Lia",
                        "Ricarda",
                        "Rogelio",
                        "Erna",
                        "Elana",
                        "Marquitta",
                        "Alexia",
                        "Orpha",
                        "Lorenza",
                        "Maisha",
                        "Melia",
                        "Inell",
                        "Stefanie",
                        "Karlene",
                        "Candi",
                        "Kenton",
                        "Caridad",
                        "Aurelia",
                        "Elke",
                        "Delisa",
                        "Jewell",
                        "Rema",
                        "Elden",
                        "Sun",
                        "Tamra",
                        "Elenor",
                        "Jamison",
                        "Talitha",
                        "Olimpia",
                        "Waltraud",
                        "Wilford",
                        "Lashonda",
                        "Krista",
                        "Francisca",
                        "Adria",
                        "Emile",
                        "Eufemia",
                        "Albertine",
                        "Mariann",
                        "Kaitlin",
                        "Adam",
                        "Librada",
                        "Kelvin",
                        "Melda",
                        "Annabelle",
                        "Bella",
                        "Madlyn",
                        "Dorotha",
                };

                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command);
                        if (p0.Equals("gr") || p0.Equals("generate-runners")) {
                                _generate_runners(command);
                                return true;
                        }

                        return false;
                }

                private void _generate_runners(string command)
                {
                        if (!base.DoCommand(command))
                                return;

                        if (string.IsNullOrWhiteSpace(command) || !int.TryParse(command, out var count)) {
                                DoInstructions($"Invalid number format: '{command}'");
                                return;
                        }

                        count = Math.Min(1000, Math.Abs(count));

                        var p0 = new Random((int) DateTime.Now.Ticks);

                        Program.GetUpdateContext(context => {

                                var q0 = new List<Runner>();
                                var n0 = new HashSet<String>();

                                while (n0.Count < count)
                                        n0.Add($"{_first[p0.Next(_first.Length)]} {_second[p0.Next(_second.Length)]}");

                                foreach(var name in n0)
                                        q0.Add(new Runner {
                                                Id = Guid.NewGuid(),
                                                Name = name,
                                        });

                                var q1 = context.RunnerSet.ToArray();
                                var q2 = q0.Except(q0.Where(u => q1.Select(v => v.Name).Contains(u.Name))).Take(1000 - q1.Length).ToArray();

                                if (q2.Length == 0)
                                        return;

                                context.RunnerSet.AddRange(q2);
                        });
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine($"'gr' or 'generate-runners': Generate a number of runners (best effort), and store in datastore (max {_max_runners} runners will be stored)");

                        WriteLine("Usage: 'gr' <number of runners> -or-");
                        WriteLine("Usage: 'generate-runners' <number of runners>");
                }
        }
}
