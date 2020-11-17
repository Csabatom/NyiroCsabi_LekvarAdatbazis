using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyírőCsabi_LekvárAdatbázis
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new SQLiteConnection("Data Source= mydb.db");
            connection.Open();

            var createCommand = connection.CreateCommand();

            createCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS lekvar(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                meret INTEGER(10) NOT NULL,
                tipus VARCHAR(10) NOT NULL);";
            createCommand.ExecuteNonQuery();

            while(true)
            {
                Console.Clear();
                double? meret = null;
                String tipus = null;
                if (meret == null)
                {
                    Console.Write("Adja meg a lekvár méretét (Liter): ");
                    try
                    {
                        meret = double.Parse(Console.ReadLine());
                    }
                    catch (Exception e) {
                        Console.Clear();
                    }
                }
                else if (tipus == null)
                {
                    Console.WriteLine();
                    Console.Write("Adja meg a lekvár tipusát\n1. Szilva\n2. Barack\n3. Vegyes\n4. Hagyma\n\nVálasztott sorszám: ");
                    int valasztottElem = int.Parse(Console.ReadLine());
                    switch (valasztottElem)
                    {
                        case 1:
                            tipus = "Szilva";
                            break;
                        case 2:
                            tipus = "Barack";
                            break;
                        case 3:
                            tipus = "Vegyes";
                            break;
                        case 4:
                            tipus = "Hagyma";
                            break;
                        default:
                            tipus = null;
                            break;
                    }
                }
                if (meret != null && tipus != null)
                {
                    var insertCommand = connection.CreateCommand();

                    insertCommand.CommandText = "INSERT INTO lekvar (meret, tipus) VALUES (@meret,@tipus)";

                    insertCommand.Parameters.AddWithValue("@meret", meret);
                    insertCommand.Parameters.AddWithValue("@tipus", tipus);

                    insertCommand.ExecuteNonQuery();
                    Console.Clear();
                }
                // 1. FELADAT
                try
                {
                    var lekvarSumCommand = connection.CreateCommand();
                    lekvarSumCommand.CommandText = @"SELECT SUM(meret) FROM lekvar";
                    var feladat1reader = lekvarSumCommand.ExecuteReader();
                    while (feladat1reader.Read())
                    {
                        var sumMeret = feladat1reader.GetDouble(0);
                        Console.WriteLine("Az összes lekvár mennyisége: " + sumMeret + " Liter.");
                    }

                    Console.WriteLine();

                    // 2. FELADAT
                    var lekvarSumFajtankentCommand = connection.CreateCommand();
                    lekvarSumFajtankentCommand.CommandText = @"SELECT tipus, SUM(meret) FROM lekvar GROUP BY tipus";
                    var feladat2reader = lekvarSumFajtankentCommand.ExecuteReader();
                    while (feladat2reader.Read())
                    {
                        var tipusEredmeny = feladat2reader.GetString(0);
                        var sumMeretTipusonkent = feladat2reader.GetDouble(1);
                        Console.WriteLine(tipusEredmeny + ": " + sumMeretTipusonkent + " Liter.");
                    }

                    Console.WriteLine();

                    // 3. FELADAT
                    var atlagosMeretCommand = connection.CreateCommand();
                    atlagosMeretCommand.CommandText = @"SELECT AVG(meret) FROM lekvar";
                    var feladat3reader = atlagosMeretCommand.ExecuteReader();
                    while (feladat3reader.Read())
                    {
                        var atlagosMeretEredmeny = feladat3reader.GetDouble(0);
                        Console.WriteLine("Lekvárok átlagos mérete: " + atlagosMeretEredmeny + " Liter.");
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("Nincs adat, vigyen be adatot!");
                }

                Console.ReadKey();
            }
        }
    }
}
