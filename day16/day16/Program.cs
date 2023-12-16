using System.Drawing;

var lines = File.ReadAllLines("in.txt");
int width = lines[0].Length;
int height = lines.Length;

var map = string.Join("", lines).Select((c, i) => (c, i)).ToDictionary(c => new Point(c.i % width, c.i / height), c => c.c);

List<int> countEnergized = new List<int>();

for(int i = 0; i < width; i++)
{ 
  countEnergized.Add(CountEnergized(map, new Point(i, -1), new Point(0,1)));
  countEnergized.Add(CountEnergized(map, new Point(i, height), new Point(0, -1)));
}

for (int i = 0; i < height; i++)
{
  countEnergized.Add(CountEnergized(map, new Point(-1, i), new Point(1, 0)));
  countEnergized.Add(CountEnergized(map, new Point(width, i), new Point(-1, 0)));
}

Console.WriteLine(countEnergized.Max());

int CountEnergized(Dictionary<Point,char> map, Point startPoint, Point startDir)
{
  Queue<(Point, Point)> queue = new Queue<(Point, Point)>();
  queue.Enqueue((startPoint, startDir));
  HashSet<(Point, Point)> visited = new HashSet<(Point, Point)>();

  while (queue.Any())
  {
    var current = queue.Dequeue();
    Point currentPos = current.Item1;
    Point currentDir = current.Item2;

    if (!visited.Add((currentPos, currentDir)))
      continue;

    Point nextPos = new Point(currentPos.X + currentDir.X, currentPos.Y + currentDir.Y);
    if (!map.ContainsKey(nextPos))
      continue;

    if (map[nextPos] == '.')
      queue.Enqueue((nextPos, currentDir));

    if (map[nextPos] == '-')
    {
      if (currentDir.Y != 0)
      {
        queue.Enqueue((nextPos, new Point(-1, 0)));
        queue.Enqueue((nextPos, new Point(1, 0)));
      }
      else
        queue.Enqueue((nextPos, currentDir));
    }

    if (map[nextPos] == '|')
    {
      if (currentDir.X != 0)
      {
        queue.Enqueue((nextPos, new Point(0, -1)));
        queue.Enqueue((nextPos, new Point(0, 1)));
      }
      else
        queue.Enqueue((nextPos, currentDir));
    }

    if (map[nextPos] == '\\')
      queue.Enqueue((nextPos, new Point(currentDir.Y, currentDir.X)));

    if (map[nextPos] == '/')
      queue.Enqueue((nextPos, new Point(-1 * currentDir.Y, -1 * currentDir.X)));
  }

  return visited.Select(v => v.Item1).Distinct().Count() - 1;
}