namespace Akamah.Engine.Shared;

public sealed class RandomNumberGenerator(int? seed = null)
{
  private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();

  public int RollInt(int minInclusive, int maxInclusive)
  {
    return _random.Next(minInclusive, maxInclusive + 1);
  }

  public float RollFloat()
  {
    return (float)_random.NextDouble();
  }

  public T Change<T>(List<T> list)
  {
    if (list.Count == 0)
    {
      throw new ArgumentException("List cannot be empty", nameof(list));
    }

    int index = RollInt(0, list.Count - 1);
    return list[index];
  }
}
