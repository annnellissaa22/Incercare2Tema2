using System;
using System.Collections.Generic;

public class Tables
{
    public Dictionary<(int, string), string> ActionTable { get; set; }
    public Dictionary<(int, string), string> GotoTable { get; set; }

    public Tables(Dictionary<(int, string), string> actionTable, Dictionary<(int, string), string> gotoTable)
    {
        ActionTable = actionTable;
        GotoTable = gotoTable;
    }
}
