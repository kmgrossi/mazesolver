using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeImageSolver
{
    /// <summary>
    /// Driver program that creates all 3 abstraction layers from the passed in maze image.
    /// It solves it using the DFS nodes representation, and then modifies the original image
    /// using the Bitmap representation.
    /// </summary>
    class MazeImageSolver
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                throw new ArgumentException("Must contain only 2 arguments.");
            }
            string fileInput = args[0]; //input image
            string fileOutput = args[1]; //output image

            //The following four lines of code represent all three abstractions used in my solution.
            //They each provide unique information about the same image
            Bitmap bmp = new Bitmap(fileInput);
            Maze maze = new Maze(fileInput, "Red", "Blue", "Black", "White");             
            DFSMazeImageSolver dfs = new DFSMazeImageSolver();
            dfs.setDFSNodeValues(maze);
            
            IEnumerable<MazePixel> pathPixels = dfs.solveMaze(maze);

            //Throw an exception if there is a null path. 
            if(pathPixels == null)
            {
                throw new Exception("A path could not be found.");
            }
            

            //Draws the green path taken to reach the destination. 
            foreach(MazePixel mp in pathPixels)
            {
                bmp.SetPixel(mp.getRow(), mp.getCol(), Color.Green);     
            }
            bmp.Save(fileOutput);
            Console.WriteLine("COMPLETED.");

        }
    }
}
