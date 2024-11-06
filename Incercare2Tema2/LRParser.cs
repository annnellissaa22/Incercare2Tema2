using ConsoleTableExt;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;

public static class LRParser
{
    public static void Parse(InputModel model, Tables tables, string input = "i+i*i")
    {
        Stack<(char, int)> stack = new();
        stack.Push(('$', 0));
        input += '$';

        // Crearea unui tabel cu ConsoleTableExt
        var tabel = new ConsoleTable("Actiune", "Stiva");

        while (true)
        {
            var currentS = stack.Peek();
            var currentIn = input[0];

            var actionKey = (currentS.Item2, currentIn.ToString());
            if (!tables.ActionTable.TryGetValue(actionKey, out var action))
            {
                Console.WriteLine($"[ERORARE] Cheia {actionKey} nu exista in ActionTable.");
                return;
            }

            tabel.AddRow(action, string.Join(" ", stack.ToArray().Reverse().Select(el => $"{el.Item1}:{el.Item2}")));

            if (action[0] == 'd') // Deplasare
            {
                stack.Push((currentIn, int.Parse(action[1..])));
                input = input[1..];
            }
            else if (action[0] == 'r') // Reducere
            {
                int productionNumber = int.Parse(action[1..]);
                var production = model.Productions[productionNumber - 1];

                // Pop pentru fiecare simbol din producție
                for (int i = 0; i < production.Right.Length; i++)
                {
                    stack.Pop();
                }

                // Verifică și obține noua stare din GotoTable
                var topState = stack.Peek().Item2;
                var nonTerminal = production.Left[0];
                var gotoKey = (topState, nonTerminal.ToString());

                if (tables.GotoTable.TryGetValue(gotoKey, out var newStateStr))
                {
                    var newState = int.Parse(newStateStr);
                    stack.Push((nonTerminal, newState));
                }
                else
                {
                    Console.WriteLine($"[ERORARE] Cheia {gotoKey} nu exista in GotoTable.");
                    return;
                }
            }
            else if (action[0] == 'a') // Accept
            {
                Console.WriteLine("Input acceptat.");
                break;
            }
            else
            {
                Console.WriteLine($"[ERORARE] Actiune necunoscută: {action}");
                return;
            }

            tabel.Write();
        }
    }
}
