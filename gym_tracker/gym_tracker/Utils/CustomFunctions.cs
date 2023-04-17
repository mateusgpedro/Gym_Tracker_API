namespace gym_tracker.Utils;

public static class CustomFunctions
{
    public static bool ContainsAny(this string haystack, params char[] needles)
    {
        foreach (char needle in needles)
        {
            if (haystack.Contains(needle))
                return true;
        }

        return false;
    }
}