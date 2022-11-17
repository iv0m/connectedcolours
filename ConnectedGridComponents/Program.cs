/// <summary>
/// IM - 17/11/2022
/// </summary>
public class GFG
{
    // Given input array of colours
    static char[,] matrix = new char[,]
        {
                { 'y', 'y', 'g', 'b' },
                { 'y', 'g', 'b', 'g' },
                { 'b', 'g', 'g', 'g' },
        };

    // Set size of rows/columns
    static int Rows = matrix.GetLength(0);
    static int Cols = matrix.GetLength(1);

    // This is a navigation structure (right/down/up/left)
    static int[] dx = { 0, 1, -1, 0 };
    static int[] dy = { 1, 0, 0, -1 };

    // stores information about which cell
    // are already visited in a particular BFS
    //static int[,] visited = new int[Rows, Cols];
    static Dictionary<char, int[,]> visited = new Dictionary<char, int[,]>();

    // Stores the count of cells in
    // the largest connected component
    static int COUNT;

    /// <summary>
    /// Hold coordinates of the 2 adjecent cells
    /// </summary>
    class pair
    {
        public int first, second;
        public pair(int first, int second)
        {
            this.first = first;
            this.second = second;
        }
    }

    /// <summary>
    /// Checks if a cell has valid coordinates and is been visited before
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="matrix"></param>
    /// <param name="colourKey"></param>
    /// <returns></returns>
    static bool is_valid(int x, int y, char[,] matrix, char colourKey)
    {
        if (x < Rows && y < Cols && x >= 0 && y >= 0)
        {
            if (!HasCellBeenVisited(x, y, colourKey)
                && matrix[x, y] == colourKey)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    // Map to count the frequency of
    // each connected component
    static Dictionary<char, int> mp = new Dictionary<char, int>();

    /// <summary>
    /// Checks if a cell for a particular colour has been visited
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="colourKey"></param>
    /// <returns></returns>
    static bool HasCellBeenVisited(int x, int y, char colourKey)
    {
        if (visited.Keys.Contains(colourKey))
        {
            var v = visited[colourKey];

            if (v[x, y] == 1)
                return true;
        }

        return false;
    }

    static void SetCellVisited(int x, int y, char colourKey)
    {
        if (visited.Keys.Contains(colourKey))
        {
            var v = visited[colourKey];
            v[x, y] = 1;
        }
    }

    // function to calculate the
    // largest connected component
    static void findComponentSize(char[,] matrix, char colourToSearch)
    {
        // Iterate over every cell
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                var currentColorKey = matrix[i, j];

                if(!HasCellBeenVisited(i, j, currentColorKey) && matrix[i, j] == colourToSearch)
                {
                    COUNT = 0;                    

                    // Stores the indices of the matrix cells
                    List<pair> q = new List<pair>();

                    // Mark the starting cell as visited
                    // and push it into the queue
                    q.Add(new pair(i, j));
                    SetCellVisited(i, j, colourToSearch);

                    // Iterate while the queue
                    // is not empty
                    while (q.Count > 0)
                    {
                        pair p = q[0];
                        q.RemoveAt(0);

                        int x = p.first;
                        int y = p.second;
                        COUNT++;

                        // Go to the adjacent cells
                        for (int k = 0; k < 4; k++)
                        {
                            int newX = x + dx[k];
                            int newY = y + dy[k];

                            if (is_valid(newX, newY, matrix, colourToSearch))
                            {
                                q.Add(new pair(newX, newY));
                                SetCellVisited(newX, newY, colourToSearch);
                            }
                        }
                    }

                    if (mp.ContainsKey(colourToSearch))
                    {
                        mp[colourToSearch] += COUNT;
                    }
                    else
                    {   if(COUNT > 1)                        
                            mp.Add(colourToSearch, COUNT);
                        else 
                            mp.Add(colourToSearch, 0);
                    }
                }
            }
        }
    }

    /**
     * This create a map of arrays. For each color key, initialises
     * an array of visited coordinates (all zeros)
     **/
    static void InitialiseVisitedMapArray(char[,] matrix)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                var currentColorKey = matrix[i, j];
                if (!visited.Keys.Contains(currentColorKey))
                {
                    var visitedArray = new int[Rows, Cols];

                    // Initialise the array with zeros (un-visited cell)
                    for (int x = 0; x < Rows; x++)
                        for (int y = 0; y < Cols; y++)
                            visitedArray[x, y] = 0;

                    visited.Add(currentColorKey, visitedArray);
                }
            }
        }
    }

    /// <summary>
    /// This method returns a list of unique color keys
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    static List<char> GetUniqueColorKeys(char[,] matrix)
    {
        var uniqueKeys = new List<char>();

        for (int x = 0; x < Rows; x++)
            for (int y = 0; y < Cols; y++)
                if (!uniqueKeys.Contains(matrix[x, y]))
                    uniqueKeys.Add(matrix[x, y]);

        return uniqueKeys;
    }

    /// <summary>
    /// Program Start
    /// </summary>
    public static void Main()
    {
        // Initialise the dictionary map that hold visited cell per colour
        InitialiseVisitedMapArray(matrix);

        // Count the frequency of each connected colour
        foreach(var colour in GetUniqueColorKeys(matrix))
            findComponentSize(matrix, colour) ;

        Console.WriteLine($"Max number of connected cell is {mp.Values.Max()}");
    }
}
