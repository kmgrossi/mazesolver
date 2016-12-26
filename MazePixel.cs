namespace MazeImageSolver
{
    /// <summary>
    /// Determines whether a pixel is valid for traversal. 
    /// </summary>
    public enum PixelState
    {
        open = 0,
        closed = 1,        
    }

    /// <summary>
    /// This serves as the representation of one pixel of the passed in maze image.
    /// A pixel can be represented by its pixel position on the image, and for our purposes,
    /// it can be considered open, closes, or unassigned.
    /// </summary>
    public class MazePixel
    {
        //Setting row and column state of pixel to default position.
        int RowPixel = 0;
        int ColumnPixel = 0;

        //For a new pixel, assume it to be open. 
        PixelState CurrentPixelState = PixelState.open;

        /// <summary>
        /// Default constructor for a MazePixel
        /// </summary>
        public MazePixel()
        {
            RowPixel = 0;
            ColumnPixel = 0;
            CurrentPixelState = PixelState.open;         
        }

        /// <summary>
        /// Constructor that takes a row pixel position, column pixel position, and a pixel traversal state.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="state"></param>
        public MazePixel(int row, int column, PixelState state)
        {
            RowPixel = row;
            ColumnPixel = column;
            CurrentPixelState = state;
        }

        //Getter method for the pixel's row.
        public int getRow()
        {
            return RowPixel;
        }
           
        //Setter method for the pixel's row
        public void setRow(int row)
        {
            RowPixel = row;
        }

        //Getter method for the pixel's column.
        public int getCol()
        {
            return ColumnPixel;
        }

        //Setter method for pixel's column
        public void setCol(int column)
        {
            ColumnPixel = column;
        }

        //Getter method for the pixel's state.
        public PixelState getState()
        {
            return CurrentPixelState;
        }

        //Setter method for the pixel's state
        public void setState(PixelState ps)
        {
            this.CurrentPixelState = ps;           
        }

        /// <summary>
        /// Utility method used in solver. 
        /// Override for the Equals method and is used to check if another MazePixel is equal to another. 
        /// </summary>
        /// <param name="pixelObject"></param>
        /// <returns></returns>
        public override bool Equals(object pixelObject)
        {
            MazePixel mp = pixelObject as MazePixel;

            if(object.ReferenceEquals(mp, this))
            {
                return true;
            }

            if(pixelObject == null)
            {
                return false;
            }

            if(mp.ColumnPixel == this.ColumnPixel && 
                mp.RowPixel == this.RowPixel &&
                mp.CurrentPixelState == this.CurrentPixelState)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        /// <summary>
        /// Returns the hash code of a particular object instance. Can be used to see if two
        /// objects are the same according to their hash codes.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
