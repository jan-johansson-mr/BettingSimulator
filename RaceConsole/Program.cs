namespace RaceConsole
{
        using System;
        using System.IO;
        using System.Linq;
        using System.Net;
        using System.Net.Sockets;
        using System.Security.Cryptography;
        using System.Security.Cryptography.X509Certificates;
        using System.Text;
        using System.Xml;
        using System.Xml.Linq;
        using System.Xml.Serialization;
        using MessageContracts.RaceManifest;
        using MessageContracts.RaceNotification;

        class Program
        {
                static private int _port = 48801;
                static private string _certificate = "MIIKaQIBAzCCCiUGCSqGSIb3DQEHAaCCChYEggoSMIIKDjCCBicGCSqGSIb3DQEHAaC" +
                        "CBhgEggYUMIIGEDCCBgwGCyqGSIb3DQEMCgECoIIE/jCCBPowHAYKKoZIhvcNAQwBAzAOBAgq1oCBFvXWqgICB9AEggTYxP8pw" +
                        "d8fhP36C7BQRnsuzNos1TcbBPRv/6bV6aI3vuS4GAypt0tdKPI5crOgNslfAApJaRP3mPxHr8R7351Au4J/hGM6wxTD4w9oUWL" +
                        "i+L9iNNGrjzVZ4v0nHRPuoMAkE4vjtkpT7ZzznY1CMJqvd9i4ewx0d+v5dkSJKUCuVnS43LrTk0LK3VP4Czh8ou+7jvTyAPdS1" +
                        "qAi8X+3mVm6ZR/xqOE/ojBiPvEJC6IY3xBxNd0wxGkkD1XoybEq00Qyu/L1yhfB2h8motrDTKBF8hfCRf9BPDVjGAm7o550m8f" +
                        "DhLzWDpyUDmYRZVCIe0Yi66Me39dEmLfAh7wd18TYR8ktcj/XbuF4GJvg4OmxrtedL650uMbXhgG0huaa13CShnu/ALpe30uf9" +
                        "CWooMk4+lBt9Sa8QB8+XzO9uhxouIgQm9H0quH2jiceHU5zZGIougpdxzSgRcV4sClhPucBL16exFOiqE1UuaQyH6hiF5n6bu4" +
                        "CT1uYNJYbLIY8j7zFSFdcVi3nFTXMcCPbvUwLPjuGwJeOq62u9d/ALy7TO/PahlYrH3Z3PFiJFF6UJTOqFzAnvHG10NzIyKcBM" +
                        "j9pUJjdXe8bAFS8FE06QhKxZrP/kOmC7sHxqOSz1pF9Q/+5mERvZTTPinvCkmkdPD39eg/rCjA1uj2V0062IXdiSphj1+wTzqD" +
                        "GdACmLTm2Kc0KULg24kJHOkD9kVIN+KyFkTUsar1c7Uxa+VlbkBE6a09rP7d61gfqmqDOwDkdvkcu2s4ccHOKL0Ufpq7j4FeY6" +
                        "8GZsbk24JyIBZIGz+wuaPlReerid3na1Qyre4inz3Y1YOragrnemam981YcJRtZYT0pNLG35KMwzqCD9lV1cwUFWGM6JkxeaVq" +
                        "Ke15ZcYSTRxZiFqHN+BOnbDA733b/OkRV3nmPgWwXQkXoTV3bQbO2hxiXaBfDUxiHCdcha9H6oUZTlfW67SoDEbjLeZyf6JN8/" +
                        "LLPzNwd3vIsoqcOklZLB7U3pH4nWRcqdoRSPN9uuBKK6lDPlwvYmxA/eQreRI28L8Tt4e3J7PQUlyNDy8mFfj+1TETKKKUkRkm" +
                        "LPHz396zmH1NdNOlmh1b9RrAAb628SE7B2QVw4Q7zDiycJ8tZWV3ys0lBH6pO/q8gBuW54blkLo9GZYP7qTDjJkwsgOOFXLP54" +
                        "7TW5xoTTDj4Y1nxUeptjIcONMzBoOReekCnzZmKnTEk1LngABpzTwnLEzUihsafilOrSBMIL0byNrXQw2blSjvte24iXqENJR1" +
                        "IGJ+ERjrx0nV1lt1prqcqq/uFDUTAd8MsIN9iNP9vsvLBk5rVgVXr3pUVQakYQlb5/QECzvBPWYh9+BYqrguKgaeARkkPUuDdL" +
                        "V54rl+FmUYO+bQ2iZDD3VUSYTMv7N7480cMpEp5fKWvrBFeYfVMMRb2paCbBxDhf6wVp/2tWZZY0M/L2xtq+TBtjULcwyMf2ce" +
                        "NImnw1Pz8sEzv4GQq9nJ8V4aXnI5ZZucQwo9uEK3eBz6riWr+iMbyUynRhNhQquB3NJm8U3L1K8DCmiijNUfHakQegvQC5BwbD" +
                        "wDD9Jl5AtBTHMYPnxPDuPpPo3tz5EG90vl5ebmQhjRXgIwFd/zfMqSVJ6zDk7xibxfjaBq0/IeTbfQOpTGB+jANBgkrBgEEAYI" +
                        "3EQIxADATBgkqhkiG9w0BCRUxBgQEAQAAADBpBgkqhkiG9w0BCRQxXB5aAEkASQBTACAARQB4AHAAcgBlAHMAcwAgAEQAZQB2A" +
                        "GUAbABvAHAAbQBlAG4AdAAgAEMAZQByAHQAaQBmAGkAYwBhAHQAZQAgAEMAbwBuAHQAYQBpAG4AZQByMGkGCSsGAQQBgjcRATF" +
                        "cHloATQBpAGMAcgBvAHMAbwBmAHQAIABSAFMAQQAgAFMAQwBoAGEAbgBuAGUAbAAgAEMAcgB5AHAAdABvAGcAcgBhAHAAaABpA" +
                        "GMAIABQAHIAbwB2AGkAZABlAHIwggPfBgkqhkiG9w0BBwagggPQMIIDzAIBADCCA8UGCSqGSIb3DQEHATAcBgoqhkiG9w0BDAE" +
                        "DMA4ECOq8yhhXJ7eJAgIH0ICCA5jHVrN2hbovkDGyTYtoogs72Wj+GHZeCo9AksMbw1OGGAjAYP9qtNkhp525Peu7+PATajQSH" +
                        "ouVOvkzPXDXeDijI40ZkQ92uyRp6UE3p+IqgkDVe0vI+swMyZ55Y2v5K5FYRbS1dYB/8NgZe/SgkhiAkeaizM5hngj/OGom2s6" +
                        "75VsfVDWcjHtgSsXGoua+t7OwQTyRkJf+lKx87vQyLgDdXYOp+k+dvXbNNXiACuzQVvam2AW9TPyxl+FuV/U0FN0XWPth56Oar" +
                        "wrrq4jCJw3uuJfhotF8m98RvNXSTlNvYVr/MAYlv9+lNMHGy0rwhAQsNvgkvM2JzS9H5IuUAddS6sbWP8O583MXVSNKxG0+DRL" +
                        "Y8EnoZ/ScBcXIQHdtouQr67krrByMJBICRVLfq0uRJRqGK/3dM0DUlhiFIRQs+zNehv3RayksIh0I8hNiIcEzS8OwA/AANLQ1b" +
                        "Y3aTl2uObbuxsX3EcDbYUKPiO55klKAKf2v/D36xJZAdXdL6QSsmyLvs530LYJutBlbwBSfMw4l9P7FbUm0gN9yNMJdTOL+grZ" +
                        "+AgrL8yknCqiwxpGoMYYRwxHwUBY9kFuXH7eewAnYRCIJbFH8hOUHOXPtiE2GWLI7C82KQ+YtHTr/dbxub9iLlbfbQrt84UREp" +
                        "i/AbdRY6ry62mVokBwGtqmxQIxbIBENFVGOejHKK/KldG9vhDWZBTiqxizSlw0BO/4nVlPui7PVnVGEANGaMe4VN59ezDKwS4i" +
                        "PEiy51tBKO+j3giFN25dIUZ1vTkwjSs9bCsvwlRpcSjXTm3GLztfSi1yByTbTyKUSVmrF4Jj5YKAfpkEW6tntWUvR7q6Ava8VT" +
                        "dBsrM6EEeMz+h7svEEVLTlBqF3itGZpS+/uK31mmm6ccLLUIiFe9vHeUwdbpt+3OB+krd9jFGGxobw5/KyBUGAeaYKX/p0BW7U" +
                        "6q7gaFnhJqF/BPD65CrYFna8p/QwlE9l8B+YzuOqlKsDNWS9jJ0BzVS+IrFJkAtMaDNhILWtvlcLnwkdkG+UGCnw999PBox0vK" +
                        "cd1gAdn+NvLFOAltCVg3Vmu0Pw7Ouj4ggIPidy6r1VjurNcPtH+mflUsAaMXmqkLpSULE0WzqL3kt5lQ3pi6zUboUxszCzp2dl" +
                        "7J0QKmeGy8fFULerUJHGBdLG9+pq/Mobfx6lWkbeLxDsXw98Wc2eieA2OwCrI69RsPKznkQsr910yezA7MB8wBwYFKw4DAhoEF" +
                        "KWUSTxxGUn2JCIMRog1OQrBX6hoBBT1xNJcWXJCV+ew35XR9GCAvEeSEwICB9A=";

                private static readonly XmlSerializerNamespaces _namespaces
                        = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                                new XmlQualifiedName(string.Empty, "http://tempuri.org/ResultManifest"),
                                new XmlQualifiedName("ds", "http://www.w3.org/2000/09/xmldsig#"),
                        });

                static byte[] _exported_certificate;

                static void Main(string[] args)
                {
                        Console.WriteLine("Race Control started, hit 'ctrl-c' to quit");

                        _exported_certificate = new X509Certificate2(
                                Convert.FromBase64String(_certificate))
                                .Export(X509ContentType.Cert);

                        var p0 = new TcpListener(IPAddress.Any, _port);

                        p0.Start();

                        while (true) {

                                using (var q0 = p0.AcceptTcpClient()) {
                                        Console.WriteLine("Start race...");
                                        var q1 = _handle_request(q0);
                                        q0.Close();
                                        _handle_race(q1);
                                        Console.WriteLine("Race done");
                                }
                        }
                }

                private static EvaluateRaceType _handle_request(TcpClient tcpClient)
                {
                        var p0 = new Byte[8192];

                        using (var m0 = new MemoryStream()) {

                                var l0 = tcpClient.Client.Receive(p0);
                                m0.Write(p0, 0, l0);

                                while (l0 == p0.Length) {
                                        l0 = tcpClient.Client.Receive(p0);
                                        m0.Write(p0, 0, l0);
                                }

                                m0.Position = 0;

                                return new XmlSerializer(typeof(EvaluateRaceType)).Deserialize(m0) as EvaluateRaceType;
                        }
                }

                private static void _handle_race(EvaluateRaceType race)
                {
                        var p0 = race.Wages.Select(u0 => new {
                                wages = u0,
                                payout_odds = ((double) (race.total_amount)) / ((double) (u0.total_wage)),
                                probability_to_win = ((double) (u0.total_wage)) / ((double) (race.total_amount)),
                        }).ToList();

                        var p1 = 0d;
                        var p2 = ((double) (new Random((int) DateTime.Now.Ticks).Next(100)) / ((double) (100)));
                        var p3 = p0.SkipWhile(u0 => (p1 += u0.probability_to_win) < p2).Take(1).Single();
                        var p4 = Guid.NewGuid();

                        var p5 = new ResultType {
                                Id = new Uri($"urn:result:{p4.ToString()}").AbsoluteUri,
                                Odds = p3.payout_odds,
                                Receipt = p3.wages.Wager.Select(u0 => new ReceiptType {
                                        Amount = (u0.wage * p3.payout_odds),
                                        Name = u0.name,
                                }).ToArray(),
                                Runner = p3.wages.Runner.name,
                        };

                        _save_manifest(p4, _serialize(_sign_manifest(p4, p5)));
                }

                private static byte[] _serialize<T>(T that)
                {
                        var p0 = new XmlSerializer(typeof(T));

                        using (var m0 = new MemoryStream()) {

                                var w0 = XmlWriter.Create(m0, new XmlWriterSettings {
                                        CloseOutput = false,
                                        ConformanceLevel = ConformanceLevel.Auto,
                                        Encoding = new UTF8Encoding(false),
                                        Indent = false,
                                        NamespaceHandling = NamespaceHandling.OmitDuplicates,
                                        NewLineHandling = NewLineHandling.None,
                                        NewLineOnAttributes = false,
                                        OmitXmlDeclaration = false,
                                });

                                p0.Serialize(w0, that, _namespaces);

                                return m0.ToArray();
                        }
                }

                private static byte[] _compute_digest(byte[] context)
                {
                        using (var p0 = SHA512.Create())
                                return p0.ComputeHash(context);
                }

                private static byte[] _compute_signature(byte[] context)
                {
                        using (var p0 = new X509Certificate2(Convert.FromBase64String(_certificate)))
                                return p0.GetRSAPrivateKey().SignData(context, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                }

                private static void _save_manifest(Guid id, byte[] manifest)
                {
                        var path = $"Manifest - {id.ToString("D")} - {DateTime.Now.ToLongDateString()}.xml";
                        using (var f0 = File.CreateText(path))
                                f0.Write(XDocument.Parse(Encoding.UTF8.GetString(manifest)).ToString());
                }

                private static ResultManifestType _sign_manifest(Guid reference, ResultType result)
                {
                        var q0 = new SignedInfoType {
                                CanonicalizationMethod = new CanonicalizationMethodType {
                                        Algorithm = new Uri("urn:morph:demo").AbsoluteUri,
                                },
                                Reference = new ReferenceType[] {
                                                new ReferenceType {
                                                        DigestMethod = new DigestMethodType {
                                                                Algorithm = new Uri("http://www.w3.org/2000/09/xmldsig#sha512").AbsoluteUri,
                                                        },
                                                        DigestValue = _compute_digest(_serialize(result)),
                                                        URI = new Uri($"urn:result:{reference.ToString()}").AbsoluteUri,
                                                },
                                        },
                                SignatureMethod = new SignatureMethodType {
                                        Algorithm = new Uri("http://www.w3.org/2000/09/xmldsig#rsa-sha512").AbsoluteUri,
                                },
                        };

                        return new ResultManifestType {
                                Result = result,
                                Signature = new SignatureType {
                                        KeyInfo = new KeyInfoType {
                                                Items = new object[] {
                                                new X509DataType {
                                                        Items = new object[] {
                                                                _exported_certificate,
                                                        },
                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                ItemsChoiceType.X509Certificate,
                                                        },
                                                },
                                        },
                                                ItemsElementName = new ItemsChoiceType2[] {
                                                ItemsChoiceType2.X509Data,
                                        },
                                        },
                                        SignatureValue = new SignatureValueType {
                                                Value = _compute_signature(_serialize(q0)),
                                        },
                                        SignedInfo = q0,
                                },
                        };
                }
        }
}
