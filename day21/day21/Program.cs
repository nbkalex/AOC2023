using System.Diagnostics.Metrics;
using System.Drawing;

var lines = File.ReadAllLines("in.txt");
int width = lines[0].Length;
int height = lines.Length;

var map = string.Join("", lines).Select((c, i) => (new Point(i % width, i / height), c)).ToDictionary(p => p.Item1, p => p.c);
Point start = map.First(p => p.Value == 'S').Key;
map[start] = '.';

var neighbors = new List<Point>()
{
  new Point(1,0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)
};
const int STEPS = 65 + (131 * 1);


List<long> differences = new List<long>();
List<long> couts = new List<long>();
HashSet<Point> points = new HashSet<Point>() { start };
for (long i = 0; i < STEPS; i++)
{
  HashSet<Point> newpoints = new HashSet<Point>();
  foreach (var p in points)
  {
    foreach (var n in neighbors)
    {
      Point p2 = new Point(p.X + n.X, p.Y + n.Y);
      Point normalized = new Point((100 * width + p2.X) % width, (100 * height + p2.Y) % height);

      if (map[normalized] != '#')
        newpoints.Add(p2);
    }
  }

  points = newpoints;
}

PrintMap(points);

long countShapes = (STEPS - lines[0].Length / 2) / lines[0].Length;
countShapes = 2 * countShapes + 1;

//int pointsCount = 3885;
//Console.WriteLine(countShapes * countShapes * pointsCount);

// 628201665206816 too low
// 628404721710275
// 317583390107640
// 628207875824572 too high

//Console.WriteLine(5 * pointsCount);

Console.WriteLine(points.Count);

void PrintMap(HashSet<Point> points)
{
  int width = points.Max(p => p.X) - points.Min(p => p.X);
  int height = points.Max(p => p.Y) - points.Min(p => p.Y);

  List<List<char>> list = new List<List<char>>();
  for (int i = 0; i < height + 1; i++)
  {
    list.Add(new List<char>());
    for (int j = 0; j < width + 1; j++)
      list.Last().Add(' ');
  }

  int offsetx = Math.Abs(points.Min(p => p.X));
  int offsety = Math.Abs(points.Min(p => p.Y));
  foreach (var point in points)
    list[offsety + point.Y][offsetx + point.X] = (char)0x25A0;


  string mapText = string.Join("\r\n", list.Select(pt => string.Join("", pt)));
  File.WriteAllText("out.txt", mapText);
}