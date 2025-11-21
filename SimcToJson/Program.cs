
// ŚCIEŻKA do pliku SIMC z GUS (CSV/TXT z separatorem ;)
using System.Text;
using System.Text.Json;

var simcPath = @"C:\Users\rw200\Downloads\SIMC_Statystyczny_2025-11-20\SIMC_Statystyczny_2025-11-20.csv";   // <<< TU PODMIEŃ NA SWOJĄ ŚCIEŻKĘ
var outputPath = @"C:\Users\rw200\OneDrive\Pulpit\JobSearch (2)\JobSearch\JobSearch\polish-locations.json"; // <<< TU GDZIE MA POWSTAĆ JSON

Console.OutputEncoding = Encoding.UTF8;

if (!File.Exists(simcPath))
{
    Console.WriteLine($"Nie znaleziono pliku SIMC: {simcPath}");
    return;
}

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var enc1250 = Encoding.GetEncoding(1250);
var lines = File.ReadAllLines(simcPath, enc1250);

if (lines.Length == 0)
{
    Console.WriteLine("Plik SIMC jest pusty.");
    return;
}

// Pierwsza linia – nagłówek, szukamy indeksu kolumny "NAZWA"
var header = lines[0].Split(';');
int nazwaIndex = Array.FindIndex(header, h => h.Trim().Equals("NAZWA", StringComparison.OrdinalIgnoreCase));

if (nazwaIndex == -1)
{
    Console.WriteLine("Nie znaleziono kolumny NAZWA w pliku SIMC.");
    return;
}

var names = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

// Lecimy po wszystkich wierszach oprócz nagłówka
for (int i = 1; i < lines.Length; i++)
{
    var parts = lines[i].Split(';');
    if (parts.Length <= nazwaIndex) continue;

    var name = parts[nazwaIndex].Trim();
    if (string.IsNullOrWhiteSpace(name)) continue;

    names.Add(name);
}
Console.WriteLine($"Zebrano nazw miejscowości: {names.Count}");

var locationObjects = names
    .OrderBy(n => n)
    .Select(n => new { name = n })
    .ToList();

var json = JsonSerializer.Serialize(locationObjects,
    new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    });

File.WriteAllText(outputPath, json, Encoding.UTF8);
Console.WriteLine($"Zapisano plik: {outputPath}");
