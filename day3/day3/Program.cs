using System.Drawing;

Dictionary<Point, char> symbols = new Dictionary<Point, char>();
List<Part> parts = new List<Part>();

var input = File.ReadAllLines("in.txt");
int weight = input[0].Length;
int height = input.Length;

for (int i = 0; i < height; i++)
{
  for (int j = 0; j < weight; j++)
  {
    char c = input[i][j];

    if (char.IsDigit(c))
    {
      Part part = new Part() { X = j, Y = i, Data = c.ToString() };
      while (++j < weight && char.IsDigit(input[i][j]))
        part.Data += input[i][j];

      parts.Add(part);
      j--;
    }
    else if( c != '.')
        symbols.Add(new Point(j, i), c);

  }
}

var withSymbols = parts.Where(p => p.Neighbors.Any(n => symbols.ContainsKey(n))).ToArray();
Console.WriteLine(withSymbols.Sum(s => s.Value));

long ratioSum = symbols.Where(s => s.Value == '*')
                       .Select(s => parts.Where(p => p.Neighbors.Contains(s.Key)))
                       .Where(sp => sp.Count() > 1)
                       .Sum(symbolParts => symbolParts.Aggregate(1, (s,v) => s*v.Value));         

Console.WriteLine(ratioSum);

class Part
{
  public int X { get; set; }
  public int Y { get; set; }
  public int X2 { get { return X + Data.Length - 1; } }
  public string Data { get; set; } = "";
  public int Value { get { return int.Parse(Data); } }
  public List<Point> Neighbors
  {
    get
    {
      List<Point> result = new List<Point>(){
          new Point(X-1, Y+1),
          new Point(X-1, Y-1),
          new Point(X2+1, Y-1),
          new Point(X2+1, Y+1),
          new Point(X2+1, Y),
          new Point(X-1, Y),
      };

      for (int i = 0; i < Data.Length; i++)
      {
        result.Add(new Point(X + i, Y - 1));
        result.Add(new Point(X + i, Y + 1));
      }
      return result;
    }
  }
}
