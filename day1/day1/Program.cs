using System.Threading;

var spelledNumbers = new[] {"padding", "1",   "2",   "3",     "4",    "5",    "6",   "7",     "8",     "9",
                            "padding", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var lines = File.ReadAllLines("in.txt");

var numbers1 = lines.Select(l =>
          l.First(c => char.IsDigit(c)) * 10 +
          l.Last(c => char.IsDigit(c)));

var numbers2 = lines.Select((l,i) =>
{ 
 var indicesLR = spelledNumbers.Select((sn,i) => (l.IndexOf(sn), i))
                             .Where(indices => indices.Item1 != -1)
                             .OrderBy(index => index.Item1)
                             .ToArray();

  var indicesRL = spelledNumbers.Select((sn, i) => (l.LastIndexOf(sn), i))
                             .Where(indices => indices.Item1 != -1)
                             .OrderByDescending(index => index.Item1)
                             .ToArray();

  Console.WriteLine($"{i+1}: {indicesLR.First().i % 10 } {indicesRL.First().i % 10} - {l}");
  return indicesLR.First().i % 10 * 10 + indicesRL.First().i % 10;
});

Console.WriteLine(numbers1.Sum());
Console.WriteLine(numbers2.Sum());

// failed: 55656