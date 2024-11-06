using System;
using System.Collections.Generic;

public class InputModel
{
    public List<Production> Productions { get; set; } = new List<Production>();
}

public class Production
{
    public string Left { get; set; }
    public string Right { get; set; }
}
