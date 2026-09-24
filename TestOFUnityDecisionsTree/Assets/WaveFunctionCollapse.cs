using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    public Texture2D inputImage;
    public int N = 2;
    public int outputWidth;
    public int outputHeight;
    public Texture2D outputImage;
    private GridCell[,] outputGrid;
    private Dictionary<string, Pattern> patterns;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        // WFC algorithm

        // Load the input image
        var inputPixels = inputImage.GetPixels();
        var width = inputImage.width;
        var height = inputImage.height;

        // Extract patterns from the input image
        patterns = ExtractPatterns(inputPixels, width, height, N);

        // Create the empty wave/grid
        InitializeOutputGrid(outputWidth, outputHeight, patterns);

        // Run WFC algorithm
        CollapseGrid();

        // Generate the output image
        GenerateOutputImage();

        // Apply the texture to the plane
        renderer.material.mainTexture = outputImage;
    }

    /// <summary>
    ///     Builds the patterns to use as tiles
    /// </summary>
    /// <param name="pixels">The sample image pixels</param>
    /// <param name="width">width of the image</param>
    /// <param name="height">height of the image</param>
    /// <param name="size">The N value that controls how large (NxN) the pattern is</param>
    /// <returns>Returns the patterns as a dictionary using a key built by the pixel colors</returns>
    private Dictionary<string, Pattern> ExtractPatterns(Color[] pixels, int width, int height, int size)
    {
        var patterns = new Dictionary<string, Pattern>();

        for (var y = 0; y <= height - size; y++)
        for (var x = 0; x <= width - size; x++)
        {
            var patternPixels = new Color[size * size];
            for (var dy = 0; dy < size; dy++)
            for (var dx = 0; dx < size; dx++)
                patternPixels[dy * size + dx] = pixels[(y + dy) * width + x + dx];

            var patternKey = PatternToString(patternPixels);
            if (patterns.ContainsKey(patternKey))
                patterns[patternKey].frequency++;
            else
                patterns[patternKey] = new Pattern(patternPixels, size);
        }

        return patterns;
    }

    /// <summary>
    ///     Translates a pattern (array of colors) into a string to use as a key for the dictionary
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    private string PatternToString(Color[] pattern)
    {
        var sb = new StringBuilder();
        foreach (var color in pattern)
            sb.Append(color.r).Append(",").Append(color.g).Append(",").Append(color.b).Append(";");
        return sb.ToString();
    }

    /// <summary>
    ///     Prepares the output grid for the start of the algorithm, setting all cells with a list of all the possible patterns
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="patterns"></param>
    private void InitializeOutputGrid(int width, int height, Dictionary<string, Pattern> patterns)
    {
        outputGrid = new GridCell[width, height];
        
        for (int y = 0; y< height; y++)
        {
            for (int x = 0; x< width; x++)
            {
                outputGrid[x, y] = new GridCell(x, y);
                foreach (Pattern pattern in patterns.Values)
                {
                    outputGrid[x, y].possiblePatterns.Add(pattern);
                }
            }
        }
    }

    /// <summary>
    ///     The main part of the algorithm: find the cell with the lowest entropy, collapse it, update other cells, and repeat
    /// </summary>
    private void CollapseGrid()
    {
        while (true)
        {
            var cell = GetCellWithLowestEntropy();
            if (cell == null) break;

            var selectedPattern = SelectPattern(cell);
            cell.possiblePatterns.Clear();
            cell.possiblePatterns.Add(selectedPattern);

            PropagateConstraints(cell);
        }
    }

    /// <summary>
    ///     Finds the cells with the lowest entropy
    /// </summary>
    /// <returns></returns>
    private GridCell GetCellWithLowestEntropy()
    {
        GridCell lowestEntropyCell = null;
        var lowestEntropy = float.MaxValue;

        for (int y = 0; y< outputHeight; y++)
        {
            for (int x = 0; x< outputWidth; x++)
            {
                var entro = CalculateEntropy(outputGrid[x, y]);
                if (entro < lowestEntropy)
                {
                    lowestEntropy = entro;
                    lowestEntropyCell = outputGrid[x, y];
                }
            }
        }

        return lowestEntropyCell;
    }

    /// <summary>
    ///     Calculates the entropy of a specific cell, using Shannon Entropy
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private float CalculateEntropy(GridCell cell)
    {
        var totalFrequency = cell.possiblePatterns.AsParallel().Sum(pattern => pattern.frequency);

        return cell.possiblePatterns.AsParallel().
            Select(pattern => pattern.frequency / (float)totalFrequency).
            Aggregate<float, float>(0, (current, probability) => current - probability * Mathf.Log(probability, 2));
    }

    /// <summary>
    ///     Decides which pattern to collapse the cell into
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private Pattern SelectPattern(GridCell cell)
    {
        // Select a pattern randomly (you can implement more sophisticated selection methods)
        var index = Random.Range(0, cell.possiblePatterns.Count);
        return cell.possiblePatterns[index];
    }

    /// <summary>
    ///     Update contraints from the collapsed cell onwards.
    ///     I implemented this as a graph BFS, but you could use other strategies
    /// </summary>
    /// <param name="cell"></param>
    private void PropagateConstraints(GridCell cell)
    {
        var cellsToUpdate = new Queue<GridCell>();
        cellsToUpdate.Enqueue(cell);

        while (cellsToUpdate.Count > 0)
        {
            var currentCell = cellsToUpdate.Dequeue();
            var neighbors = GetNeighbors(currentCell);

            foreach (var neighbor in neighbors)
            {
                var updated = UpdatePossiblePatterns(currentCell, neighbor);
                if (updated)
                {
                    if (neighbor.possiblePatterns.Count == 0)
                        // If you find a cell with no patterns, it means you found an impossible situation, so you the algorithm can't continue. You could restart the algorithm, for now I just threw an exception.
                        throw new UnityException("Encountered an impossible situation.");
                    cellsToUpdate.Enqueue(neighbor);
                }
            }
        }
    }

    /// <summary>
    ///     Finds the neighbors of a specified cell
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private List<GridCell> GetNeighbors(GridCell cell)
    {
        var neighbors = new List<GridCell>();
        var x = cell.x;
        var y = cell.y;

        if (x > 0)
            neighbors.Add(outputGrid[x - 1, y]);
        if (x < outputWidth - 1)
            neighbors.Add(outputGrid[x + 1, y]);
        if (y > 0)
            neighbors.Add(outputGrid[x, y - 1]);
        if (y < outputHeight - 1)
            neighbors.Add(outputGrid[x, y + 1]);

        return neighbors;
    }

    /// <summary>
    ///     Updates the patterns for the neighbor cell.
    ///     Basically look through neighbor.possiblePatterns and find out if some have become illegal. Update the list so that
    ///     you only have legal patterns.
    /// </summary>
    /// <param name="currentCell"></param>
    /// <param name="neighbor"></param>
    /// <returns>
    ///     Return true if the original list has changed, false otherwise. We can use this information to update the
    ///     neighbor's neighbors or stop (meanining you reached an equilibrium)
    /// </returns>
    private bool UpdatePossiblePatterns(GridCell currentCell, GridCell neighbor)
    {
        var possiblePatterns = new List<Pattern>(neighbor.possiblePatterns); //save the old patterns
        neighbor.possiblePatterns.Clear(); //clear the patterns list

        //now let's re-add patterns to neighbor.possiblePatterns if they are still valid
        throw new UnityException("Complete me!");

        return possiblePatterns.Count != neighbor.possiblePatterns.Count;
    }

    /// <summary>
    ///     Checks whether the neighbor's pattern you specify is valid for at least one of the currentCell's patterns.
    /// </summary>
    /// <param name="currentCell"></param>
    /// <param name="neighbor"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    private bool IsPatternValid(GridCell currentCell, GridCell neighbor, Pattern pattern)
    {
        foreach (var selectedPattern in currentCell.possiblePatterns)
        {
            // Determine the direction of the neighbor relative to the current cell
            var dx = neighbor.x - currentCell.x;
            var dy = neighbor.y - currentCell.y;

            if (dx == 1) // Neighbor is to the right
            {
                if (selectedPattern.CheckRightEdge(pattern)) return true;
            }
            else if (dx == -1) // Neighbor is to the left
            {
                if (selectedPattern.CheckLeftEdge(pattern)) return true;
            }
            else if (dy == 1) // Neighbor is above
            {
                if (selectedPattern.CheckTopEdge(pattern)) return true;
            }
            else if (dy == -1) // Neighbor is below
            {
                if (selectedPattern.CheckBottomEdge(pattern)) return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Builds the final image based on the collapsed grid. Note that each GridCell is equal to NxN pixels (the pattern
    ///     that it contains)
    /// </summary>
    private void GenerateOutputImage()
    {
        outputImage = new Texture2D(outputWidth * N, outputHeight * N);

        for (var y = 0; y < outputHeight; y++)
        for (var x = 0; x < outputWidth; x++)
        {
            var pattern = outputGrid[x, y].possiblePatterns[0];
            for (var dy = 0; dy < pattern.size; dy++)
            for (var dx = 0; dx < pattern.size; dx++)
                outputImage.SetPixel(x * pattern.size + dx, y * pattern.size + dy,
                    pattern.pixels[dy * pattern.size + dx]);
        }

        //Removed filtering to make the image look sharp
        outputImage.filterMode = FilterMode.Point;
        outputImage.Apply();
    }
}