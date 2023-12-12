var lines = File.ReadAllLines("in.txt")
  .Select(l => (l.Split(" ")[0], l.Split(" ")[1].Split(",").Select(t => int.Parse(t)).ToList()));


// part1
int sum = 0;
foreach (var line in lines)
{
  string row = "";
  List<int> validate = new List<int>();
  for (int i = 0; i < 5; i++)
  {
    row += line.Item1;
    if (i < 4)
      row += "?";
    validate.AddRange(line.Item2);
  }

  row = string.Join("", row.Reverse());
  validate.Reverse();

  int total = validate.Sum();
  int countTotal = row.Count(c => c == '?' || c == '#');

  var broken = row.Select((c, i) => (c, i)).Where(c => c.c == '?').OrderBy(c => c.i).ToArray();

  var generated = new HashSet<(string,int)>() { (row, countTotal) };
  foreach (var b in broken)
  {
    HashSet<(string,int)> g2 = new HashSet<(string,int)>();
    foreach (var g in generated)
    {
      string noBroken = g.Item1.Remove(b.i, 1);
      string first = noBroken.Insert(b.i, "#");
      string second = noBroken.Insert(b.i, ".");

      if (IsValid(first, validate, b.i, total, g.Item2))
        g2.Add((first, g.Item2));

      if (IsValid(second, validate, b.i, total, g.Item2-1))
        g2.Add((second, g.Item2 - 1));
    }

    generated = g2;

  }
  var validated = generated.Where(g => IsValid(g.Item1, validate, g.Item1.Length - 1, total, int.MaxValue));
  bool x = false;
  if (x)
  {
    foreach (var vg in validated)
      Console.WriteLine(vg + "    ");
  }

  Console.WriteLine(validated.Count());
  sum += validated.Count();
}

Console.WriteLine(sum);

bool IsValid(string line, List<int> validation, int index, int total, int countTotal)
{
  if (countTotal < total)
    return false;

  string subline = line.Substring(0, index + 1);
  var tokens = subline.Split(".", StringSplitOptions.RemoveEmptyEntries);
  if (index == line.Length - 1 && tokens.Length != validation.Count)
    return false;

  if (tokens.Length == 0)
    return true;

  if (tokens.Length > validation.Count)
    return false;

  for (int i = 0; i < tokens.Length - 1; i++)
    if (tokens[i].Length != validation[i])
      return false;

  if (index < line.Length - 1)
    return tokens.Last().Length <= validation[tokens.Length - 1];
  else
    return tokens.Last().Length == validation[tokens.Length - 1];
}