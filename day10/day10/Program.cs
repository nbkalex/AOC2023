using System.Drawing;
using System.Linq;

Dictionary<char, (Point, Point)> pipeTypes = new Dictionary<char, (Point, Point)>()
{
  {'-', (new Point(1,0), new Point(-1,0)) },
  {'|', (new Point(0,1), new Point(0,-1)) },
  {'L', (new Point(0,-1), new Point(1, 0)) },
  {'J', (new Point(0, -1), new Point(-1, 0)) },
  {'7', (new Point(-1, 0), new Point(0,1)) },
  {'F', (new Point(1, 0), new Point(0,1)) },
  {'.', (new Point(10,10), new Point(10,10)) }
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
foreach(var n in neighbors)
{
  Point p = new Point(startPos.X + n.X, startPos.Y + n.Y);
  var nDir1 = pipeTypes[pipesMap[p]].Item1;
  var nDir2 = pipeTypes[pipesMap[p]].Item2;

  if (new Point(nDir1.X + p.X, nDir1.Y + p.Y) == startPos
   || new Point(nDir2.X + p.X, nDir2.Y + p.Y) == startPos)
  {
    next = p; 
    break;
  }
}

List<Point> loop = new List<Point>() { startPos, next };
var hash = loop.ToHashSet();
while (true)
{
  var val = pipesMap[next];
  var dir = pipeTypes[val];

  Point next1 = new Point(next.X + dir.Item1.X, next.Y + dir.Item1.Y);
  Point next2 = new Point(next.X + dir.Item2.X, next.Y + dir.Item2.Y);

  if(!hash.Contains(next1))
    next = next1;
  else
  { 
    if(!hash.Contains(next2))
      next = next2;
    else
      break;
  }

  loop.Add(next);
  hash.Add(next);
}

var notLoop = pipesMap.Keys.Where(m => !hash.Contains(m));

Console.WriteLine(loop.Count()/2);

const int DISPLAY_OFFSET = 20;

foreach (var m in pipesMap)
{
  Console.SetCursorPosition(DISPLAY_OFFSET + m.Key.X, DISPLAY_OFFSET + m.Key.Y);
  char display = m.Value;

  if (hash.Contains(m.Key))
    Console.ForegroundColor = ConsoleColor.Blue;
  Console.WriteLine(display);

  Console.ResetColor();
}
