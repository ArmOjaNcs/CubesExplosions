using System;

public static class UserUtils 
{
    private static Random _random = new Random();

    public static int GetRandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }

    public static int GetRandomNumber(int max)
    {
        return _random.Next(max);
    }
}
