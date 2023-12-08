var lines = File.ReadAllLines("in.txt");

string instructions = lines[0];
var map = lines.Skip(2).ToDictionary(
  l => l.Split(" = ")[0], 
  l => l.Split(" = ")[1]
          .Replace(")", "")
          .Replace("(", "")
          .Split(", "));

int step = 0;
//string current = "AAA";

//while(current != "ZZZ")
//{
//  int index = step % instructions.Length;
//  int lrIndex = instructions[index] == 'L' ? 0 : 1;
//  current = map[current][lrIndex];
//  step++;
//}

var starts = map.Where(m => m.Key.EndsWith("A")).Select(kvp => kvp.Key).ToArray();
Dictionary<int, int> stepsToEnd = new Dictionary<int, int>();

while (stepsToEnd.Count() < starts.Length)
{
  int index = step % instructions.Length;
  int lrIndex = instructions[index] == 'L' ? 0 : 1;

  for (int i = 0; i < starts.Length; i++)
  { 
    starts[i] = map[starts[i]][lrIndex];
    if (starts[i].EndsWith('Z') && !stepsToEnd.ContainsKey(i))
      stepsToEnd.Add(i, step+1);
  }

  step++;
}


Console.WriteLine(string.Join(", ", stepsToEnd.Values.Select(s => s.ToString())));