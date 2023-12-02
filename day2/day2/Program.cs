var maxLimits = new[] { "red", "green", "blue" };
const int limit = 12;

var games = File.ReadAllLines("in.txt")
  .Select(l => l.Split(": ")[1].Split("; ")
  .Select(subset =>
  {
    var cubes = subset.Split(", ");
    return maxLimits.Select(limit =>
    {
      var found = cubes.FirstOrDefault(c => c.EndsWith(limit));
      return found == null ? 0 : int.Parse(found.Split(" ").First());
    }).ToList();
  }).ToList()).ToList();

HashSet<int> invalidIndices = new HashSet<int>();

var powers = new List<int>();

foreach (var game in games)
{
  var max = new int[3];

  foreach (var subset in game)
  {
    for(var cubeIndex = 0; cubeIndex < 3; cubeIndex++)
    {
      if (subset[cubeIndex] > limit + cubeIndex)
        invalidIndices.Add(games.IndexOf(game));

      if(subset[cubeIndex] > max[cubeIndex])
        max[cubeIndex] = subset[cubeIndex];
    }
  }

  powers.Add(max.Aggregate(1, (s, n) => s * n));
}

Console.WriteLine(games.Count * (games.Count + 1) / 2 - invalidIndices.Sum() - invalidIndices.Count);
Console.WriteLine(powers.Sum());
