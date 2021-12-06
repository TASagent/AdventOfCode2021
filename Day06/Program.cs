const string inputFile = @"../../../../input06.txt";


Console.WriteLine("Day 06 - Lanternfish ");
Console.WriteLine("Star 1");
Console.WriteLine();


List<int> ages = File.ReadAllText(inputFile)
    .Split(',')
    .Select(int.Parse)
    .ToList();

long newborns = 0;
long almostReadies = 0;
int dayIndex = 0;

long[] ageBins = new long[7];

for (int i = 0; i < 7; i++)
{
    ageBins[i] = ages.Count(x => x == i);
}
almostReadies = ages.Count(x => x == 7);
newborns = ages.Count(x => x == 8);

for (int i = 0; i < 80; i++)
{
    long currentFish = ageBins[dayIndex];
    ageBins[dayIndex] += almostReadies;
    almostReadies = newborns;
    newborns = currentFish;

    dayIndex = (dayIndex + 1) % 7;
}

Console.WriteLine($"The answer is: {ageBins.Sum() + newborns + almostReadies}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

for (int i = 80; i < 256; i++)
{
    long currentFish = ageBins[dayIndex];
    ageBins[dayIndex] += almostReadies;
    almostReadies = newborns;
    newborns = currentFish;

    dayIndex = (dayIndex + 1) % 7;
}

Console.WriteLine($"The answer is: {ageBins.Sum() + newborns + almostReadies}");



Console.WriteLine();
Console.ReadKey();
