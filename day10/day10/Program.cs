using System.Diagnostics.CodeAnalysis;

Dictionary<char, (Point, Point)> pipeTypes = new Dictionary<char, (Point, Point)>()
{
  {'-', (new Point(1,0), new Point(-1,0)) },
  {'|', (new Point(0,1), new Point(0,-1)) },
  {'L', (new Point(0,-1), new Point(1, 0)) },
  {'J', (new Point(0, -1), new Point(-1, 0)) },
  {'7', (new Point(-1, 0), new Point(0,1)) },
  {'F', (new Point(1, 0), new Point(0,1)) }
};

var lines = File.ReadAllLines("in.txt");

var neighbors = new List<Point>()
{
  new Point(0,1), new Point(0,-1), new Point(-1, 0), new Point(1, 0)
};

Dictionary<Point, char> pipesMap = new Dictionary<Point, char>();
for (int i = 0; i < lines.Length; i++)
  for (int j = 0; j < lines[i].Length; j++)
    pipesMap.Add(new Point(j, i), lines[i][j]);

var startPos = pipesMap.First(pm => pm.Value == 'S').Key;
Point next = new Point();
foreach (var n in neighbors)
{
  Point p = new Point(startPos.X + n.X, startPos.Y + n.Y);
  if(!pipesMap.ContainsKey(p) || !pipeTypes.ContainsKey(pipesMap[p]))
    continue;

  var nDir1 = pipeTypes[pipesMap[p]].Item1;
  var nDir2 = pipeTypes[pipesMap[p]].Item2;

  if (new Point(nDir1.X + p.X, nDir1.Y + p.Y) == startPos
   || new Point(nDir2.X + p.X, nDir2.Y + p.Y) == startPos)
  {
    next = p;
    break;
  }
}

pipesMap[startPos] = 'L';

List<Point> loop = new List<Point>() { startPos };
next = startPos;
var hash = loop.ToHashSet();
var intermediates = new List<Point>();
while (true)
{
  var val = pipesMap[next];
  var dir = pipeTypes[val];

  Point next1 = new Point(next.X + dir.Item1.X, next.Y + dir.Item1.Y);
  Point next2 = new Point(next.X + dir.Item2.X, next.Y + dir.Item2.Y);

  foreach (var n in neighbors)
  {
    Point p = new Point(next.X + n.X, next.Y + n.Y);
    if (p != next1 && p != next2)
    {
      Point intermediate = new Point(next.X + n.X / 2, next.Y + n.Y / 2);
      if (!pipesMap.ContainsKey(intermediate))
      { 
        pipesMap.Add(intermediate, ' ');
        intermediates.Add(intermediate);
      }
    }
    else
    {
      Point intermediate1 = new Point(next.X + dir.Item1.X / 2, next.Y + dir.Item1.Y / 2);
      Point intermediate2 = new Point(next.X + dir.Item2.X / 2, next.Y + dir.Item2.Y / 2);

      char val1 = intermediate1.X == next.X ? '|' : '-';
      char val2 = intermediate2.X == next.X ? '|' : '-';
      if (!pipesMap.ContainsKey(intermediate1))
      { 
        pipesMap.Add(intermediate1, val1);
        loop.Add(intermediate1);
      }
      if (!pipesMap.ContainsKey(intermediate2))
      { 
        pipesMap.Add(intermediate2, val2);
        loop.Add(intermediate2);
      }

    }
  }

  if (!hash.Contains(next1))
    next = next1;
  else
  {
    if (!hash.Contains(next2))
      next = next2;
    else
      break;
  }

  loop.Add(next);
  hash.Add(next);
}

//Console.WriteLine(loop.Count / 2);

//const int DISPLAY_OFFSET = 10;

//foreach (var m in pipesMap)
//{
//  Console.SetCursorPosition(DISPLAY_OFFSET + (int)(2 * m.Key.X), DISPLAY_OFFSET + (int)(2 * m.Key.Y));
//  char display = m.Value;

//  if (hash.Contains(m.Key))
//    Console.ForegroundColor = ConsoleColor.Blue;
//  Console.WriteLine(display);

//  Console.ResetColor();
//}

Console.SetCursorPosition(0, 0);

Queue<Point> toVisit = new Queue<Point>();
 
float minY = loop.Min(p => p.Y) + 0.5f;
float minX = loop.Where(p => p.Y == minY).Min(p => p.X) + 0.5f;

Point topLeft = new Point(minX, minY);
toVisit.Enqueue(topLeft);
int count = 0;
HashSet<Point> visited = new HashSet<Point>();
while(toVisit.Any())
{
  Point current = toVisit.Dequeue();

  if (pipesMap.ContainsKey(current) && pipesMap[current] != ' ')
    count++;

  foreach (var n in neighbors)
  {
    Point f = new Point(current.X + (float)n.X/2, current.Y + (float)n.Y/2);
    if(visited.Contains(f))
      continue;

    if(!loop.Contains(f))
    {
      toVisit.Enqueue(f);
      visited.Add(f);
    }
  }
}

Console.Clear();
Console.WriteLine(count);

struct Point
{
  public override int GetHashCode()
  {
    return X.GetHashCode() + Y.GetHashCode();
  }

  public override bool Equals([NotNullWhen(true)] object? obj)
  {
    if (obj == null) return false;
    return this == (Point)obj;
  }

  public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;
  public static bool operator !=(Point p1, Point p2) => p1.X != p2.X || p1.Y != p2.Y;

  public Point(float x, float y)
  {
    X = x; Y = y;
  }

  public float X, Y;
}