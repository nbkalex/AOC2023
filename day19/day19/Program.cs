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
        Func<int, bool>? condition = null;
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

    return (id, rules);
  }).ToDictionary(k => k.id, v => v.rules);

var parts = input[1].Split("\r\n")
  .Select(partStr =>
  {
    return partStr.Trim('{').Trim('}').Split(",")
    .Select(pt => pt.Split("="))
    .ToDictionary(k => k[0], v => int.Parse(v[1]));
  }).ToArray();

int bound = 4001;
long count = 0;

for (int i1 = 1; i1 < bound; i1++)
  for (int i2 = 1; i2 < bound; i2++)
    for (int i3 = 1; i3 < bound; i3++)
      for (int i4 = 1; i4 < bound; i4++)
      {
        Dictionary<string, int> part = new Dictionary<string, int>()
        {
          { "x", i1 }, { "m", i2 }, { "a", i3 }, { "s" , i4}
        };

        if (IsAccepted(part))
          count++;
      }

Console.WriteLine();
Console.WriteLine(count);

bool IsAccepted(Dictionary<string, int> part)
{
  string workflowId = "in";
  while (true)
  {
    if (workflowId == "R")
      return false;

    if (workflowId == "A")
    {
      return true;
    }

    var workflow = workflows[workflowId];
    foreach (var rule in workflow)
    {
      if (rule.condition == null || rule.condition(part[rule.op1]))
      {
        workflowId = rule.destination;
        break;
      }
    }
  }
}