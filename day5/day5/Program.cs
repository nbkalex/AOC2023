var input = File.ReadAllLines("in.txt");

List<long[]>[] maps = new List<long[]>[7];
for (long i = 0; i < 7; i++)
  maps[i]= new List<long[]>();

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

List<long> seeds2 = new List<long>();
for (int i = 0; i < seeds.Length; i+=2)
  for(int j = 0; j <= seeds[i+1]; j++)
    seeds2.Add(seeds[i] + j);

var values = GetValues(seeds, maps, 0);
var values2= GetValues(seeds2.ToArray(), maps, 0);
Console.WriteLine(values.Aggregate("", (s, v) => s + v + " "));
Console.WriteLine(values.Min());
Console.WriteLine(values2.Min());

long[] GetValues(long[] input, List<long[]>[] maps, long mapIndex)
{
  if (mapIndex == maps.Length)
    return input;

  var map = maps[mapIndex];
  return GetValues(input.Select(i =>
  {
    var found = map.FirstOrDefault(m => i >= m[1] && i <= m[1] + m[2]);
    return found != null ? found[0] + i - found[1] : i;
  }).ToArray(), maps, ++mapIndex);
}
