using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Hauptprogramm
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                // Eingabe des Suchordners
                Console.Write("Bitte geben Sie den Pfad zum Ordner mit den Textdateien ein (z. B. C:\\Beispieltext): ");
                string pfadOrdner = Console.ReadLine();

                // Überprüfung, ob Ordner vorhanden ist
                if (!Directory.Exists(pfadOrdner))
                {
                    throw new DirectoryNotFoundException("Der angegebene Pfad wurde nicht gefunden.");
                }

                // Wenn Ordner vorhanden --> Auflistung aller Textdateien in diesem Ordner
                string[] dateien = Directory.GetFiles(pfadOrdner, "*.txt");
                // Wenn keine Textdateien im Ordner --> Fehlermeldung
                if (dateien.Length == 0)
                {
                    throw new FileNotFoundException("Es wurden keine Textdateien im angegebenen Ordner gefunden.");
                }

                // Wenn Textdateien vorhanden sind, Auflistung der Dateien mit ID am Bildschirm
                Console.WriteLine("Verfügbare Textdateien:");
                for (int i = 0; i < dateien.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(dateien[i])}");
                }

                // Auswahl der zu analysierenden Datei mit Nummer
                Console.Write("Bitte wählen Sie eine Datei aus (Geben Sie die Nummer ein): ");
                int dateiID = int.Parse(Console.ReadLine()) - 1;
                // Überprüfung, ob eingegebene Nummer vorhanden ist
                if (dateiID < 0 || dateiID >= dateien.Length)
                {
                    throw new IndexOutOfRangeException("Ungültige Datei-Nummer.");
                }

                // Bestimmung der ausgewählten Datei und einlesen des Inhalts
                string ausgewaehlteDatei = dateien[dateiID];
                string[] text = File.ReadAllLines(ausgewaehlteDatei);

                // Erstellung einen Dictionarys für die Auswertung des Textes
                Dictionary<string, int> einzelneWorte = new Dictionary<string, int>();
                // Bestimmung der Seperatoren für die einzelnen Wörter
                char[] separatoren = { ' ', ',', '.', ';', ':', '(', ')', '[', ']', '{', '}', '<', '>', '\'', 
                    '"', '-', '_', '/', '\\', '|', '*', '&', '^', '%', '$', '#', '@', '!', '?', '=', '+', '~', '`' };

                // Zeilenweise Abarbeitung des Dateiinhalts
                foreach (string zeile in text)
                {
                    // Zeilen mittels Seperatoren in einzelne Wörter aufsplitten
                    string[] woerter = zeile.Split(separatoren, StringSplitOptions.RemoveEmptyEntries);
                    // Behandlung der einzelnen Worte
                    foreach (string wort in woerter)
                    {
                        // Wenn Wort schon vorhanden, dann Zähler um 1 erhöhen
                        if (einzelneWorte.ContainsKey(wort))
                        {
                            einzelneWorte[wort]++;
                        }
                        // Wenn Wort nicht vorhanden, Wort hinzufügen und Zähler auf 1 setzen
                        else
                        {
                            einzelneWorte[wort] = 1;
                        }
                    }
                }

                // Ausgeben der Anzahl der Wörter (Addition der Zähler für die Worte)
                Console.WriteLine($"Anzahl der Wörter im Text: {einzelneWorte.Values.Sum()}");

                // Ausgabe der 10 häufigsten Wörter
                Console.WriteLine("Die 10 häufigsten Wörter:");
                // Sortierung des Dictionarys nach wert im Zähler und Behandlung der ersten 10 Eintrge
                foreach (var eintrag in einzelneWorte.OrderByDescending(eintrag => eintrag.Value).Take(10))
                {
                    Console.WriteLine($"{eintrag.Key}: {eintrag.Value}");
                }

                // Eingabe des Pfades für die Ausgabedatei
                Console.WriteLine("Bitte geben Sie den Speicherort für die Ergebnisdatei an:");
                string ordnerAusgabe = Console.ReadLine();

                //Eingabe des Dateinamens für die Ausgabedatei
                Console.WriteLine("Bitte geben Sie einen Dateinamen für die Ergebnisdatei an:");
                string dateinameAusgabe = Console.ReadLine();

                // Zusammenfügen des Pfades und des Dateinamens in eine Variable (dateiAusgabe)
                string dateiAusgabe = Path.Combine(ordnerAusgabe, dateinameAusgabe);
                // Speichern der Ausgabedatei
                using (StreamWriter dateiSpeichern = new StreamWriter(dateiAusgabe))
                {
                    // Dateiinhalt definieren
                    dateiSpeichern.WriteLine($"Anzahl der Wörter im Text: {einzelneWorte.Values.Sum()}");
                    dateiSpeichern.WriteLine("Die 10 häufigsten Wörter:");
                    foreach (var eintrag in einzelneWorte.OrderByDescending(eintrag => eintrag.Value).Take(10))
                    {
                        dateiSpeichern.WriteLine($"{eintrag.Key}: {eintrag.Value}");
                    }
                }

                // Meldung an den Benutzer, wenn Datei gespeichert wurde
                Console.WriteLine("Ergebnisse wurden in die Datei gespeichert.");

                // Abfrage, ob Benutzer neue Datei analysieren will
                Console.Write("Möchten Sie eine weitere Datei analysieren? (j/n)");
                string auswahl = Console.ReadLine();
                if (auswahl.ToLower() != "j")
                {
                    break;
                }
            }
            // Exceptionhandling für die einzelnen Fehler
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Das Verzeichnis konnte nicht gefunden werden.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Die Datei konnte nicht gefunden werden.");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Ungültige Datei-Nummer.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ungültige Eingabe für die Datei-Nummer.");
            }
            catch (Exception)
            {
                Console.WriteLine("Ein Fehler ist aufgetreten.");
            }
        }
    }
}
