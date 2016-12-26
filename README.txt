DESIGN CHOICES
================

1. 	I used Depth First Search since I assumed that there was only one valid path between the start and end point. 
	My thought process was that if you were to construct a decision tree or a graph out of the Maze image, then you 
	would find that the longest/deepest branch of the tree would be the solution to the Maze. 

2. 	I chose to represent the Maze image through three abstractions
		a.	First Layer - 	Bitmap -> Used to retrieve the colors of the original maze image. 

		b.	Second Layer - 	Maze class composed of MazePixels -> Upon retrieving the colors of the maze through the Bitmap,
					we can classift certains pixels as open and closed.

		c. 	Third Layer - 	DFSNodes - To avoid altering the constructed Maze, I created another layer which represented each 
					MazePixel as a Node/Vertex for use in Depth First Search. The attributes on this layer consisted
					of classifying nodes as visited, unvisited, and the ability to keep track of previous nodes.

3. 	I used 4 way movement for this solution as for some cases when traversing through the maze, the path would go through walls when 
	a diagonal pixel was considered open while the pixels near that open pixel were considered close and should act like a wall. This adds
	some performance overhead since going through a maze with only 4 movements available is going to be slower that going through a maze with 8 way movement.
	However, I would rather the solver be 100% accurate and sacrifice 2-3 seconds of performance on worst cases.



