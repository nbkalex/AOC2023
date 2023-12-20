using System.Collections.Generic;
using System.Data;

var input = File.ReadAllText("in.txt").Split("\r\n\r\n");

var workflows = input[0].Split("\r\n").Select(
  w =>
  {
    string[] wTokens = w.Split("{");
    string id = wTokens[0];
    wTokens[1] = wTokens[1].TrimEnd('}');
    var rules = wTokens[1].Split(",").Select(
      rt =>
      {
        string[] ruleTokens = rt.Split(":");
        string conditionStr = ruleTokens.Length > 1 ? ruleTokens[0] : "";
        Func<int, bool> condition = null;
        string op1 = "";
        if (conditionStr.Any())
        {
          op1 = conditionStr[0].ToString();
          int op2 = int.Parse(conditionStr.Substring(2));
          condition = conditionStr[1] == '<'
            ? (att) => att < op2
            : (att) => att > op2;
        }

        string destination = ruleTokens.Length == 1 ? ruleTokens[0] : ruleTokens[1];

        return (op1, condition, destination);
      }).ToArray();


    rules[rules.Length - 1] = (rules[rules.Length - 1].op1, (att) => rules.SkipLast(1).All(r => !r.condition(att)), rules[rules.Length - 1].destination);
    return (id, rules);
  }).ToDictionary(k => k.id, v => v.rules);

var parts = input[1].Split("\r\n")
  .Select(partStr =>
  {
    return partStr.Trim('{').Trim('}').Split(",")
    .Select(pt => pt.Split("="))
    .ToDictionary(k => k[0], v => int.Parse(v[1]));
  }).ToArray();

long count = 0;

foreach (var part in parts)
{
  string workflowId = "in";
  while (true)
  {
    if (workflowId == "R")
      break;

    if (workflowId == "A")
    {
      count += part.Values.Sum();
      break;
    }

    var workflow = workflows[workflowId];
    foreach (var rule in workflow)
    {
      if (rule.op1 == "" || rule.condition(part[rule.op1]))
      {
        workflowId = rule.destination;
        break;
      }
    }
  }
}

Queue<(string, Dictionary<string, int[]>)> toVisit = new Queue<(string, Dictionary<string, int[]>)>();

Dictionary<string, int[]> combinations = new Dictionary<string, int[]>()
{
  { "x", Enumerable.Range(1, 4000).ToArray() },
  { "m", Enumerable.Range(1, 4000).ToArray() },
  { "a", Enumerable.Range(1, 4000).ToArray() },
  { "s", Enumerable.Range(1, 4000).ToArray() }
};

toVisit.Enqueue(("in", combinations));
List<int[][]> acctepted = new List<int[][]>();

while (toVisit.Any())
{
  var current = toVisit.Dequeue();

  if (current.Item1 == "A")
  {
    acctepted.Add(current.Item2.Values.ToArray());
    continue;
  }

  if (current.Item1 == "R")
  {
    continue;
  }

  foreach (var rule in workflows[current.Item1].SkipLast(1))
  {
    var combN = current.Item2.ToDictionary(k => k.Key, k => k.Value);

    var combNext = combN[rule.op1].Where(c => rule.condition(c)).ToArray();
    combN[rule.op1] = combNext;
    toVisit.Enqueue((rule.destination, combN));
  }

  // last has no condition
  var comb = current.Item2.ToDictionary(k => k.Key, k => k.Value);
  foreach (var rule2 in workflows[current.Item1].SkipLast(1))
  {
    var combNext2 = comb[rule2.op1].Where(c => !rule2.condition(c)).ToArray();
    comb[rule2.op1] = combNext2;
  }

  toVisit.Enqueue((workflows[current.Item1].Last().destination, comb));
}

Console.WriteLine(count);
Console.WriteLine(acctepted.Select(a => a.Aggregate((long)1, (s, v) => s * v.Length)).Sum());

