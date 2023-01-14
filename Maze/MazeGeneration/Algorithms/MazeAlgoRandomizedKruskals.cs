

//namespace MazeAlgos
//{
//    public class MazeAlgoRandomizedKruskals : IMazeAlgorithm
//    {
//        Random rng;

//        public override MazeWallData ExecuteAlgorithm(WallState[,] maze, Vector2Int size, int seed)
//        {
//            // Initialize the maze with all walls.
//            for(int i = 0; i < size.x; i++)
//            {
//                for(int j = 0; j < size.y; j++)
//                {
//                    maze[i, j] = WallState.LEFT | WallState.RIGHT | WallState.UP | WallState.DOWN;
//                }
//            }

//            // Create a list of all the walls in the maze.
//            List<Wall> walls = new List<Wall>();
//            for(int i = 0; i < size.x; i++)
//            {
//                for(int j = 0; j < size.y; j++)
//                {
//                    // Do not add walls for cells on the edges of the maze.
//                    if(i > 0)
//                    {
//                        walls.Add(new Wall(new Vector2Int(i, j), new Vector2Int(i - 1, j), WallState.LEFT));
//                    }
//                    if(i < size.x - 1)
//                    {
//                        walls.Add(new Wall(new Vector2Int(i, j), new Vector2Int(i + 1, j), WallState.RIGHT));
//                    }
//                    if(j > 0)
//                    {
//                        walls.Add(new Wall(new Vector2Int(i, j), new Vector2Int(i, j - 1), WallState.UP));
//                    }
//                    if(j < size.y - 1)
//                    {
//                        walls.Add(new Wall(new Vector2Int(i, j), new Vector2Int(i, j + 1), WallState.DOWN));
//                    }
//                }
//            }
//            // Shuffle the walls.
//            walls.Shuffle(rng);

//            // Create a disjoint set data structure to track the connected components.
//            DisjointSet set = new DisjointSet(size.x * size.y);

//            // Iterate through the walls and remove the ones that do not create a cycle.
//            foreach(Wall wall in walls)
//            {
//                int root1 = set.Find(wall.Cell1.x * size.y + wall.Cell1.y);
//                int root2 = set.Find(wall.Cell2.x * size.y + wall.Cell2.y);
//                if(root1 != root2)
//                {
//                    set.Union(root1, root2);
//                    maze[wall.Cell1.x, wall.Cell1.y] &= ~wall.Direction;
//                    maze[wall.Cell2.x, wall.Cell2.y] &= ~GetOppositeWall(wall.Direction);
//                }
//            }

//            return maze;
//        }


//        // Helper function to get the opposite wall direction.
//        WallState GetOppositeWall(WallState direction)
//        {
//            if(direction == WallState.LEFT)
//            {
//                return WallState.RIGHT;
//            }
//            else if(direction == WallState.RIGHT)
//            {
//                return WallState.LEFT;
//            }
//            else if(direction == WallState.UP)
//            {
//                return WallState.DOWN;
//            }
//            else if(direction == WallState.DOWN)
//            {
//                return WallState.UP;
//            }
//            else
//            {
//                return direction;
//            }
//        }
//    }

//    // Wall class to represent a wall between two cells.
//    public class Wall
//    {
//        public Vector2Int Cell1;
//        public Vector2Int Cell2;
//        public WallState Direction;

//        public Wall(Vector2Int cell1, Vector2Int cell2, WallState direction)
//        {
//            Cell1 = cell1;
//            Cell2 = cell2;
//            Direction = direction;
//        }
//    }

//    // Disjoint set data structure to track the connected components.
//    public class DisjointSet
//    {
//        int[] parent;
//        int[] rank;

//        public DisjointSet(int size)
//        {
//            parent = new int[size];
//            rank = new int[size];
//            for(int i = 0; i < size; i++)
//            {
//                parent[i] = i;
//                rank[i] = 0;
//            }
//        }

//        public int Find(int x)
//        {
//            if(parent[x] != x)
//            {
//                parent[x] = Find(parent[x]);
//            }
//            return parent[x];
//        }

//        public void Union(int x, int y)
//        {
//            int rootX = Find(x);
//            int rootY = Find(y);
//            if(rootX != rootY)
//            {
//                if(rank[rootX] < rank[rootY])
//                {
//                    parent[rootX] = rootY;
//                }
//                else if(rank[rootY] < rank[rootX])
//                {
//                    parent[rootY] = rootX;
//                }
//                else
//                {
//                    parent[rootY] = rootX;
//                    rank[rootX]++;
//                }
//            }
//        }
//    }

//    // Extension method to shuffle a list.
//    public static class ExtensionMethods
//    {
//        public static void Shuffle<T>(this IList<T> list, Random rng)
//        {
//            int n = list.Count;
//            while(n > 1)
//            {
//                n--;
//                int k = rng.Next(n + 1);
//                T value = list[k];
//                list[k] = list[n];
//                list[n] = value;
//            }
//        }
//    }
//}