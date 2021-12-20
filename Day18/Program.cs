
const string inputFile = @"../../../../input18.txt";


Console.WriteLine("Day 18 - Snailfish");
Console.WriteLine("Star 1");
Console.WriteLine();

int output1 = 0;

//To add two Snailfish numbers - Concat and Reduce
string[] inputLines = File.ReadAllLines(inputFile);

List<TreePair> nodes = inputLines.Select(ParseRoot).ToList();

TreePair sum = nodes[0];

for (int i = 1; i < nodes.Count; i++)
{
    //Concat
    sum = new TreePair(null)
    {
        Left = sum,
        Right = nodes[i]
    };
    sum.Left.Parent = sum;
    sum.Right.Parent = sum;

    //Simplify
    while (true)
    {
        if (sum.FindFirstExplosionOrDefault() is TreePair explodingPair)
        {
            explodingPair.Explode();
            continue;
        }

        if (sum.FindFirstSplitOrDefault() is TreeValue splittingPair)
        {
            splittingPair.Split();
            continue;
        }

        break;
    }
}

Console.WriteLine($"The answer is: {sum.GetMagnitude()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
long maxMagnitude = 0;

for (int i = 0; i < nodes.Count - 1; i++)
{
    for (int j = i + 1; j < nodes.Count; j++)
    {
        nodes = inputLines.Select(ParseRoot).ToList();

        sum = new TreePair(null)
        {
            Left = nodes[i],
            Right = nodes[j]
        };
        sum.Left.Parent = sum;
        sum.Right.Parent = sum;

        //Simplify
        while (true)
        {
            if (sum.FindFirstExplosionOrDefault() is TreePair explodingPair)
            {
                explodingPair.Explode();
                continue;
            }

            if (sum.FindFirstSplitOrDefault() is TreeValue splittingPair)
            {
                splittingPair.Split();
                continue;
            }

            break;
        }

        maxMagnitude = Math.Max(maxMagnitude, sum.GetMagnitude());

        nodes = inputLines.Select(ParseRoot).ToList();

        sum = new TreePair(null)
        {
            Left = nodes[j],
            Right = nodes[i]
        };
        sum.Left.Parent = sum;
        sum.Right.Parent = sum;

        //Simplify
        while (true)
        {
            if (sum.FindFirstExplosionOrDefault() is TreePair explodingPair)
            {
                explodingPair.Explode();
                continue;
            }

            if (sum.FindFirstSplitOrDefault() is TreeValue splittingPair)
            {
                splittingPair.Split();
                continue;
            }

            break;
        }

        maxMagnitude = Math.Max(maxMagnitude, sum.GetMagnitude());
    }
}

Console.WriteLine($"The answer is: {maxMagnitude}");

Console.WriteLine();
Console.ReadKey();

TreePair ParseRoot(string inputString)
{
    int index = 0;

    return Parse(null, inputString, ref index) as TreePair;
}

TreeNode Parse(TreePair parent, string inputString, ref int index)
{
    if (inputString[index] == '[')
    {
        //Parse TreePair
        TreePair newPair = new TreePair(parent);
        index++;

        newPair.Left = Parse(newPair, inputString, ref index);

        if (inputString[index++] != ',')
        {
            throw new Exception();
        }

        newPair.Right = Parse(newPair, inputString, ref index);

        if (inputString[index++] != ']')
        {
            throw new Exception();
        }

        return newPair;
    }
    else
    {
        //Parse TreeValue
        return new TreeValue(parent)
        {
            Value = (inputString[index++] - '0')
        };
    }
}

abstract class TreeNode
{
    public TreePair Parent { get; set; }

    public TreeNode(TreePair parent) => Parent = parent;

    public abstract long GetMagnitude();
}

class TreeValue : TreeNode
{
    public int Value { get; set; }

    public TreeValue(TreePair parent) : base(parent) { }

    public bool ShouldSplit() => Value >= 10;

    public void Split()
    {
        TreePair newChild = new TreePair(Parent);
        newChild.Left = new TreeValue(newChild) { Value = Value / 2 };
        newChild.Right = new TreeValue(newChild) { Value = (Value + 1) / 2 };

        Parent.ReplaceChild(
            oldChild: this,
            newChild: newChild);
    }

    public override long GetMagnitude() => Value;
}

class TreePair : TreeNode
{
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }


    public TreePair(TreePair parent)
        : base(parent) { }

    public int GetDepth()
    {
        TreePair parent = this;
        int depth = 0;

        while (parent.Parent != null)
        {
            parent = parent.Parent;
            depth++;
        }

        return depth;
    }

    public void ReplaceChild(TreeNode oldChild, TreeNode newChild)
    {
        if (Left == oldChild)
        {
            Left = newChild;
        }
        else
        {
            Right = newChild;
        }
    }

    public bool ShouldExplode() => GetDepth() >= 4;

    public TreePair FindFirstExplosionOrDefault()
    {
        if (ShouldExplode())
        {
            return this;
        }

        switch (Left)
        {
            case TreePair leftPair:
                {
                    if (leftPair.FindFirstExplosionOrDefault() is TreePair leftMatch)
                    {
                        return leftMatch;
                    }
                    break;
                }

            default:
                break;
        }

        switch (Right)
        {
            case TreePair rightPair:
                {
                    if (rightPair.FindFirstExplosionOrDefault() is TreePair rightMatch)
                    {
                        return rightMatch;
                    }
                    break;
                }

            default:
                break;
        }

        return null;
    }

    public TreeValue FindFirstSplitOrDefault()
    {
        switch (Left)
        {
            case TreePair leftPair:
                {
                    TreeValue leftMatch = leftPair.FindFirstSplitOrDefault();
                    if (leftMatch is not null)
                    {
                        return leftMatch;
                    }
                    break;
                }

            case TreeValue leftValue:
                {
                    if (leftValue.ShouldSplit())
                    {
                        return leftValue;
                    }
                    break;
                }
        }

        switch (Right)
        {
            case TreePair rightPair:
                {
                    TreeValue rightMatch = rightPair.FindFirstSplitOrDefault();
                    if (rightMatch is not null)
                    {
                        return rightMatch;
                    }
                    break;
                }

            case TreeValue rightValue:
                {
                    if (rightValue.ShouldSplit())
                    {
                        return rightValue;
                    }
                    break;
                }
        }

        return null;
    }

    public void Explode()
    {
        //Get the LefterValue from:
        //  Traveling up until you arrive from the right node,
        //  Travel down left node
        //  Travel down right node until hitting a value
        TreeValue lefterValue = null;

        TreeNode lastNode = this;
        TreePair parentNode = Parent;

        while (true)
        {
            if (parentNode is null || parentNode.Right == lastNode)
            {
                break;
            }

            lastNode = parentNode;
            parentNode = lastNode.Parent;
        }

        if (parentNode is not null)
        {
            lastNode = parentNode.Left;
            while (lastNode is TreePair lastTreePair)
            {
                lastNode = lastTreePair.Right;
            }

            lefterValue = lastNode as TreeValue;
        }

        if (lefterValue is not null)
        {
            lefterValue.Value += (this.Left as TreeValue).Value;
        }

        //Handle Right
        TreeValue righterValue = null;
        lastNode = this;
        parentNode = Parent;


        while (true)
        {
            if (parentNode is null || parentNode.Left == lastNode)
            {
                break;
            }

            lastNode = parentNode;
            parentNode = lastNode.Parent;
        }

        if (parentNode is not null)
        {
            lastNode = parentNode.Right;
            while (lastNode is TreePair lastTreePair)
            {
                lastNode = lastTreePair.Left;
            }

            righterValue = lastNode as TreeValue;
        }

        if (righterValue is not null)
        {
            righterValue.Value += (this.Right as TreeValue).Value;
        }

        Parent.ReplaceChild(this, new TreeValue(Parent) { Value = 0 });
    }

    public override long GetMagnitude() =>
        3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
}

