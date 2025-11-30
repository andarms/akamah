namespace Akamah.Engine.Scenes;

public class PerlinNoise
{
  private readonly int[] permutation;

  public PerlinNoise(int seed = 0)
  {
    var rand = new Random(seed);
    permutation = new int[512];
    int[] p = new int[256];

    for (int i = 0; i < 256; i++)
      p[i] = i;

    // Shuffle
    for (int i = 0; i < 256; i++)
    {
      int swap = rand.Next(256);
      (p[i], p[swap]) = (p[swap], p[i]);
    }

    // Duplicate
    for (int i = 0; i < 512; i++)
      permutation[i] = p[i & 255];
  }

  public float Noise(float x, float y)
  {
    int xi = (int)MathF.Floor(x) & 255;
    int yi = (int)MathF.Floor(y) & 255;

    float xf = x - MathF.Floor(x);
    float yf = y - MathF.Floor(y);

    float u = Fade(xf);
    float v = Fade(yf);

    int aa = permutation[permutation[xi] + yi];
    int ab = permutation[permutation[xi] + yi + 1];
    int ba = permutation[permutation[xi + 1] + yi];
    int bb = permutation[permutation[xi + 1] + yi + 1];

    float x1 = Lerp(Grad(aa, xf, yf), Grad(ba, xf - 1, yf), u);
    float x2 = Lerp(Grad(ab, xf, yf - 1), Grad(bb, xf - 1, yf - 1), u);

    return (Lerp(x1, x2, v) + 1) * 0.5f; // Normalizado 0–1
  }

  private static float Fade(float t)
      => t * t * t * (t * (t * 6 - 15) + 10);

  private static float Lerp(float a, float b, float t)
      => a + t * (b - a);

  private static float Grad(int hash, float x, float y)
  {
    switch (hash & 3)
    {
      case 0: return x + y;
      case 1: return -x + y;
      case 2: return x - y;
      default: return -x - y;
    }
  }
}
