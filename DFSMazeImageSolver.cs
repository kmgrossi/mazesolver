using System.Collections.Generic;

namespace MazeImageSolver
{
    /// <summary>
    /// Solves the Maze using Depth First Search. The reason why I chose Depth First Search is
    /// because I can assume that the maze has a start point and an end point and that there is only one
    /// valid path between these two points. The correct path in the maze is naturally deep since
    /// the decision tree to reach the destination is going to the deepest branch in said decision tree.
    /// 
    /// It is important to create another representation of the Maze in this Solver Class since we do
    /// not want to alter the state of the provided Maze instance with this class's methods.
    /// </summary>
    class DFSMazeImageSolver
    {
        public enum NodeState
        {
            //states of a node during depth first traversal
            notVisited = 0,
            visited = 1,
            onStack = 2
        }
        /// <summary>
        /// Creates a 2D array of Nodes that have an x,y, and state attribute. Similar to our maze
        /// class, except now our enumerables are changed to clearly represent the states of nodes
        /// during the depth first traversal.
        /// </summary>
        class DFSNode
        {
            int rowPosition, colPosition = 0;
            DFSNode previousNode = null;            
            NodeState state = NodeState.notVisited; //default val is notvisited.

            //Constructor
            public DFSNode(int rowPixel, int colPixel, NodeState ns)
            {
                rowPosition = rowPixel;
                colPosition = colPixel;
                state = ns;                
            }

            public int getRowPosition()
            {
                return rowPosition;
            }

            public int getColPosition()
            {
                return colPosition;
            }

            public NodeState getState()
            {
                return state;
            }

            public void setState(NodeState ns)
            {
               this.state = ns;
            }

            public DFSNode getPrevNode()
            {
                return previousNode;
            }

            public void setPrevNode(DFSNode node)
            {
                this.previousNode = node;
            }
        }
        
        //Declare a 2d array of null values 
        DFSNode[,] nodeList = null;
        
        /// <summary>
        /// Iterates through the passed in iterable of MazePixels that represent the complete Maze instance
        /// </summary>
        public void setDFSNodeValues(Maze maze)
        {
            nodeList = new DFSNode[maze.getWidth(), maze.getHeight()];            
            IEnumerator<MazePixel> mazePixels = maze.getAllPixels(); 

            //Here we translate the enum states between maze pixels and dfs nodes.
            //A closed state means that this node is "visited", while an open maze Pixel translates
            //to an "unvisited node".
            while(mazePixels.MoveNext())
            {
                MazePixel mp = mazePixels.Current;

                //We only worry if a node is visited or not visited. 
                if(mp.getState() == PixelState.closed)
                {
                    nodeList[mp.getRow(), mp.getCol()] = new DFSNode(mp.getRow(), mp.getCol(), NodeState.visited);
                }
                //if a pixel is not considered a wall, then it has to be traversable territory
                else 
                {
                    nodeList[mp.getRow(), mp.getCol()] = new DFSNode(mp.getRow(), mp.getCol(), NodeState.notVisited);
                }              
            }
        }

        /// <summary>
        /// These following two methods explain the type of approach to the Maze solving problem.
        /// These methods only work because they both operate on the same x,y plane but are operating
        /// on different layers. Because of this we can switch between "layers" of the Maze to grab the
        /// current, relevant information from each instance. 
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        private DFSNode fromMazePixel(MazePixel mp)
        {
            return nodeList[mp.getRow(), mp.getCol()];
        }

        private MazePixel fromDFSNode(Maze m, DFSNode node)
        {
            return m.getPixel(node.getRowPosition(), node.getColPosition());
        }

        /// <summary>
        /// Given a maze, solve it using depth first traversal. Returns the path taken by the solver to reach
        /// the destination.
        /// </summary>
        /// <param name="m"></param>
        public IEnumerable<MazePixel> solveMaze(Maze m)
        {            
            Stack<DFSNode> nodeStack = new Stack<DFSNode>();
            DFSNode start = fromMazePixel(m.getStartingPixel());         
            
            start.setPrevNode(null);
            start.setState(NodeState.visited);
            nodeStack.Push(start);

            //While there remains univisted neighbors.
            while(nodeStack.Count > 0)
            {
                DFSNode currentNode = nodeStack.Pop();                
                MazePixel currentPixel = fromDFSNode(m, currentNode);

                //If the currentNode we are on corresponds to the end pixel of the Maze, we are done. 
                if (m.finished(currentPixel))
                {                    
                    IEnumerable<MazePixel> path = mazePath(m, currentNode);
                    return path;                 
                }                

                //This foreach will serve as our adjacency list. Reminder that our adjacency lists are at max size 4 since
                //I only considered non-diagonal movement. 
                foreach(MazePixel adjacentPixel in m.adjacentPixels(currentPixel))
                {                   
                        DFSNode adjacentNode = fromMazePixel(adjacentPixel);
                        if (adjacentNode.getState() == NodeState.notVisited )
                        {
                            adjacentNode.setState(NodeState.onStack);
                            nodeStack.Push(adjacentNode);
                            adjacentNode.setPrevNode(currentNode);
                        }              
                }
                //After all neighbors have been explored, set current node to visited. 
                currentNode.setState(NodeState.visited);                           
            }
            //No solution could be found.
            return null;
        }

        /// <summary>
        /// This will build the path of nodes used to reach the destination and then add them to a List.
        /// The size of the list depends on the given maze. 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<MazePixel> mazePath(Maze m, DFSNode end)
        {
            DFSNode currentNode = end;
            List<MazePixel> pathPixels = new List<MazePixel>();

            while(currentNode != null)
            {
                pathPixels.Add(fromDFSNode(m, currentNode));
                currentNode = currentNode.getPrevNode();
            }
            return pathPixels;
        }     

    }
}
