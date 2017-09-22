
namespace EventConsole.Commands
{
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using EventConsole.Model.Entity;

        public partial class GenerateWagersCommand : CommandBase
        {
                private const int _max_wagers = 1000;

                private static string[] _first = {
                        "Dino",
                        "Clarine",
                        "Zula",
                        "Marianela",
                        "Frank",
                        "Irene",
                        "Nathan",
                        "Talisha",
                        "Joane",
                        "Eliz",
                        "Sixta",
                        "Florentino",
                        "Raye",
                        "Voncile",
                        "Elvira",
                        "Francina",
                        "Monte",
                        "Ela",
                        "Jeffry",
                        "Les",
                        "Ernie",
                        "Lillie",
                        "Bethel",
                        "Ligia",
                        "Eloisa",
                        "Veronica",
                        "Sharika",
                        "Terina",
                        "Zofia",
                        "Arleen",
                        "Bonita",
                        "Olive",
                        "Quincy",
                        "Ma",
                        "Soraya",
                        "Alec",
                        "Annabel",
                        "Anastacia",
                        "Neida",
                        "Ellis",
                        "Carl",
                        "Tressa",
                        "Felicidad",
                        "Jacklyn",
                        "Henry",
                        "Carlee",
                        "Jadwiga",
                        "Gema",
                        "Berna",
                        "Eleonora",
                };

                private static string[] _second = {
                        "Brandy",
                        "Ermelinda",
                        "Ione",
                        "Gilda",
                        "Jenny",
                        "Jenna",
                        "Lennie",
                        "Zenaida",
                        "Giuseppe",
                        "Rosie",
                        "Katherine",
                        "Lucien",
                        "Anisa",
                        "Classie",
                        "Rowena",
                        "Peggy",
                        "Reagan",
                        "Tammy",
                        "Aliza",
                        "Joan",
                        "Eboni",
                        "Tatum",
                        "Hedwig",
                        "Susy",
                        "Chun",
                        "Delorse",
                        "Karlene",
                        "Donita",
                        "Lola",
                        "Brad",
                        "Jacquiline",
                        "Jeffry",
                        "Vincenza",
                        "Angila",
                        "Azucena",
                        "Regenia",
                        "Tanja",
                        "Carmina",
                        "Lashon",
                        "Dottie",
                        "Lakiesha",
                        "Hayden",
                        "Arvilla",
                        "Cesar",
                        "Veda",
                        "Lauryn",
                        "Lorie",
                        "Ilda",
                        "Waylon",
                        "Lynsey",
                };

                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command);
                        if (p0.Equals("gw") || p0.Equals("generate-wagers")) {
                                _generate_wagers(command);
                                return true;
                        }

                        return false;
                }

                private void _generate_wagers(string command)
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

                                var q0 = new List<Wager>();
                                var n0 = new HashSet<String>();

                                while (n0.Count < count)
                                        n0.Add($"{_first[p0.Next(_first.Length)]} {_second[p0.Next(_second.Length)]}");

                                foreach (var name in n0)
                                        q0.Add(new Wager {
                                                Id = Guid.NewGuid(),
                                                Name = name,
                                        });

                                var q1 = context.WagerSet.ToArray();
                                var q2 = q0.Except(q0.Where(u => q1.Select(v => v.Name).Contains(u.Name))).Take(1000 - q1.Length).ToArray();

                                if (q2.Length == 0)
                                        return;

                                context.WagerSet.AddRange(q2);
                        });
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine($"'gw' or 'generate-wagers': Generate a number of wagers (best effort), and store in datastore (max {_max_wagers} wagers will be stored)");

                        WriteLine("Usage: 'gw' <number of wagers> -or-");
                        WriteLine("Usage: 'generate-wagers' <number of wagers>");
                }
        }
}
