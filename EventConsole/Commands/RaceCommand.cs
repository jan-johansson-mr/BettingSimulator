
namespace EventConsole.Commands
{
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;
        using System.Net.Sockets;
        using System.Text;
        using System.Xml;
        using System.Xml.Serialization;
        using EventConsole.Model.Entity;
        using MessageContracts.RaceNotification;
        using Microsoft.EntityFrameworkCore;

        public partial class RaceCommand : CommandBase
        {
                private const int _maxBet = 100;
                private const int _minBet = 10;
                private const int _maxWagers = 200;
                private const int _minWagers = 10;
                private const int _maxRunners = 8;
                private const int _minRunners = 4;
                private readonly XmlSerializerNamespaces _namespaces
                        = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                                new XmlQualifiedName(string.Empty, "http://tempuri.org/EvaluateRace"),
                        });

                public override bool DoCommand(string command)
                {
                        var p0 = Program.GetHead(ref command);
                        if (p0.Equals("r") || p0.Equals("race")) {

                                if (_race(command, out var id))
                                        _notify_race(id);

                                return true;
                        }

                        return false;
                }

                private bool _race(string command, out Guid id)
                {
                        if (!base.DoCommand(command))
                                return false;

                        id = Program.GetUpdateContext(context => {

                                var r0 = new Random((int) DateTime.Now.Ticks);
                                var r1 = PickStakeholders(r0, _minWagers, _maxWagers, context.WagerSet.ToArray());
                                var r2 = PickStakeholders(r0, _minRunners, _maxRunners, context.RunnerSet.ToArray());

                                var p0 = new Pool {
                                        Id = Guid.NewGuid(),
                                };

                                var p1 = r2.Select(u => new Event {
                                        PoolId = p0.Id,
                                        RunnerId = u.Id,
                                });

                                var p2 = r1.Select(u => new Wage {
                                        RunnerId = r2[r0.Next(r2.Length)].Id,
                                        PoolId = p0.Id,
                                        Money = r0.Next(_minBet, _maxBet),
                                        WagerId = u.Id,
                                });

                                context.EventSet.AddRange(p1);
                                context.WageSet.AddRange(p2);
                                context.PoolSet.Add(p0);

                                return p0.Id;
                        });

                        return true;
                }

                private void _notify_race(Guid id)
                {
                        var p0 = Program.GetReadOnlyContext(context =>
                                context.EventSet
                                        .Include(u => u.Pool)
                                        .Include(u => u.Runner)
                                        .Include(u => u.Wages).ThenInclude(u => u.Wager)
                                        .Where(u => u.PoolId == id).ToArray());

                        var p1 = new EvaluateRaceType {
                                pool = new Uri($"urn:pool:{id.ToString()}").AbsoluteUri,
                                timestamp = DateTime.Now,
                                total_amount = p0.SelectMany(u0 => u0.Wages.Select(u1 => u1.Money)).Aggregate((u0, v0) => u0 + v0),
                                total_runners = p0.Length,
                                total_wagers = p0.SelectMany(u0 => u0.Wages.Select(u1 => u1.Wager)).Count(),
                                Wages = p0.Select(u0 => new WagesType {
                                        Runner = new RunnerType {
                                                name = u0.Runner.Name,
                                        },
                                        total_wage = u0.Wages.Select(u1 => u1.Money).Aggregate((u1, v1) => u1 + v1),
                                        Wager = u0.Wages.SelectMany(u1 => u1.Wager.Wages.Select(u2 => new WagerType {
                                                name = u2.Wager.Name,
                                                wage = u2.Money,
                                        })).ToArray(),
                                }).ToArray(),
                        };

                        var p2 = _create_message(p1);

                        using (var p3 = new TcpClient(Program._host, Program._port)) {

                                var l0 = p3.Client.Send(p2);

                                while (l0 < p2.Length)
                                        l0 += p3.Client.Send(p2, l0, p2.Length - l0, SocketFlags.None);

                                p3.Client.Shutdown(SocketShutdown.Send);
                                p3.Close();

                        }
                }

                private byte[] _create_message<T>(T entity)
                        where T : class
                {
                        var p0 = new XmlSerializer(typeof(T));

                        using (var m0 = new MemoryStream()) {

                                var p1 = XmlWriter.Create(m0, new XmlWriterSettings {
                                        CloseOutput = false,
                                        ConformanceLevel = ConformanceLevel.Auto,
                                        Encoding = Encoding.UTF8,
                                        Indent = false,
                                        NamespaceHandling = NamespaceHandling.OmitDuplicates,
                                        NewLineHandling = NewLineHandling.None,
                                        NewLineOnAttributes = false,
                                        OmitXmlDeclaration = true,
                                });

                                p0.Serialize(m0, entity, _namespaces);

                                return m0.ToArray();
                        }
                }

                private static T[] PickStakeholders<T>(Random random, int minStakeholders, int maxStakeholders, T[] stakeholders)
                        where T : class
                {
                        var p0 = random.Next(minStakeholders, maxStakeholders);

                        if (stakeholders.Length > p0) {

                                var idxs = new HashSet<int>();

                                while (idxs.Count < p0)
                                        idxs.Add(random.Next(stakeholders.Length));

                                var selection = new List<T>();

                                foreach (var idx in idxs)
                                        selection.Add(stakeholders[idx]);

                                stakeholders = selection.ToArray();

                        }

                        return stakeholders;
                }

                public override void DoInstructions(string header = null)
                {
                        base.DoInstructions(header);

                        Console.WriteLine($"'r' or 'race': Performe a race, with runners and wages");

                        WriteLine("Usage: 'r' -or-");
                        WriteLine("Usage: 'race'");
                }
        }
}
