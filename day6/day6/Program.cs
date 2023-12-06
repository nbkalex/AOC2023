var lines = File.ReadAllLines("in.txt");

var input = lines.Select(l => l.Split(":")[1]
.Split(" ", StringSplitOptions.RemoveEmptyEntries)
.Select(v => int.Parse(v))).ToArray();

var input2 = lines.Select(l => long.Parse(l.Split(":")[1]
.Split(" ", StringSplitOptions.RemoveEmptyEntries)
.Aggregate("", (s,v) => s+v))).ToArray();


//var timeDists = input[0].Zip(input[1]).ToArray();
var timeDists = new []{ (First:input2[0], Second:input2[1]) };

long product = 1;
foreach(var td in timeDists)
{
	int count = 0;
	for (int i = 1; i < td.First; i++)
	{
		if(i * (td.First - i) > td.Second)
			count++;
	}

  product*= count;
}

Console.WriteLine(product);

// failed: 55656