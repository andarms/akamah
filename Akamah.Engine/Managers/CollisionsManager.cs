using System;
using System.Collections.Generic;
using System.Linq;

namespace Akamah.Engine.Managers;

public static class CollisionsManager
{
  const int CELL_SIZE = 64;
  const int WORLD_WIDTH = 10_000;
  const int WORLD_HEIGHT = 10_000;

  private static SpatialHashGrid spatialGrid = new(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);
  private static readonly List<GameObject> objects = [];
  private static readonly Dictionary<GameObject, Rectangle> lastKnownCollisionBounds = [];

  public static void Initialize()
  {
    spatialGrid = new SpatialHashGrid(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);
    objects.Clear();
    lastKnownCollisionBounds.Clear();
  }

  public static void AddObject(GameObject obj)
  {
    if (obj.Collider == null) return;
    objects.Add(obj);
    var bounds = GetBounds(obj);
    lastKnownCollisionBounds[obj] = bounds;
    spatialGrid.AddObject(obj, bounds);
  }

  public static void RemoveObject(GameObject obj)
  {
    if (objects.Remove(obj))
    {
      if (lastKnownCollisionBounds.TryGetValue(obj, out var bounds))
      {
        spatialGrid.RemoveObject(obj, bounds);
        lastKnownCollisionBounds.Remove(obj);
      }
      if (obj.Collider != null)
      {
        obj.Collider.Collisions.Clear();
      }
    }
  }


  public static void Update(float dt)
  {
    UpdatePositions();
    CheckCollisions();
  }

  /// <summary>
  /// Process collision enter/stay/exit events for all objects using spatial hashing
  /// </summary>
  private static void CheckCollisions()
  {
    var checkedPairs = new HashSet<(GameObject, GameObject)>();

    foreach (var objA in objects)
    {
      if (objA.Collider == null) continue;

      var objABounds = GetBounds(objA);
      var potentialCollisions = spatialGrid.GetPotentialCollisions(objABounds);

      foreach (var objB in potentialCollisions)
      {
        if (objB == objA || objB.Collider == null) continue;

        // Ensure we only check each pair once
        var pair = objA.GetHashCode() < objB.GetHashCode() ? (objA, objB) : (objB, objA);
        if (checkedPairs.Contains(pair)) continue;
        checkedPairs.Add(pair);

        bool wasColliding = objA.Collider.Collisions.Contains(objB);
        bool isColliding = CheckCollisionRecs(objABounds, GetBounds(objB));

        if (isColliding && !wasColliding)
        {
          objA.Collider.Collisions.Add(objB);
          objB.Collider.Collisions.Add(objA);

          // Trigger collision events here if needed
          OnCollisionEnter?.Invoke(objA, objB);
        }
        else if (!isColliding && wasColliding)
        {
          objA.Collider.Collisions.Remove(objB);
          objB.Collider.Collisions.Remove(objA);

          // Trigger collision exit events here if needed
          OnCollisionExit?.Invoke(objA, objB);
        }
      }
    }
  }  // Collision events that can be subscribed to
  public static event Action<GameObject, GameObject>? OnCollisionEnter;
  public static event Action<GameObject, GameObject>? OnCollisionExit;

  private static void UpdatePositions()
  {
    foreach (var obj in objects)
    {
      if (obj.Collider == null) continue;

      var currentBounds = GetBounds(obj);
      if (lastKnownCollisionBounds.TryGetValue(obj, out var lastKnownBounds) &&
          (lastKnownBounds.X != currentBounds.X || lastKnownBounds.Y != currentBounds.Y ||
           lastKnownBounds.Width != currentBounds.Width || lastKnownBounds.Height != currentBounds.Height))
      {
        // Update spatial grid with new position
        spatialGrid.UpdateObject(obj, lastKnownBounds, currentBounds);
        lastKnownCollisionBounds[obj] = currentBounds;
      }
    }
  }
  public static IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    if (obj.Collider == null) yield break;

    var objBounds = GetBounds(obj);
    var potentialObjects = spatialGrid.GetPotentialCollisions(objBounds);

    foreach (var other in potentialObjects)
    {
      if (other == obj || other.Collider == null) continue;

      if (CheckCollisionRecs(objBounds, GetBounds(other)))
      {
        yield return other;
      }
    }
  }  // Helper method to get Rectangle bounds from GameObject
  private static Rectangle GetBounds(GameObject obj)
  {
    if (obj.Collider == null) return new Rectangle(0, 0, 0, 0);

    return new Rectangle(
      obj.Position.X + obj.Collider.Offset.X,
      obj.Position.Y + obj.Collider.Offset.Y,
      obj.Collider.Size.X,
      obj.Collider.Size.Y
    );
  }

  public static void ResolveSolidCollision(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (obj.Collider == null || other.Collider == null) return;
    StopObject(obj, other, resolveX, resolveY);
  }

  private static void StopObject(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (obj.Collider == null || other.Collider == null) return;

    // Get the actual collision bounds for both objects
    Rectangle objBounds = GetBounds(obj);
    Rectangle otherBounds = GetBounds(other);

    if (resolveX)
    {
      if (objBounds.X < otherBounds.X)
      {
        // Object is to the left, push it left to just touch the other object
        float newX = otherBounds.X - obj.Collider.Size.X - obj.Collider.Offset.X;
        obj.Position = new Vector2(newX, obj.Position.Y);
      }
      else
      {
        // Object is to the right, push it right to just touch the other object
        float newX = otherBounds.X + otherBounds.Width - obj.Collider.Offset.X;
        obj.Position = new Vector2(newX, obj.Position.Y);
      }
    }

    if (resolveY)
    {
      if (objBounds.Y < otherBounds.Y)
      {
        // Object is above, push it up to just touch the other object
        float newY = otherBounds.Y - obj.Collider.Size.Y - obj.Collider.Offset.Y;
        obj.Position = new Vector2(obj.Position.X, newY);
      }
      else
      {
        // Object is below, push it down to just touch the other object
        float newY = otherBounds.Y + otherBounds.Height - obj.Collider.Offset.Y;
        obj.Position = new Vector2(obj.Position.X, newY);
      }
    }
  }

  public static void Clear()
  {
    spatialGrid.Clear();
    objects.Clear();
    lastKnownCollisionBounds.Clear();
  }

  /// <summary>
  /// Get debug information about the spatial grid
  /// </summary>
  public static (int cellCount, int objectCount) GetSpatialGridDebugInfo()
  {
    return spatialGrid.GetDebugInfo();
  }
}