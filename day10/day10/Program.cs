using System.Drawing;
using System.Linq;

var lines = File.ReadAllLines("in.txt");

var neighbors = new List<Point>()
{
  new Point(0,1), new Point(0,-1), new Point(-1, 0), new Point(1, 0)
};

Dictionary<Point, char> pipesMap = new Dictionary<Point, char>();
for (int i = 0; i < lines.Length; i++)
  for (int j = 0; j < lines[i].Length; j++)
    pipesMap.Add(new Point(j, i), lines[i][j]);

var visited = new Dictionary<Point, int>();
var nextPoints = new Queue<Point>();
nextPoints.Enqueue(pipesMap.First(pm => pm.Value == 'S').Key);
visited.Add(nextPoints.Last(), 0);
while (nextPoints.Any())
{
  var current = nextPoints.Dequeue();

  foreach (var neighbor in neighbors)
  {
    Point n = new Point(current.X + neighbor.X, current.Y + neighbor.Y);
    if (pipesMap.ContainsKey(n)
    && (!visited.ContainsKey(n) || visited[n] > visited[current] + 1)
    && AreConnected((current, pipesMap[current]), (n, pipesMap[n])))
    {
      nextPoints.Enqueue(n);
      if (visited.ContainsKey(n))
        visited[n] = visited[current] + 1;
      else
        visited.Add(n, visited[current] + 1);
    }
  }
}

Console.WriteLine(visited.Values.Max());

Dictionary<Point, bool> visitedTiles = new Dictionary<Point, bool>();

var tocheck = pipesMap.Where(m => !visited.ContainsKey(m.Key) && m.Value != 'S');
foreach (var tile in tocheck)
{
  if (visitedTiles.ContainsKey(tile.Key))
    continue;

  bool valid = true;
  Queue<Point> toVisit = new Queue<Point>();
  toVisit.Enqueue(tile.Key);
  HashSet<Point> visitedTiles2 = new HashSet<Point>(){tile.Key };
  while (toVisit.Count > 0)
  {
    var current = toVisit.Dequeue();
    foreach (var neighbor in neighbors)
    {
      Point n = new Point(current.X + neighbor.X, current.Y + neighbor.Y);
      if (!pipesMap.ContainsKey(n))
      { 
        valid = false;
        continue;
      }

      if(visited.ContainsKey(n)) 
        continue;

      if (visitedTiles2.Contains(n))
        continue;

      if (pipesMap[n] == '.')
      { 
        toVisit.Enqueue(n);
        visitedTiles2.Add(n);
      }
    }
  }

  foreach (var tile2 in visitedTiles2)
    if(!visitedTiles.ContainsKey(tile2))
      visitedTiles.Add(tile2, valid);

}

//string toIgnore = "|-S";
//var ignored = visited.Where(v => !toIgnore.Contains(pipesMap[v.Key]));

foreach (var vt in visitedTiles.Keys)
{
  if (!visitedTiles[vt])
    continue;

  int leftCount =  visited.Count(v =>vt.Y == v.Key.Y && vt.X > v.Key.X);
  int rightCount = visited.Count(v =>vt.Y == v.Key.Y && vt.X < v.Key.X);
  int topCount =   visited.Count(v =>vt.X == v.Key.X && vt.Y < v.Key.Y);
  int botCount =   visited.Count(v =>vt.X == v.Key.X && vt.Y > v.Key.Y);
  List<int> edges = new List<int>() { leftCount, rightCount, topCount, botCount };
  if (edges.All(e => e % 2 == 0))
    visitedTiles[vt] = false;
}

Console.WriteLine(visitedTiles.Count(vt => vt.Value));

const int DISPLAY_OFFSET = 10;

foreach(var m in pipesMap)
{
  Console.SetCursorPosition(DISPLAY_OFFSET + m.Key.X, DISPLAY_OFFSET + m.Key.Y);
  char display = !visitedTiles.ContainsKey(m.Key) ? m.Value : visitedTiles[m.Key] ? 'I' : 'O';

   if(display == 'O')
    Console.ForegroundColor = ConsoleColor.Red;
  else if (display == 'I')
    Console.ForegroundColor = ConsoleColor.Blue;

  Console.WriteLine(display);

  Console.ResetColor();
}


bool AreConnected((Point, char) p1, (Point, char) p2)
{
  Dictionary<char, (Point, Point)> pipeTypes = new Dictionary<char, (Point, Point)>()
{
  {'|', (new Point(0,1), new Point(0,-1)) },
  {'-', (new Point(1,0), new Point(-1,0)) },
  {'L', (new Point(0,-1), new Point(1, 0)) },
  {'J', (new Point(0, -1), new Point(-1, 0)) },
  {'7', (new Point(-1, 0), new Point(0,1)) },
  {'F', (new Point(1, 0), new Point(0,1)) },
  {'.', (new Point(), new Point()) }
};

  bool firstConnectsSecond = p1.Item2 == 'S';
  if (!firstConnectsSecond)
  {
    Point p12c1 = new Point(p1.Item1.X + pipeTypes[p1.Item2].Item1.X,
                           p1.Item1.Y + pipeTypes[p1.Item2].Item1.Y);
    Point p12c2 = new Point(p1.Item1.X + pipeTypes[p1.Item2].Item2.X,
                           p1.Item1.Y + pipeTypes[p1.Item2].Item2.Y);
    firstConnectsSecond = p12c1 == p2.Item1 || p12c2 == p2.Item1;
  }

  Point p22c1 = new Point(p2.Item1.X + pipeTypes[p2.Item2].Item1.X,
                         p2.Item1.Y + pipeTypes[p2.Item2].Item1.Y);
  Point p22c2 = new Point(p2.Item1.X + pipeTypes[p2.Item2].Item2.X,
                         p2.Item1.Y + pipeTypes[p2.Item2].Item2.Y);

  return (p22c1 == p1.Item1 || p22c2 == p1.Item1) && firstConnectsSecond;
}
