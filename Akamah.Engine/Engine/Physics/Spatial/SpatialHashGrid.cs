using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Physics.Spatial;

/// <summary>
/// Spatial hash grid for efficient collision detection
/// </summary>
public class SpatialHashGrid
{
  private readonly Dictionary<int, HashSet<GameObject>> grid;
  private readonly int cellSize;
  private readonly int worldWidth;
  private readonly int worldHeight;
  private readonly int gridWidth;
  private readonly int gridHeight;

  public SpatialHashGrid(int cellSize, int worldWidth, int worldHeight)
  {
    this.cellSize = cellSize;
    this.worldWidth = worldWidth;
    this.worldHeight = worldHeight;
    this.gridWidth = (worldWidth / cellSize) + 1;
    this.gridHeight = (worldHeight / cellSize) + 1;
    this.grid = new Dictionary<int, HashSet<GameObject>>();
  }

  /// <summary>
  /// Convert world position to grid cell coordinates
  /// </summary>
  private (int x, int y) WorldToGrid(float worldX, float worldY)
  {
    int gridX = Math.Max(0, Math.Min(gridWidth - 1, (int)(worldX / cellSize)));
    int gridY = Math.Max(0, Math.Min(gridHeight - 1, (int)(worldY / cellSize)));
    return (gridX, gridY);
  }

  /// <summary>
  /// Convert grid coordinates to hash key
  /// </summary>
  private int GridToHash(int gridX, int gridY)
  {
    return gridY * gridWidth + gridX;
  }

  /// <summary>
  /// Get all grid cells that a rectangle overlaps
  /// </summary>
  private IEnumerable<int> GetOverlappingCells(Rectangle bounds)
  {
    var (minX, minY) = WorldToGrid(bounds.X, bounds.Y);
    var (maxX, maxY) = WorldToGrid(bounds.X + bounds.Width, bounds.Y + bounds.Height);

    for (int y = minY; y <= maxY; y++)
    {
      for (int x = minX; x <= maxX; x++)
      {
        yield return GridToHash(x, y);
      }
    }
  }

  /// <summary>
  /// Add object to spatial grid
  /// </summary>
  public void AddObject(GameObject obj, Rectangle bounds)
  {
    foreach (int cellHash in GetOverlappingCells(bounds))
    {
      if (!grid.TryGetValue(cellHash, out var cell))
      {
        cell = new HashSet<GameObject>();
        grid[cellHash] = cell;
      }
      cell.Add(obj);
    }
  }

  /// <summary>
  /// Remove object from spatial grid
  /// </summary>
  public void RemoveObject(GameObject obj, Rectangle bounds)
  {
    foreach (int cellHash in GetOverlappingCells(bounds))
    {
      if (grid.TryGetValue(cellHash, out var cell))
      {
        cell.Remove(obj);
        if (cell.Count == 0)
        {
          grid.Remove(cellHash);
        }
      }
    }
  }

  /// <summary>
  /// Update object position in spatial grid
  /// </summary>
  public void UpdateObject(GameObject obj, Rectangle oldBounds, Rectangle newBounds)
  {
    // Only update if bounds actually changed
    if (oldBounds.X == newBounds.X && oldBounds.Y == newBounds.Y &&
        oldBounds.Width == newBounds.Width && oldBounds.Height == newBounds.Height)
    {
      return;
    }

    RemoveObject(obj, oldBounds);
    AddObject(obj, newBounds);
  }

  /// <summary>
  /// Get all objects that could potentially collide with the given object
  /// </summary>
  public IEnumerable<GameObject> GetPotentialCollisions(Rectangle bounds)
  {
    var potentialObjects = new HashSet<GameObject>();

    foreach (int cellHash in GetOverlappingCells(bounds))
    {
      if (grid.TryGetValue(cellHash, out var cell))
      {
        foreach (var obj in cell)
        {
          potentialObjects.Add(obj);
        }
      }
    }

    return potentialObjects;
  }

  /// <summary>
  /// Get all objects within a viewport area for rendering
  /// </summary>
  public IEnumerable<GameObject> GetObjectsInViewport(Vector2 topLeft, Vector2 bottomRight)
  {
    var viewportBounds = new Rectangle(
      topLeft.X,
      topLeft.Y,
      bottomRight.X - topLeft.X,
      bottomRight.Y - topLeft.Y
    );

    var visibleObjects = new HashSet<GameObject>();

    foreach (int cellHash in GetOverlappingCells(viewportBounds))
    {
      if (grid.TryGetValue(cellHash, out var cell))
      {
        foreach (var obj in cell)
        {
          if (!obj.Visible) continue;
          visibleObjects.Add(obj);
        }
      }
    }

    return visibleObjects;
  }

  /// <summary>
  /// Get objects in specific grid cells efficiently (for rendering optimization)
  /// </summary>
  public IEnumerable<GameObject> GetObjectsInCells(int minGridX, int minGridY, int maxGridX, int maxGridY)
  {
    var objects = new HashSet<GameObject>();

    // Clamp to valid grid bounds
    minGridX = Math.Max(0, minGridX);
    minGridY = Math.Max(0, minGridY);
    maxGridX = Math.Min(gridWidth - 1, maxGridX);
    maxGridY = Math.Min(gridHeight - 1, maxGridY);

    for (int y = minGridY; y <= maxGridY; y++)
    {
      for (int x = minGridX; x <= maxGridX; x++)
      {
        int cellHash = GridToHash(x, y);
        if (grid.TryGetValue(cellHash, out var cell))
        {
          foreach (var obj in cell)
          {
            objects.Add(obj);
          }
        }
      }
    }

    return objects;
  }

  /// <summary>
  /// Get viewport grid coordinates
  /// </summary>
  public (int minX, int minY, int maxX, int maxY) GetViewportGridBounds(Vector2 topLeft, Vector2 bottomRight)
  {
    var (minX, minY) = WorldToGrid(topLeft.X, topLeft.Y);
    var (maxX, maxY) = WorldToGrid(bottomRight.X, bottomRight.Y);
    return (minX, minY, maxX, maxY);
  }

  /// <summary>
  /// Clear all objects from the grid
  /// </summary>
  public void Clear()
  {
    grid.Clear();
  }

  /// <summary>
  /// Get the cell size used by this grid
  /// </summary>
  public int CellSize => cellSize;


}