const string inputFile = @"../../../../input12.txt";

Console.WriteLine("Day 12 - Passage Pathing");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);
Dictionary<string, Node> nodeLookup = new Dictionary<string, Node>();

foreach (string line in lines)
{
    string[] nodes = line.Split('-');

    Node startingNode = nodeLookup.GetValueOrDefault(nodes[0]);
    Node endingNode = nodeLookup.GetValueOrDefault(nodes[1]);

    if (startingNode is null)
    {
        startingNode = new Node()
        {
            Name = nodes[0],
            IsSmall = !char.IsUpper(nodes[0][0])
        };
        nodeLookup[startingNode.Name] = startingNode;
    }

    if (endingNode is null)
    {
        endingNode = new Node()
        {
            Name = nodes[1],
            IsSmall = !char.IsUpper(nodes[1][0])
        };
        nodeLookup[endingNode.Name] = endingNode;
    }

    endingNode.ConnectedNodes.Add(startingNode);
    startingNode.ConnectedNodes.Add(endingNode);
}

Node startNode = nodeLookup["start"];
Node endNode = nodeLookup["end"];

List<string> validPaths = new List<string>();

FindAllPaths(new List<Node>() { startNode });

Console.WriteLine($"The answer is: {validPaths.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
validPaths.Clear();

FindAllPaths2(new List<Node>() { startNode }, false);

Console.WriteLine($"The answer is: {validPaths.Distinct().Count()}");

Console.WriteLine();
Console.ReadKey();


void FindAllPaths(List<Node> currentPath)
{
    Node currentNode = currentPath[^1];
    if (currentNode == endNode)
    {
        validPaths.Add(string.Join(',', currentPath.Select(x => x.Name)));
        return;
    }

    foreach(Node node in currentNode.ConnectedNodes
        .Where (x=>!x.IsSmall || !currentPath.Contains(x))
        .ToList())
    {
        currentPath.Add(node);
        FindAllPaths(currentPath);
        currentPath.RemoveAt(currentPath.Count - 1);
    }
}

void FindAllPaths2(List<Node> currentPath, bool usedRevist)
{
    Node currentNode = currentPath[^1];
    if (currentNode == endNode)
    {
        validPaths.Add(string.Join(',', currentPath.Select(x => x.Name)));
        return;
    }

    foreach (Node node in currentNode.ConnectedNodes.Where(x=> x != startNode))
    {
        if (node.IsSmall)
        {
            if (currentPath.Contains(node))
            {
                if (usedRevist)
                {
                    continue;
                }

                currentPath.Add(node);
                FindAllPaths2(currentPath, true);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
            else
            {
                currentPath.Add(node);
                FindAllPaths2(currentPath, usedRevist);
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
        else
        {
            currentPath.Add(node);
            FindAllPaths2(currentPath, usedRevist);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}

class Node
{
    public bool IsSmall { get; init; }
    public string Name { get; init; }
    public List<Node> ConnectedNodes { get; } = new List<Node>();
}
