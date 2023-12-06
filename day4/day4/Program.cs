var input = File.ReadAllLines("in.txt")
  .Select(l => l.Split(": ")[1].Split(" | ")
    .Select(numbers => numbers.Split(" ", StringSplitOptions.RemoveEmptyEntries)
      .Select(n => int.Parse(n))).ToList())
  .Select((cards) => cards[0].Intersect(cards[1]).Count())
  .ToList();

// part 1
Console.WriteLine(input.Where(p => p> 0).Select(p => Math.Pow(2, p-1)).Sum());

// part 2
var cards = Enumerable.Repeat(1, input.Count()).ToArray();
for (int index = 0; index < cards.Length; index++)
{
  int card = input[index];
  for (int i = index+1; i <= index+card && i < cards.Length; i++)
    cards[i] += cards[index];
}

Console.WriteLine(cards.Sum());