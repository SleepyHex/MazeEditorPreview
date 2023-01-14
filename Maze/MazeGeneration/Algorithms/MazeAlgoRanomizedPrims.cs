
//namespace MazeAlgos
//{
//    public class MazeAlgoRanomizedPrims : IMazeAlgorithm
//    {
//        public override WallState[,] ExecuteAlgorithm(WallState[,] maze, Vector2Int size, int seed)
//        {
//            // Initialize random number generator with given seed
//            var random = new System.Random(seed);

//            // Create list of available neighbors for each cell
//            var availableNeighbors = new List<Neighbor>[size.x, size.y];
//            for(int x = 0; x < size.x; x++)
//            {
//                for(int y = 0; y < size.y; y++)
//                {
//                    availableNeighbors[x, y] = new List<Neighbor>();

//                    // Add valid neighbors to the list
//                    if(x > 0)
//                        availableNeighbors[x, y].Add(new Neighbor(new Vector2Int(x - 1, y), WallState.RIGHT));
//                    if(x < size.x - 1)
//                        availableNeighbors[x, y].Add(new Neighbor(new Vector2Int(x + 1, y), WallState.LEFT));
//                    if(y > 0)
//                        availableNeighbors[x, y].Add(new Neighbor(new Vector2Int(x, y - 1), WallState.UP));
//                    if(y < size.y - 1)
//                        availableNeighbors[x, y].Add(new Neighbor(new Vector2Int(x, y + 1), WallState.DOWN));
//                }
//            }

//            // Start from a random cell
//            var currentCell = new Vector2Int(random.Next(size.x), random.Next(size.y));
//            maze[currentCell.x, currentCell.y] |= WallState.VISITED;

//            // Create list of visited cells
//            var visitedCells = new List<Vector2Int>();
//            visitedCells.Add(currentCell);

//            // Repeat until all cells have been visited
//            while(visitedCells.Count < size.x * size.y)
//            {
//                // Select a random unvisited neighbor
//                var neighbors = availableNeighbors[currentCell.x, currentCell.y];
//                var neighborIndex = random.Next(neighbors.Count);
//                var neighbor = neighbors[neighborIndex];

//                // Remove the wall between the current cell and the selected neighbor
//                maze[currentCell.x, currentCell.y] &= ~neighbor.SharedWall;
//                maze[neighbor.Position.x, neighbor.Position.y] &= ~GetOppositeWall(neighbor.SharedWall);

//                // Mark the neighbor as visited
//                maze[neighbor.Position.x, neighbor.Position.y] |= WallState.VISITED;
//                visitedCells.Add(neighbor.Position);

//                // Update the current cell to the selected neighbor
//                currentCell = neighbor.Position;
//            }

//            return maze;
//        }

//        WallState GetOppositeWall(WallState wall)
//        {
//            switch(wall)
//            {
//                case WallState.LEFT:
//                    return WallState.RIGHT;
//                case WallState.RIGHT:
//                    return WallState.LEFT;
//                case WallState.UP:
//                    return WallState.DOWN;
//                case WallState.DOWN:
//                    return WallState.UP;
//                default:
//                    throw new InvalidOperationException("Invalid wall state");
//            }
//        }
//    }
//}