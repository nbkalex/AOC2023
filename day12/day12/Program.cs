using System.Diagnostics;

var lines = File.ReadAllLines("in.txt")
  .Select(l => (l.Split(" ")[0], l.Split(" ")[1].Split(",").Select(t => int.Parse(t)).ToList()));


// part1
long sum = 0;
int countLine = 0;
foreach (var line in lines)
{
  Stopwatch stopwatch = new Stopwatch();
  stopwatch.Start();

  countLine++;
  string row = "";
  List<int> validate = new List<int>();

  const int REPEAT = 5;
  for (int i = 0; i < REPEAT; i++)
  {
    row += line.Item1;
    if (i < REPEAT-1)
      row += "?";
    validate.AddRange(line.Item2);
  }

  row = string.Join("", row.Reverse());
  validate.Reverse();

  int total = validate.Sum();
  int countTotal = row.Count(c => c == '?' || c == '#');
  int countHashTag = row.Count(c => c == '#');

  var broken = row.Select((c, i) => (c, i)).Where(c => c.c == '?').OrderBy(c => c.i).ToArray();

  var generated = new List<(string,int, int)>() { (row, countTotal, countHashTag) };
  foreach (var b in broken)
  {
    var g2 = new List<(string,int, int)>();
    foreach (var g in generated)
    {
      string noBroken = g.Item1.Remove(b.i, 1);
      string first = noBroken.Insert(b.i, "#");
      string second = noBroken.Insert(b.i, ".");

      if (IsValid(first, validate, b.i, total, g.Item2, g.Item3+1))
        g2.Add((first, g.Item2, g.Item3 + 1));

      if (IsValid(second, validate, b.i, total, g.Item2-1, g.Item3))
        g2.Add((second, g.Item2 - 1, g.Item3));
    }

    generated = g2;

  }
  var validated = generated.Where(g => IsValid(g.Item1, validate, g.Item1.Length - 1, total, int.MaxValue, g.Item3)).ToList();
  bool x = false;
  if (x)
  {
    foreach (var vg in validated)
      Console.WriteLine(vg + "    ");
  }

  Console.WriteLine(countLine.ToString() + ": " + validated.Count() + " - " + stopwatch.Elapsed);
  sum += validated.Count();
}

Console.WriteLine(sum);

bool IsValid(string line, List<int> validation, int index, int total, int countTotal, int countHashTag)
{
  if (countTotal < total)
    return false;

  if(countHashTag > total)
    return false;

  string subline = line.Substring(0, index + 1);
  var tokens = subline.Split(".", StringSplitOptions.RemoveEmptyEntries);

  

  if (index == line.Length - 1 && tokens.Length != validation.Count)
    return false;

  if (tokens.Length == 0)
    return true;

  if (tokens.Length > validation.Count)
    return false;

  if (tokens.Any() && tokens.Last().Length < validation[tokens.Length - 1]
      && subline.EndsWith('.'))
    return false;

  for (int i = 0; i < tokens.Length - 1; i++)
    if (tokens[i].Length != validation[i])
      return false;

  if (index < line.Length - 1)
  { 
    if(tokens.Last().Length <= validation[tokens.Length - 1])
      return true;

    return tokens.Last().Length <= validation[tokens.Length - 1];
  }
  else
    return tokens.Last().Length == validation[tokens.Length - 1];
}