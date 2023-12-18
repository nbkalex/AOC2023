using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Http.Headers;

Dictionary<string, Point> directions = new Dictionary<string, Point>()
{
  {"U", new Point(0,-1) },
  {"D", new Point(0,1) },
  {"L", new Point(-1,0) },
  {"R", new Point(1,0) },
};

var neighbors = new List<Point>() { new Point(-1,0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };

var input = File.ReadAllLines("in.txt")
  .Select(l => l.Split(" "))
  .Select(l => (dir: directions[l[0]], length: int.Parse(l[1]), part2: l[2].Substring(2,6)));

List<Point> map = new List<Point>(){new Point() };
foreach(var line in input)
{
  for(int i = 0; i < line.length; i++)
    map.Add(new Point(map.Last().X + line.dir.X, map.Last().Y + line.dir.Y));
}

int minY = map.Min(x => x.Y);
int minX = map.Where(p => p.Y == minY).Min(p => p.X);

Point topLeft = new Point(minX, minY);
var visited = map.ToHashSet();
while (map.Contains(topLeft))
  topLeft = new Point(topLeft.X+1, topLeft.Y+1);

Queue<Point> queue = new Queue<Point>();
queue.Enqueue(topLeft);

while (queue.Any())
{
  var p = queue.Dequeue();
  foreach (var n in neighbors)
  {
    Point nextPoint = new Point(p.X + n.X, p.Y + n.Y);
    if(!visited.Add(nextPoint))
      continue;

    queue.Enqueue(nextPoint);
  }
}

Console.WriteLine(visited.Count());


// Part 2
Dictionary<char, string> directionsByIndex = new Dictionary<char, string>()
{
  {'0', "R"},  // Right
  {'1', "D"},  // Down
  {'2', "L"}, // Left
  {'3', "U"}, // Up
};

 var input2 = input.Select(i => (directionsByIndex[i.part2[5]], int.Parse(i.part2.Substring(0, 5), System.Globalization.NumberStyles.HexNumber))).ToArray();

List<(Point, Point)> intervals = new List<(Point,Point)>();
Point current = new Point();
foreach(var i in input2)
{
  Point dir = directions[i.Item1];
  int distance = i.Item2;
  Point end = new Point(current.X + dir.X * distance, current.Y + dir.Y * distance);
  intervals.Add((current, end));
  current = end;
}

intervals.Reverse();
Console.WriteLine(PolygonArea(intervals.Select(i => i.Item1).ToArray()));

double PolygonArea(Point[] verices)
{
  // Initialize area
  double area = 0.0;

  // Calculate value of shoelace formula
  int j = verices.Length - 1;
  for (int i = 0; i < verices.Length; i++)
  {
    area += (verices[j].X + verices[i].X) * (verices[j].Y - verices[i].Y);
    j = i;  // j is previous vertex to i
  }

  // Return absolute value
  return Math.Abs(area / 2.0);
}

// 952408144115
// 1069685419