using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace MazeImageSolver
{
    /// <summary>
    /// This class constructs a data structure of Maze Pixels into a 2D array of maze pixels,
    /// representing the final, traversable Maze.  
    /// </summary> 
    class Maze
    {
        MazePixel start = null;
        MazePixel end = null;
        MazePixel[,] mazeImage = null;

        int width, height = 0;

        //Getter method for Maze's pixel width
        public int getWidth()
        {
            return width;
        }

        //Getter method for Maze's height
        public int getHeight()
        {
            return height;
        }

        //Getter method for Maze's start Pixel
        public MazePixel getStartingPixel()
        {
            return start;
        }

        //Getter method for Maze's end pixel
        public MazePixel getEndPixel()
        {
            return end;
        }

        //Getter method for a particular MazePixel in the Maze. 
        public MazePixel getPixel(int rowPix, int colPix)
        {
            if(rowPix < 0 || colPix < 0 || rowPix >= width || colPix >= height)
            {
                throw new Exception("Invalid position parameters for getPixel.");
            }

            return mazeImage[rowPix, colPix];
        }

        /// <summary>
        /// Constructor for Maze which uses a Bitmap to initially represent the image, and then iterates
        /// through the Bitmap to create our own data structure of MazePixels.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="wallColor"></param>
        /// <param name="openPixelColor"></param>
        public Maze(string filename, string startColor, string endColor, string wallColor, string openPixelColor)
        {
            //Check to see for a valid file
            if(!File.Exists(filename))
            {
                throw new FileNotFoundException("The file you passed in is not valid.");
            }
            
            Bitmap mazeBitImage = null;

            //From Microsoft Developer Network: Bitmap can only support images of the
            //following format: BMP, GIF, EXIF, JPG, PNG and TIFF. We throw an exception
            //if the passed in file is not a valid file type.
            try
            {
                mazeBitImage = new Bitmap(filename);
            }
            catch(Exception)
            {
                throw new ArgumentException("Passed in image is not supported.");
            }

            mazeImage = new MazePixel[mazeBitImage.Width, mazeBitImage.Height];
            height = mazeBitImage.Height;
            width = mazeBitImage.Width;

            //Traverse through the Bitmap and grab each pixel and then attach additional
            //information about each pixel to the 2D array of MazePixels.
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Color bitmapPixelColor = mazeBitImage.GetPixel(x, y);

                    switch(bitmapPixelColor.Name)
                    {
                        case "ffff0000": //red
                            mazeImage[x, y] = new MazePixel(x, y, PixelState.open);
                            start = mazeImage[x, y];
                            break;

                        case "ff0000ff": //blue
                            mazeImage[x, y] = new MazePixel(x, y, PixelState.open);
                            end = mazeImage[x, y];
                            break;

                        case "ff000000": //black
                            mazeImage[x, y] = new MazePixel(x, y, PixelState.closed);                            
                            break;

                        case "ffffffff": //white
                            mazeImage[x, y] = new MazePixel(x, y, PixelState.open);
                            break;

                        default:
                            throw new Exception("Invalid Color in picture found.");                     
                    }
                }
            }
        }

        /// <summary>
        /// Method that lets the solver know if the current pixel is the end pixel.s
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        public bool finished(MazePixel mp)
        {
            return mp.Equals(end);
        }

        /// <summary>
        /// Returns the adjacent pixels surrounding the passed in pixel. This does not include
        /// diagonal pixels as there is a specific case where the traversal might pass through walls
        /// if the diagonal pixel is considered open. This means that the max capacity is 4 and the least 
        /// capacity is 2 (when the pixel is in the corner). I know that there should be a way to incorporate
        /// 8 way movement, but this should suffice and it cuts the amount of space allocated on the average case
        /// anyway (albeit this performance increase isn't a big deal). 
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        public MazePixel[] adjacentPixels(MazePixel mp)
        {
            int rowPix = mp.getRow();
            int colPix = mp.getCol();

            MazePixel[] adjPix = new MazePixel[4];
            int count = 0;

            for (int x = rowPix - 1; x <= rowPix + 1; x++)
            {
                for (int y = colPix - 1; y <= colPix + 1; y++)
                {
                    if (((x == mp.getRow() && y != mp.getCol()) || (x != mp.getRow() && y == mp.getCol())))
                    {
                        adjPix[count] = mazeImage[x, y];
                        count++;
                    }
                }
            }
            return adjPix;
        }

        /// <summary>
        /// Allows us to return all the pixels of the maze one at a time. Will be using this
        /// to construct another representation for the solver by iterating through each MazePixel
        /// one at a time to construct a mirroring DFS nodes. 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MazePixel> getAllPixels()
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    yield return mazeImage[x, y];
                }
            }
        }
    }
}
