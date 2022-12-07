using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    enum NodeType
    {
        DIRECTORY, FILE
    }

    record Node
    {
        public NodeType Type;
        public string Name;
        public Node Parent;
        public List<Node> Children = new();
        public int FileSize;
        public int Depth;
        private int _cachedSize = -1;
        public int Size()
        {
            if (Type == NodeType.FILE) {
                return FileSize;
            } else if (Type == NodeType.DIRECTORY)
            {
                if (_cachedSize == -1)
                {
                    _cachedSize = Children.Sum(c => c.Size()); 
                }
                return _cachedSize;
            } else
            {
                return 0;
            }
        }

        public void AddFile(string name, int size)
        {
            this.Children.Add(new Node() { Type = NodeType.FILE, Name = name, FileSize = size, Depth = this.Depth + 1 });
        }
        public Node GetDirectory(string name)
        {
            return Children.Where(c => c.Type == NodeType.DIRECTORY && c.Name == name).First();
        }
        public Node CreateDirectory(string name)
        {
            var newDirectory = new Node() { Type = NodeType.DIRECTORY, Name = name, Parent = this, Depth = this.Depth + 1 };
            this.Children.Add(newDirectory);
            return newDirectory;
        }

    }


    internal class Day07 : BaseDay
    {
        Node rootNode;
        void PrintNode(Node n)
        {
            Console.Write(new String(' ', n.Depth * 2));
            Console.Write($"- {n.Name}");
            if (n.Type == NodeType.DIRECTORY)
            {
                Console.Write($" (dir size=[{n.Size()}])");
            } else
            {
                Console.Write($" (file, size={n.FileSize})");
            }
            Console.WriteLine();
            if (n.Type == NodeType.DIRECTORY) {
                foreach (var c in n.Children)
                {
                    PrintNode(c);
                }
            }
        }

        public Day07()
        {
            rootNode = new Node() { Type = NodeType.DIRECTORY, Name = "/" };
            Node currentDirectory = rootNode;
            foreach(var line in File.ReadAllLines(InputFilePath))
            {
                var parts = line.Split(" ");
                if (parts[0] == "$")
                {
                    /* handle terminal command */
                    switch(parts[1])
                    {
                        case "cd":
                            switch (parts[2])
                            {
                                case "..":
                                    currentDirectory = currentDirectory.Parent;
                                    break;
                                case "/":
                                    currentDirectory = rootNode;
                                    break;
                                default:
                                    currentDirectory = currentDirectory.GetDirectory(parts[2]);
                                    break;
                            }
                            break;
                        case "ls":
                            /* dont really need this? */
                            break;
                    }
                    continue;
                }

                if (parts[0] == "dir")
                {
                    currentDirectory.CreateDirectory(parts[1]);
                } else
                {
                    currentDirectory.AddFile(parts[1], int.Parse(parts[0]));
                }


            }
            //PrintNode(rootNode);
        }

        public long SumDirectoriesUnderSize(Node currentDirectory, int size)
        {
            long totalSizes = 0;
            foreach (var n in currentDirectory.Children)
            {
                if (n.Type == NodeType.FILE) { continue; }
                var directorySize = n.Size();
                if (directorySize <= 100000)
                {
                    totalSizes += n.Size();
                }

                totalSizes += SumDirectoriesUnderSize(n, size);
            }

            return totalSizes;
        }

        public override ValueTask<string> Solve_1()
        {
            long totalSizes = SumDirectoriesUnderSize(rootNode,100000);   

            return new(totalSizes.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            const int FS_SIZE = 70000000;
            var freeSpaceRequired = 30000000;
            var totalUsed = rootNode.Size();

            var freeTarget = freeSpaceRequired - (FS_SIZE - totalUsed);

            Stack<Node> directories = new();
            directories.Push(rootNode);
            long bestTargetSize = totalUsed;
            Node bestTarget = rootNode;

            while (directories.Count > 0)
            {
                var currentDirectory = directories.Pop();
                var currentSize = currentDirectory.Size();
                if (currentSize >= freeTarget && currentSize < bestTargetSize)
                {
                    bestTarget = currentDirectory;
                    bestTargetSize = currentSize;
                }

                foreach(var c in currentDirectory.Children.Where(c=>c.Type == NodeType.DIRECTORY))
                {
                    directories.Push(c);
                }
            }



            return new(bestTargetSize.ToString());
        }
    }
}
