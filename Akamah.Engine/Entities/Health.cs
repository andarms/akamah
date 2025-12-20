namespace Akamah.Engine.Entities;

public class Health(int max)
{
  public int Current { get; set; } = max;
  public int Max { get; set; } = max;

  public void TakeDamage(int amount)
  {
    Current -= amount;
    if (Current < 0) Current = 0;
  }
}
