var input = File.ReadAllLines("in.txt");

List<long[]>[] maps = new List<long[]>[7];
for (long i = 0; i < 7; i++)
  maps[i] = new List<long[]>();

var seeds = input[0].Split(": ")[1].Split(" ").Select(s => long.Parse(s)).ToArray();

long currentMapIndex = -1;
for (long i = 1; i < input.Length; i++)
{
  if (!input[i].Any())
  {
    currentMapIndex++;
    i++;
    continue;
  }

  maps[currentMapIndex].Add(input[i].Split(" ").Select(s => long.Parse(s)).ToArray());
}

List<(long, long)> seeds2 = new List<(long, long)>();
for (int i = 0; i < seeds.Length; i += 2)
  seeds2.Add((seeds[i], seeds[i + 1]));

var values = GetValues(seeds, maps, 0);
var values2 = GetValues2(seeds2.ToArray(), maps, 0);

Console.WriteLine(values.Aggregate("", (s, v) => s + v + " "));
Console.WriteLine(values.Min());
Console.WriteLine(values2.Min(v => v.Item1));

long[] GetValues(long[] input, List<long[]>[] maps, long mapIndex)
{
  Console.WriteLine(input.Aggregate("", (s,v) => s + v + " "));

  if (mapIndex == maps.Length)
    return input;

  var map = maps[mapIndex];

  return GetValues(input.Select(i =>
  {
    var found = map.FirstOrDefault(m => i >= m[1] && i <= m[1] + m[2]);
    return found != null ? found[0] + i - found[1] : i;
  }).ToArray(), maps, ++mapIndex);
}

(long, long)[] GetValues2((long, long)[] input, List<long[]>[] maps, long mapIndex)
{
  if (mapIndex == maps.Length)
    return input;

  var map = maps[mapIndex];

  List<(long, long)> ranges = new List<(long, long)>();
  foreach (var range in input)
  {
    var intersections = map.Select(m => (m, Intersect((m[1], m[2]), range)))
                           .Where(m => m.Item2 != null)
                           .ToList();
    foreach (var intersection in intersections)
    {
      var m = intersection.m;
      var intr = intersection.Item2;
      ranges.Add((m[0] + intr.Value.Item1 - m[1], intr.Value.Item2));
    }

    if (intersections.Any())
    {
      intersections = intersections.OrderBy(i => i.Item2.Value.Item1).ToList();
      intersections.Insert(0,(null, (range.Item1 - 1, 1)));
      intersections.Add((null, (range.Item1 + range.Item2 + 1, 1)));
    }
    else
      ranges.Add(range);

    for (var i = 0; i < intersections.Count - 1; i++)
    {
      long left1 = intersections[i].Item2.Value.Item1;
      long right1 = intersections[i].Item2.Value.Item1 + intersections[i].Item2.Value.Item2;

      long left2 = intersections[i + 1].Item2.Value.Item1;
      long right2 = intersections[i + 1].Item2.Value.Item1 + intersections[i + 1].Item2.Value.Item2;

      if (right1 + 1 < left2)
        ranges.Add((right1, left2 - right1-1));
    }

  }


  return GetValues2(ranges.ToArray(), maps, ++mapIndex);
}
 
(long, long)? Intersect((long, long) range1, (long, long) range2)
{
  long left1 = range1.Item1;
  long right1 = range1.Item1 + range1.Item2;

  long left2 = range2.Item1;
  long right2 = range2.Item1 + range2.Item2;

  if (right1 < left2 || left1 > right2)
    return null;
  else
    return (Math.Max(left1, left2), Math.Min(right1, right2) - Math.Max(left1, left2));
}