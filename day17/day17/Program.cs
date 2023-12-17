using System.Drawing;

var lines = File.ReadAllLines("in.txt");
int width = lines[0].Length;
int height = lines.Length;
var map = string.Join("", lines).Select((c, i) => (c, i)).ToDictionary(c => new Point(c.i % width, c.i / width), c => int.Parse(c.c.ToString()));

Dictionary<(Point, Point, int), int> visited = new Dictionary<(Point, Point, int), int>();

Queue<(Point, Point, int, int)> queue = new Queue<(Point, Point, int, int)>();
Point start = new Point();
Point end = new Point(width - 1, height - 1);
int totalEnd = int.MaxValue;

queue.Enqueue((start, new Point(1, 0), 1, 0));
visited.Add((start, new Point(1, 0), 1), 0);

var directions = new List<Point>() { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };

while (queue.Any())
{
  var current = queue.Dequeue();

  Point pos = current.Item1;
  Point dir = current.Item2;

  int countStrait = current.Item3;
  int total = current.Item4;

  foreach (var direction in directions)
  {
    if (direction.X == -1 * dir.X && direction.Y == -1 * dir.Y)
      continue;

    Point newPos = new Point(pos.X + direction.X, pos.Y + direction.Y);
    if (!map.ContainsKey(newPos))
      continue;

    bool changeDir = direction != dir;
    int nextCountStrait = changeDir ? 1 : countStrait + 1;
    if(nextCountStrait > 3)
      continue;

    int nextTotal = total + map[newPos];

    var hash = (newPos, direction, nextCountStrait);
    if (visited.ContainsKey(hash))
    {
      if(visited[hash] <= nextTotal)
        continue;

      visited[hash] = nextTotal;
    }
    else
      visited.Add(hash, nextTotal);

    if(newPos == end && totalEnd > nextTotal)
      totalEnd = nextTotal;

    queue.Enqueue((newPos, direction, nextCountStrait, nextTotal));
  }
}

Console.WriteLine(totalEnd);
