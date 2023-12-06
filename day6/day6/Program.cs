var lines = File.ReadAllLines("in.txt");

var input = lines.Select(l => l.Split(":")[1]
.Split(" ", StringSplitOptions.RemoveEmptyEntries)
.Select(v => long.Parse(v))).ToArray();

var input2 = lines.Select(l => long.Parse(l.Split(":")[1]
.Split(" ", StringSplitOptions.RemoveEmptyEntries)
.Aggregate("", (s, v) => s + v))).ToArray();


var timeDists = input[0].Zip(input[1]).ToArray();
var timeDists2 = new[] { (First: input2[0], Second: input2[1]) };

Console.WriteLine(Solution(timeDists));
Console.WriteLine(Solution(timeDists2));

long Solution((long First, long Second)[] aValues)
{
  return aValues.Aggregate((long)1, (s, v) => s * Enumerable.Range(1, (int)v.First - 2).Count(i => i * (v.First - i) > v.Second));
}