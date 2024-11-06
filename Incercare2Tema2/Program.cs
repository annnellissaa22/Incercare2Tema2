using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Descarcă fișierele JSON
        string productionJson = File.ReadAllText("Resources/productions.json");
        string actionTableJson = File.ReadAllText("Resources/tables.json");

        // Deserializare
        var inputModel = JsonConvert.DeserializeObject<InputModel>(productionJson);
        var tables = DeserializeTables(actionTableJson);

        // Testează analiza
        string input = "i+i*i";  // Poți modifica acest input
        LRParser.Parse(inputModel, tables, input);
    }

    static Tables DeserializeTables(string json)
    {
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

        var actionTable = new Dictionary<(int, string), string>();
        var gotoTable = new Dictionary<(int, string), string>();

        foreach (var entry in jsonObject["ActionTable"])
        {
            var key = ParseKey(entry.Key);
            actionTable[key] = entry.Value;
        }

        foreach (var entry in jsonObject["GotoTable"])
        {
            var key = ParseKey(entry.Key);
            gotoTable[key] = entry.Value;
        }

        return new Tables(actionTable, gotoTable);
    }

    static (int, string) ParseKey(string key)
    {
        var parts = key.Trim('(', ')').Split(',');
        return (int.Parse(parts[0]), parts[1].Trim());
    }
}
