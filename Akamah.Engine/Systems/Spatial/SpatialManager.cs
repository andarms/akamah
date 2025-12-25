using Akamah.Engine.Core.Engine;
using Akamah.Engine.World.Tiles;

namespace Akamah.Engine.Systems.Spatial;

/// <summary>
/// Unified spatial manager for both collision detection and rendering optimization
/// </summary>
public static class SpatialManager
{
  // Use the same spatial grid for both collision and rendering
  private static SpatialHashGrid spatialGrid = new(64, 1600, 1600); // Will be updated based on map size
  private static readonly Dictionary<GameObject, Rectangle> lastKnownBounds = [];
  private static readonly List<GameObject> allObjects = [];

  // Performance tracking
  public static int LastFrameVisibleObjects { get; private set; }
  public static int LastFrameCollisionChecks { get; private set; }
  public static float LastSpatialUpdateTime { get; private set; }

  // Collision event delegates (to avoid circular dependency)
  public static Action<GameObject, GameObject>? OnCollisionEnterEvent;
  public static Action<GameObject, GameObject>? OnCollisionExitEvent;

  public static void Initialize()
  {
    // Use actual map dimensions for spatial grid
    int mapWorldWidth = (int)GameWorld.Map.Limits.X;
    int mapWorldHeight = (int)GameWorld.Map.Limits.Y;
    spatialGrid = new SpatialHashGrid(64, mapWorldWidth, mapWorldHeight);
    allObjects.Clear();
    lastKnownBounds.Clear();
  }

  /// <summary>
  /// Add an object to the spatial system
  /// </summary>
  public static void AddObject(GameObject obj)
  {
    if (obj == null) return;

    // Only add objects that need spatial tracking (non-tiles or objects with colliders)
    if (obj is Tile && obj.Collider == null)
      return;

    allObjects.Add(obj);
    var bounds = GetObjectBounds(obj);
    lastKnownBounds[obj] = bounds;
    spatialGrid.AddObject(obj, bounds);
  }

  /// <summary>
  /// Remove an object from the spatial system
  /// </summary>
  public static void RemoveObject(GameObject obj)
  {
    if (allObjects.Remove(obj))
    {
      if (lastKnownBounds.TryGetValue(obj, out var bounds))
      {
        spatialGrid.RemoveObject(obj, bounds);
        lastKnownBounds.Remove(obj);
      }

      // Clear collision data
      obj.Collider?.Collisions.Clear();
    }
  }

  /// <summary>
  /// Update spatial positions for moved objects
  /// </summary>
  public static void UpdatePositions()
  {
    var startTime = (float)GetTime();

    foreach (var obj in allObjects)
    {
      var currentBounds = GetObjectBounds(obj);
      if (lastKnownBounds.TryGetValue(obj, out var lastBounds) &&
          HasBoundsChanged(lastBounds, currentBounds))
      {
        spatialGrid.UpdateObject(obj, lastBounds, currentBounds);
        lastKnownBounds[obj] = currentBounds;
      }
    }

    LastSpatialUpdateTime = (float)GetTime() - startTime;
  }

  /// <summary>
  /// Get objects visible in the current viewport for rendering
  /// </summary>
  public static IEnumerable<GameObject> GetVisibleObjects(Vector2 viewportTopLeft, Vector2 viewportBottomRight)
  {
    var visibleObjects = spatialGrid.GetObjectsInViewport(viewportTopLeft, viewportBottomRight).ToList();
    LastFrameVisibleObjects = visibleObjects.Count;
    return visibleObjects;
  }

  /// <summary>
  /// Get objects for collision detection
  /// </summary>
  public static IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    if (obj.Collider == null) return [];

    var bounds = obj.GetBounds();
    return spatialGrid.GetPotentialCollisions(bounds);
  }

  /// <summary>
  /// Perform collision detection with spatial optimization
  /// </summary>
  public static void UpdateCollisions()
  {
    int collisionChecks = 0;
    const float COLLISION_RADIUS = 100f; // Only check collisions within 100 pixels of player

    // Get objects within collision radius around the player
    var playerPos = GameWorld.Player.Position;
    var collisionArea = new Rectangle(
      playerPos.X - COLLISION_RADIUS,
      playerPos.Y - COLLISION_RADIUS,
      COLLISION_RADIUS * 2,
      COLLISION_RADIUS * 2
    );

    var nearbyObjects = spatialGrid.GetPotentialCollisions(collisionArea);

    // Filter to only objects that actually need collision detection (exclude tiles)
    var collidableObjects = nearbyObjects.Where(obj => obj.Collider != null && ShouldCheckCollisions(obj)).ToList();

    // Check collision only between player and nearby objects for better performance
    var player = GameWorld.Player;
    if (player.Collider == null)
    {
      LastFrameCollisionChecks = 0;
      return;
    }

    var playerBounds = GetObjectBounds(player);

    foreach (var obj in collidableObjects)
    {
      if (obj == player) continue;

      // Distance culling - only check objects within collision radius
      var objBounds = GetObjectBounds(obj);
      if (!IsWithinRadius(playerPos, new Vector2(objBounds.X + objBounds.Width / 2, objBounds.Y + objBounds.Height / 2), COLLISION_RADIUS))
        continue;

      // Quick AABB overlap check before expensive collision detection
      if (!BoundsOverlap(playerBounds, objBounds))
        continue;

      collisionChecks++;
      bool wasColliding = player.Collider.Collisions.Contains(obj);
      bool isColliding = CheckCollisionRecs(playerBounds, objBounds);

      if (isColliding && !wasColliding)
      {
        player.Collider.Collisions.Add(obj);
        obj.Collider!.Collisions.Add(player);
        OnCollisionEnterEvent?.Invoke(player, obj);
      }
      else if (!isColliding && wasColliding)
      {
        player.Collider.Collisions.Remove(obj);
        obj.Collider!.Collisions.Remove(player);
        OnCollisionExitEvent?.Invoke(player, obj);
      }
    }

    LastFrameCollisionChecks = collisionChecks;
  }

  /// <summary>
  /// Check if an object should participate in collision detection
  /// </summary>
  private static bool ShouldCheckCollisions(GameObject obj)
  {
    // Exclude tiles and other non-interactive objects from collision detection
    return obj is not Tile;
  }

  /// <summary>
  /// Fast AABB overlap check
  /// </summary>
  private static bool BoundsOverlap(Rectangle a, Rectangle b)
  {
    return !(a.X + a.Width < b.X || b.X + b.Width < a.X ||
             a.Y + a.Height < b.Y || b.Y + b.Height < a.Y);
  }

  /// <summary>
  /// Check if two points are within a given radius (using squared distance for performance)
  /// </summary>
  private static bool IsWithinRadius(Vector2 center, Vector2 point, float radius)
  {
    float distanceSquared = (center.X - point.X) * (center.X - point.X) +
                           (center.Y - point.Y) * (center.Y - point.Y);
    return distanceSquared <= radius * radius;
  }

  /// <summary>
  /// Get object bounds for spatial calculations
  /// </summary>
  private static Rectangle GetObjectBounds(GameObject obj)
  {
    if (obj.Collider != null)
    {
      return new Rectangle(
        obj.Position.X + obj.Collider.Offset.X - obj.Anchor.X,
        obj.Position.Y + obj.Collider.Offset.Y - obj.Anchor.Y,
        obj.Collider.Size.X,
        obj.Collider.Size.Y
      );
    }

    // For objects without colliders, use position with small default size
    return new Rectangle(obj.Position.X - obj.Anchor.X, obj.Position.Y - obj.Anchor.Y, 16, 16);
  }

  /// <summary>
  /// Check if object bounds have changed significantly
  /// </summary>
  private static bool HasBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
  {
    return oldBounds.X != newBounds.X ||
           oldBounds.Y != newBounds.Y ||
           oldBounds.Width != newBounds.Width ||
           oldBounds.Height != newBounds.Height;
  }

  /// <summary>
  /// Clear all spatial data
  /// </summary>
  public static void Clear()
  {
    spatialGrid.Clear();
    allObjects.Clear();
    lastKnownBounds.Clear();
  }

  /// <summary>
  /// Get performance information
  /// </summary>
  public static (int totalObjects, int visibleObjects, int collisionChecks, float updateTime) GetPerformanceInfo()
  {
    return (allObjects.Count, LastFrameVisibleObjects, LastFrameCollisionChecks, LastSpatialUpdateTime);
  }

  /// <summary>
  /// Get spatial grid cell size
  /// </summary>
  public static int CellSize => spatialGrid.CellSize;

  /// <summary>
  /// Resize spatial grid to match current map dimensions
  /// </summary>
  public static void ResizeToMapDimensions()
  {
    int mapWorldWidth = (int)GameWorld.Map.Limits.X;
    int mapWorldHeight = (int)GameWorld.Map.Limits.Y;

    // Always recreate to ensure proper sizing
    spatialGrid = new SpatialHashGrid(64, mapWorldWidth, mapWorldHeight);
    lastKnownBounds.Clear();

    // Re-add all objects to the new grid
    foreach (var obj in allObjects.ToList())
    {
      var bounds = GetObjectBounds(obj);
      lastKnownBounds[obj] = bounds;
      spatialGrid.AddObject(obj, bounds);
    }
  }


}