using Akamah.Engine.Core;

namespace Akamah.Engine.Collisions;

public static class CollisionsManager
{
  // Collision events that can be subscribed to
  public static event Action<GameObject, GameObject>? OnCollisionEnter;
  public static event Action<GameObject, GameObject>? OnCollisionExit;

  public static void Initialize()
  {
    // Wire up collision events from SpatialManager
    SpatialManager.OnCollisionEnterEvent = (a, b) => OnCollisionEnter?.Invoke(a, b);
    SpatialManager.OnCollisionExitEvent = (a, b) => OnCollisionExit?.Invoke(a, b);
  }

  public static void AddObject(GameObject obj)
  {
    if (obj.Collider == null) return;
    SpatialManager.AddObject(obj);
  }

  public static void RemoveObject(GameObject obj)
  {
    SpatialManager.RemoveObject(obj);
  }

  public static void Update(float dt)
  {
    // Update spatial positions and perform collision detection
    SpatialManager.UpdatePositions();
    SpatialManager.UpdateCollisions();
  }

  public static IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    var potentialObjects = SpatialManager.GetPotentialCollisions(obj);

    foreach (var other in potentialObjects)
    {
      if (other == obj || other.Collider == null) continue;

      if (CheckCollisionRecs(obj.GetBounds(), other.GetBounds()))
      {
        yield return other;
      }
    }
  }

  // Helper method to get Rectangle bounds from GameObject
  private static Rectangle GetBounds(GameObject obj)
  {
    if (obj.Collider == null) return new Rectangle(0, 0, 0, 0);

    return new Rectangle(
      obj.Position.X + obj.Collider.Offset.X - obj.Anchor.X,
      obj.Position.Y + obj.Collider.Offset.Y - obj.Anchor.Y,
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
        float newX = otherBounds.X - obj.Collider.Size.X - obj.Collider.Offset.X + obj.Anchor.X;
        obj.Position = new Vector2(newX, obj.Position.Y);
      }
      else
      {
        // Object is to the right, push it right to just touch the other object
        float newX = otherBounds.X + otherBounds.Width - obj.Collider.Offset.X + obj.Anchor.X;
        obj.Position = new Vector2(newX, obj.Position.Y);
      }
    }

    if (resolveY)
    {
      if (objBounds.Y < otherBounds.Y)
      {
        // Object is above, push it up to just touch the other object
        float newY = otherBounds.Y - obj.Collider.Size.Y - obj.Collider.Offset.Y + obj.Anchor.Y;
        obj.Position = new Vector2(obj.Position.X, newY);
      }
      else
      {
        // Object is below, push it down to just touch the other object
        float newY = otherBounds.Y + otherBounds.Height - obj.Collider.Offset.Y + obj.Anchor.Y;
        obj.Position = new Vector2(obj.Position.X, newY);
      }
    }
  }

  public static void Clear()
  {
    SpatialManager.Clear();
  }

  /// <summary>
  /// Get debug information about the spatial system
  /// </summary>
  public static int GetSpatialObjectCount()
  {
    var performanceInfo = SpatialManager.GetPerformanceInfo();
    return performanceInfo.totalObjects;
  }
}