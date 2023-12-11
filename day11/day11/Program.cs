using System.Drawing;

var space = File.ReadAllLines("in.txt").Select(c => c.ToList()).ToList();

//const int kMultiplier = 2; // part1
const int kMultiplier = 1000000; // part2

var spaceWidth = space.Min(l => l.Count);
List<Point> galaxies = new List<Point>();
for (int i = 0; i < space.Count; i++)
  for (int j = 0; j < spaceWidth; j++)
    if (space[i][j] == '#')
      galaxies.Add(new Point(j, i));

List<(Point, Point)> galaxiesPairs = new List<(Point, Point)>();
for (int i = 0;i < galaxies.Count; i++)
  for(int j = i+1; j < galaxies.Count; j++)
    galaxiesPairs.Add((galaxies[i], galaxies[j]));

Console.WriteLine(galaxiesPairs.Sum(gp =>
{ 
  Point s = GetGaps(space, gp.Item1, gp.Item2);

  long toAdd = (s.X + s.Y) * kMultiplier;
  toAdd -= s.X + s.Y; 

  return Math.Abs(gp.Item1.X - gp.Item2.X)
       + Math.Abs(gp.Item1.Y - gp.Item2.Y) + toAdd;
  }));

Point GetGaps(List<List<char>> universe, Point p1, Point p2)
{
  int countX = 0, countY = 0;
  int minx = Math.Min(p1.X, p2.X);
  int maxx = Math.Max(p1.X, p2.X);

  for (int i = minx + 1; i < maxx; i++)
  { 
    var column = universe.Select(l => l[i]).ToArray();
    if (column.All(c => c =='.'))
      countX++;
  }

  int miny = Math.Min(p1.Y, p2.Y);
  int maxy = Math.Max(p1.Y, p2.Y);
  for (int i = miny + 1; i < maxy; i++)
    if (universe[i].All(c => c == '.'))
      countY++;

  return new Point(countX, countY);
}
