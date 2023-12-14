var lines = File.ReadAllLines("in.txt").Select(l => l.ToArray()).ToArray();

int width = lines[0].Length;
int height = lines.Length;

Dictionary<string, int> cached = new Dictionary<string, int>();
int begins = 0;
int ends = 0;
for (int rotate = 0; rotate < 1000000000; rotate++)
{
  for (int i = 1; i < height; i++)
  {
    for (int j = 0; j < width; j++)
    {
      if (lines[i][j] != 'O')
        continue;

      int currentHeight = i - 1;
      while (currentHeight >= 0 && lines[currentHeight][j] == '.')
      {
        lines[currentHeight][j] = 'O';
        lines[currentHeight + 1][j] = '.';
        currentHeight--;
      }
    }
  }

  string asString = ToString(lines) + rotate % 4;
  if (cached.ContainsKey(asString))
  {
    begins = cached[asString];
    ends = rotate;
    Console.WriteLine(cached[asString] + " - " + rotate);
    break;
  }

  cached.Add(asString, rotate);
  lines = Rotate(lines);
}

// 99460 too low

var foundRotate = begins + (4000000000 - begins - 1) % (ends - begins);
var found = cached.First(kvp => kvp.Value == foundRotate).Key;
found = found.Remove(found.Length-1);
lines = found.Split("\r\n").Select(l => l.ToArray()).ToArray();
var rotations = 4 - foundRotate % 4;
for (int i = 0; i < rotations; i++)
  lines = Rotate(lines);

int load = 0;
for (int i = 0; i < height; i++)
  for (int j = 0; j < width; j++)
    if (lines[i][j] == 'O')
      load += height - i;

Console.WriteLine("result: " +load);

//foreach (var line in cached)
//{
//  int loadBefore = 0;
//  for (int i = 0; i < height; i++)
//    for (int j = 0; j < width; j++)
//      if (lines[i][j] == 'O')
//        loadBefore += height - i;

//  var currentRotate = 4 - line.Value % 4;
//  lines = line.Key.Split("\r\n").Select(l => l.ToArray()).ToArray();
//  for (int i = 0; i < currentRotate; i++)
//    lines = Rotate(lines);

//  load = 0;
//  for (int i = 0; i < height; i++)
//    for (int j = 0; j < width; j++)
//      if (lines[i][j] == 'O')
//        load += height - i;

//  Console.WriteLine(load + " - " + loadBefore + " - " + line.Value);
//}

char[][] Rotate(char[][] pattern)
{

  List<char[]> lines = new List<char[]>();
  for (int i = 0; i < pattern[0].Length; i++)
  {
    var newLine = pattern.Select(l => l[i]).Reverse();
    lines.Add(newLine.ToArray()); ;
  }
  return lines.ToArray();
}

string ToString(char[][] pattern)
{
  return string.Join("\r\n", pattern.Select(l => string.Join("", l)));
}