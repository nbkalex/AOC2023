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

HashSet<Point> points = new HashSet<Point>() { start };
Dictionary<Point, long> repeatStepts = new Dictionary<Point, long>();

HashSet<long> toPrint = new HashSet<long>() { 6,10, 50, 100, 500, 1000, 5000 };

for (long i = 0; i < 26501365; i++)
{
  //if (toPrint.Contains(i))
  //  Console.WriteLine(i.ToString() + " " +points.Count());

  HashSet<Point> newpoints = new HashSet<Point>();
  foreach (var p in points)
  {
    foreach(var n in neighbors)
    {
      Point p2 = new Point(p.X + n.X, p.Y + n.Y);
      Point normalized = new Point((100 * width + p2.X) % width, (100 * height + p2.Y) % height);
      if (normalized == start)
      {
        var hashPoint = new Point(p2.X / width, p2.Y / height);
        bool added = repeatStepts.ContainsKey(hashPoint);
        if (!added)
        {
          repeatStepts.Add(hashPoint, i);
          Console.WriteLine(hashPoint.ToString() + " " + i);
          Console.ReadKey();
        }
      }

      if(map[normalized] != '#')
        newpoints.Add(p2);
    }
  }

  points = newpoints;
}

Console.WriteLine(points.Count());