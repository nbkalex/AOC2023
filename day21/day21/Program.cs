using System.Drawing;

var lines = File.ReadAllLines("in.txt");
int width = lines[0].Length;
int height = lines.Length;

var map = string.Join("", lines).Select((c, i) => (new Point(i%width, i/height),c)).ToDictionary(p => p.Item1, p => p.c);
Point start = map.First(p =>p.Value == 'S').Key;
map[start] = '.';

var neighbors = new List<Point>()
{
  new Point(1,0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)
};

IEnumerable<Point> points = new List<Point>() { start };
for (int i = 0; i < 64; i++)
{
  List<Point> newpoints = new List<Point>();
  foreach(var p in points)
  {
    var toAdd = neighbors.Select(n => new Point(p.X + n.X, p.Y + n.Y)).Where(p => map.ContainsKey(p) && map[p] != '#');
    newpoints.AddRange(toAdd);
  }

  points = newpoints.Distinct();
}

Console.WriteLine(points.Count());