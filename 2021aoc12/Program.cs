using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021aoc12
{
    class Program
    {
        class Barlangrendszer
        {
            Dictionary<string, HashSet<string>> barlangjai;
            Dictionary<string, bool> méret;
            IEnumerable<string[]> éllista;
            public Barlangrendszer(string path)
            {
                barlangjai = new Dictionary<string, HashSet<string>>();
                méret = new Dictionary<string, bool>();
                éllista = System.IO.File.ReadAllLines(path).Select(s => s.Split('-'));
                foreach (string[] csúcspár in éllista)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        if (barlangjai.ContainsKey(csúcspár[i]))
                            barlangjai[csúcspár[i]].Add(csúcspár[1 - i]);
                        else
                        {
                            méret[csúcspár[i]] = csúcspár[i] == csúcspár[i].ToLower();
                            barlangjai[csúcspár[i]] = new HashSet<string> { csúcspár[1 - i] };
                        }
                    }
                }
            }
            class Útvonal
            {
                Stack<string> barlangok;
                public bool van_benne_két_kisbarlang { get; private set; }
                public Útvonal(Stack<string> verem, bool volt_e )
                {
                    barlangok = verem;
                    van_benne_két_kisbarlang = volt_e;
                }
                public Útvonal(string start) : this(new Stack<string>(), false) { barlangok.Push(start); Console.WriteLine(this); }
                public Útvonal(Útvonal u) : this(new Stack<string>(new Stack<string>(u.barlangok)), u.van_benne_két_kisbarlang) { }
                public override string ToString() => String.Join(",", barlangok);
                public static Útvonal operator +(Útvonal u, string s)
                {
                    Útvonal r = new Útvonal(u);
                    if (u.Már_járt_ebben_a_kis_barlangban(s))
                    {
                        r.van_benne_két_kisbarlang = true;
                    }
                    r.barlangok.Push(s);
                    return r;
                }
                public string Utolsó_barlangja { get => barlangok.Peek(); }
                public bool Már_járt_ebben_a_kis_barlangban (string barlang) => Kisbetűs(barlang) && barlangok.Contains(barlang);
            }
            List<Útvonal> Útvonalgyűjtés(Func<Útvonal, bool> predikátum)
            {
                List<Útvonal> kész_útvonalak = new List<Útvonal>();
                Stack<Útvonal> tennivalók = new Stack<Útvonal>();
                tennivalók.Push(new Útvonal("end"));

                while (0 < tennivalók.Count)
                {
                    Útvonal útvonal = tennivalók.Pop();
                    foreach (string barlang in barlangjai[útvonal.Utolsó_barlangja])
                    {
                        if (!(barlang == "end" || (predikátum(útvonal) && útvonal.Már_járt_ebben_a_kis_barlangban(barlang))))
                        {
                            if (barlang == "start")
                                kész_útvonalak.Add(útvonal + barlang);
                            else
                                tennivalók.Push(útvonal + barlang);
                        }
                    }
                }
                return kész_útvonalak;
            }
            public void GraphViz() {
                Console.WriteLine("graph g {");
                foreach (string[] pár in éllista ) Console.WriteLine($"\"{pár[0]}\" -- \"{pár[1]}\";");
                Console.WriteLine("}");
            }
            static bool Kisbetűs(string s) => s == s.ToLower();
            public void Útvonalak_kiírása()
            {
                /** /
                List<Útvonal> útvonalak = Útvonalgyűjtés(ú => true );
                /*/
                List<Útvonal> útvonalak = Útvonalgyűjtés(ú => ú.van_benne_két_kisbarlang);
                /**/
                foreach (Útvonal útvonal in útvonalak)
                    Console.WriteLine(útvonal);
                Console.WriteLine($"===> tehát összesen {útvonalak.Count} db");

            }
        }

        static void Main(string[] args)
        {
            /** /
            Barlangrendszer barlangrendszer = new Barlangrendszer("teszt.txt");
            /*/
            Barlangrendszer barlangrendszer = new Barlangrendszer("input.txt");
            /**/

            barlangrendszer.GraphViz();

            Console.WriteLine(" -------------------------------- ");
            barlangrendszer.Útvonalak_kiírása();
            Console.ReadKey();
        }
    }
}
