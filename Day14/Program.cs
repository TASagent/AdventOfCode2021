const string inputFile = @"../../../../input14.txt";


Console.WriteLine("Day 14 - Extended Polymerization");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputFileLines = File.ReadAllLines(inputFile);

Dictionary<(char, char), char> insertionLookup = new Dictionary<(char, char), char>();

for (int i = 2; i < inputFileLines.Length; i++)
{
    insertionLookup[(inputFileLines[i][0], inputFileLines[i][1])] = inputFileLines[i][6];
}

LinkedList<char> sequence = new LinkedList<char>(inputFileLines[0]);

for (int i = 0; i < 10; i++)
{
    LinkedListNode<char> nextNode = sequence.Last.Previous;

    while (nextNode is not null)
    {
        sequence.AddAfter(nextNode, insertionLookup[(nextNode.Value, nextNode.Next.Value)]);
        nextNode = nextNode.Previous;
    }
}

List<int> elementCounts = sequence
    .Distinct()
    .Select(x => sequence.Count(c => c == x))
    .ToList();



Console.WriteLine($"The answer is: {elementCounts.Max() - elementCounts.Min()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

/* NNCB
 * NN - NC - CB
 * NCN - NBC - CHB
 * NC - CN - NB - BC - CH - HB
 * 
 * CH -> CB, BH
 * HH -> N
 * CB -> H
 * NH -> C
 * HB -> C
 * HC -> B
 * HN -> C
 * NN -> NC, CN
 * BH -> H
 * NC -> B
 * NB -> B
 * BN -> B
 * BB -> N
 * BC -> B
 * CC -> N
 * CN -> C
*/

Dictionary<string, long> priorSegmentCounts = new Dictionary<string, long>();
Dictionary<string, long> nextSegmentCounts = new Dictionary<string, long>();

Dictionary<string, (string, string)> replacementLookup =
    new Dictionary<string, (string, string)>();

for (int i = 2; i < inputFileLines.Length; i++)
{
    char c1 = inputFileLines[i][0];
    char c2 = inputFileLines[i][1];

    char cNew = inputFileLines[i][6];

    replacementLookup[$"{c1}{c2}"] = ($"{c1}{cNew}", $"{cNew}{c2}");

}

string initialSequence = inputFileLines[0];

for (int i = 0; i < initialSequence.Length - 1; i++)
{
    string segment = initialSequence[i..(i + 2)];
    priorSegmentCounts[segment] =
        priorSegmentCounts.GetValueOrDefault(segment) + 1;
}


for (int step = 0; step < 40; step++)
{
    nextSegmentCounts.Clear();

    foreach (string segment in priorSegmentCounts.Keys)
    {
        (string newSegmentA, string newSegmentB) = replacementLookup[segment];

        nextSegmentCounts[newSegmentA] =
            nextSegmentCounts.GetValueOrDefault(newSegmentA) +
            priorSegmentCounts[segment];

        nextSegmentCounts[newSegmentB] =
            nextSegmentCounts.GetValueOrDefault(newSegmentB) +
            priorSegmentCounts[segment];
    }

    (priorSegmentCounts, nextSegmentCounts) = (nextSegmentCounts, priorSegmentCounts);
}

Dictionary<char, long> characterCount = new Dictionary<char, long>();

characterCount[initialSequence[0]] = 1;
characterCount[initialSequence[^1]] = 1;

foreach (var kvp in priorSegmentCounts)
{
    characterCount[kvp.Key[0]] =
        characterCount.GetValueOrDefault(kvp.Key[0]) + kvp.Value;

    characterCount[kvp.Key[1]] =
        characterCount.GetValueOrDefault(kvp.Key[1]) + kvp.Value;
}



Console.WriteLine($"The answer is: {(characterCount.Values.Max() - characterCount.Values.Min()) / 2}");

Console.WriteLine();
Console.ReadKey();
