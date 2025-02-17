using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {

        List<Hallgato> hallgatok = Beolvasas("course.txt");


        Console.WriteLine($"Hallgatók száma: {hallgatok.Count}");


        decimal backendAtlag = hallgatok.Average(h => h.Backend);
        Console.WriteLine($"Backend átlag: {backendAtlag:F2}%");

        
        Hallgato osztalyElso = hallgatok.OrderByDescending(h => h.Frontend + h.Backend).First();
        Console.WriteLine($"Osztályelső: {osztalyElso.Nev}");

        decimal ferfiakAranya = (decimal)hallgatok.Count(h => h.Nem == "Férfi") / hallgatok.Count * 100;
        Console.WriteLine($"Férfiak aránya: {ferfiakAranya:F2}%");


    
        var teljesenElőfinanszirozottak = hallgatok.Where(h => h.Előfinanszírozás >= 2600).Select(h => h.Nev);
        Console.WriteLine($"Teljesen előfinanszírozott hallgatók: {string.Join(", ", teljesenElőfinanszirozottak)}");

        
        Console.Write("Kérem, adjon meg egy hallgató nevét: ");
        string hallgatoNev = Console.ReadLine();
        Hallgato keresettHallgato = hallgatok.FirstOrDefault(h => h.Nev == hallgatoNev);
        if (keresettHallgato != null)
        {
            var javitoVizsgak = new List<string>();
            if (keresettHallgato.Frontend < 51) javitoVizsgak.Add("Frontend");
            if (keresettHallgato.Backend < 51) javitoVizsgak.Add("Backend");
            if (javitoVizsgak.Count > 0)
                Console.WriteLine($"A {keresettHallgato.Nev} hallgatónak {string.Join(" és ", javitoVizsgak)} vizsgából kell javítóvizsgát tennie.");
            else
                Console.WriteLine($"A {keresettHallgato.Nev} hallgatónak nincs szüksége javítóvizsgára.");
        }
        else
            Console.WriteLine("Nem található ilyen hallgató.");

       
        var teljesitettek = hallgatok.Count(h => (h.Frontend == 100 || h.Backend == 100) && h.Frontend >= 51 && h.Backend >= 51);
        Console.WriteLine($"Teljesítettek száma: {teljesitettek}");

        int frontendJavito = hallgatok.Count(h => h.Frontend < 51);
        int backendJavito = hallgatok.Count(h => h.Backend < 51);
        Console.WriteLine($"Frontend: {frontendJavito} hallgatónak kell javítóvizsgát tennie.");
        Console.WriteLine($"Backend: {backendJavito} hallgatónak kell javítóvizsgát tennie.");

       
        var rendezettHallgatok = hallgatok.OrderBy(h => h.Nev.Split(' ')[1]).Select(h => new { Nev = h.Nev, Atlag = (h.Frontend + h.Backend) / 2 });
        using (StreamWriter sw = new StreamWriter("rendezett_hallgatok.txt"))
        {
            foreach (var hallgato in rendezettHallgatok)
                sw.WriteLine($"{hallgato.Nev}: {hallgato.Atlag:F2}%");
        }
    }

    static List<Hallgato> Beolvasas(string course)
    {
        List<Hallgato> hallgatok = new List<Hallgato>();
        using (StreamReader sr = new StreamReader("course.txt"))
        {
            string sor;
            while ((sor = sr.ReadLine()) != null)
            {
                string[] adatok = sor.Split(';');
                Hallgato hallgato = new Hallgato
                {
                    Nev = adatok[0],
                    Nem = adatok[1],
                    Frontend = decimal.Parse(adatok[2]),
                    Backend = decimal.Parse(adatok[3]),
                    Előfinanszírozás = decimal.Parse(adatok[4])
                };
                hallgatok.Add(hallgato);
            }
        }
        return hallgatok;
    }
}