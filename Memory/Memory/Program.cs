using System;

namespace Memory
{
    class Program
    {
        static char[] symbols = { '!', '&', '%', '*', '+', '-', '/', '$', '£', '€', '°', '§', '-', ':', ';', '|', '(', ')' };
        static int gridsize = 4;
        static int X1 = 0;
        static int Y1 = 0;
        static int X2 = 0;
        static int Y2 = 0;
        static char[,] viewedgrid = new char[gridsize, gridsize];
        static char[,] actualgrid = new char[gridsize, gridsize];

        static void Main(string[] args)
        {


            Console.Clear();
            ZeigeAnleitung();
            FuelleAnzeige(viewedgrid);
            Anzeigen(viewedgrid);
            FuelleMemory(actualgrid);


            while (!IstSpielFertig(viewedgrid))
            {
                Eingabe();
                Console.Clear();
                ZeigeAnleitung();

                DeckeAuf(viewedgrid, actualgrid, X1, Y1, X2, Y2);
                Anzeigen(viewedgrid);
                VergleicheSymbole(viewedgrid, actualgrid, X1, Y1, X2, Y2);
                DeckeZu(viewedgrid, actualgrid, X1, Y1, X2, Y2);
            }
            Console.WriteLine("Du hast es geschafft!");


        }
        static void ZeigeAnleitung()
        {
            Console.WriteLine("Hinter dem '?' verstecken sich Symbole, die paarweise vorkommen. Finden Sie diese!");
            Console.WriteLine("Wählen Sie zwei Positionen zum Aufdecken in der Form: Z1S1Z2S2");
            Console.WriteLine("Bsp. 2142 vergleicht das Symbol Zeile 2 Spalte 1 mit Zeile 4 Spalte 2");
        }
        static void Eingabe()
        {
            bool isokey = false;
            while (!isokey)
            {
                char[] eingabe = Console.ReadLine().ToCharArray();
                if (EingabePruefung(eingabe))
                {
                    X1 = eingabe[0] - 49;
                    Y1 = eingabe[1] - 49;
                    X2 = eingabe[2] - 49;
                    Y2 = eingabe[3] - 49;
                    isokey = true;
                }
            }
        }
        static bool EingabePruefung(char[] eingabe)
        {
            if (eingabe[0] == 'f' && eingabe[1] == 'u' && eingabe[2] == 'c' && eingabe[3] == 'k') { Anzeigen(actualgrid); return false; }
            //Die Eingabe muss 4 Zeichen lang sein
            if (eingabe.Length != 4) { return false; }

            //es müssen zwei verschiedene Positionen eingegeben werden
            if (eingabe[0] == eingabe[2] && eingabe[1] == eingabe[3]) return false;

            //Die Eingabe muss zwischen 1 und 'Grösse', also der Anzahl der Zeilen / Spalten sein
            if (eingabe[0] - 49 < 0 || eingabe[0] - 48 > gridsize) return false;
            if (eingabe[1] - 49 < 0 || eingabe[1] - 48 > gridsize) return false;
            if (eingabe[2] - 49 < 0 || eingabe[2] - 48 > gridsize) return false;
            if (eingabe[3] - 49 < 0 || eingabe[3] - 48 > gridsize) return false;

            return true;
        }
        static void FuelleAnzeige(char[,] ar)
        {
            // setzt an jede Stelle des Arrays ein '?'
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    ar[i, j] = '?';
                }
            }
        }
        static void Anzeigen(char[,] ar)
        {
            Console.WriteLine("  1 2 3 4");
            for (int i = 0; i < gridsize; i++)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3} {4}", i + 1, ar[i, 0], ar[i, 1], ar[i, 2], ar[i, 3]));
            }
        }
        static void FuelleMemory(char[,] ar)
        {
            //Bestimme die Anzahl der Elemnte eines Hilfsarrays: Anzahl der Verschiedenen vorkommenden Symbole
            int AnzahlElemente = gridsize * 2;
            //Hilfsarray
            int[] Hilfsarray = new int[AnzahlElemente];
            //Gib dem Hilfsarray den Startwert 0
            for (int i = 0; i < AnzahlElemente; i++)
            {
                Hilfsarray[i] = 0;
            }
            //Initialisiere den Zufallsgenerator
            Random randomnummber = new Random();

            for (int i = 0; i < gridsize; i++)
            {
                //für j von 0 bis < Anzahl Zeilen, i Inkrementieren
                for (int j = 0; j < gridsize; j++)
                {
                    //Zufallszahl Bestimmen zwischen 0 und Anzahl vorkommender Symbole
                    int random = randomnummber.Next(0, AnzahlElemente);
                    //Hilfsarray[zufallszahl]++
                    Hilfsarray[random]++;
                    //Hilfsarray[Zufallszahl]<3
                    if (Hilfsarray[random] < 3)
                    {
                        ar[i, j] = symbols[random];
                    }
                    else
                    {
                        j--;
                    }
                }
            }

        }
        static void VergleicheSymbole(char[,] ar1, char[,] ar2, int x1, int y1, int x2, int y2)
        {
            if (ar2[x1, y1] == ar2[x2, y2])
            {
                Console.WriteLine("Treffer, Super...");
                ar2[x1, y1] = ' ';
                ar2[x2, y2] = ' ';
            }
            else
            {
                Console.WriteLine("Leider kein Treffer");
            }
        }
        static void DeckeAuf(char[,] ar1, char[,] ar2, int x1, int y1, int x2, int y2)
        {
            ar1[x1, y1] = ar2[x1, y1];
            ar1[x2, y2] = ar2[x2, y2];
        }
        static void DeckeZu(char[,] ar1, char[,] ar2, int x1, int y1, int x2, int y2)
        {
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    if (ar2[i, j] == ' ')
                    {
                        ar1[i, j] = ' ';
                    }
                    else
                    {
                        ar1[i, j] = '?';
                    }

                }
            }
        }
        static bool IstSpielFertig(char[,] ar)
        {
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    if (ar[i, j] == '?')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}