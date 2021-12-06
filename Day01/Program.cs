
const string inputFile = @"../../../../input01.txt";


Console.WriteLine("Day 01 - Sonar Sweep");
Console.WriteLine("Star 1");
Console.WriteLine();

var lines = File.ReadAllLines(inputFile)
    .Select(int.Parse)
    .ToList();

int lastValue = int.MaxValue;
int incCount = 0;
foreach (int value in lines)
{
    if (value > lastValue)
    {
        incCount++;
    }
    lastValue = value;
}

Console.WriteLine($"The answer is: {incCount}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

int incCount2 = 0;
for (int i = 0; i < lines.Count - 3; i++)
{
    if (lines[i] < lines[i + 3])
    {
        incCount2++;
    }
}


Console.WriteLine($"The answer is: {incCount2}");

Console.WriteLine();
Console.ReadKey();
