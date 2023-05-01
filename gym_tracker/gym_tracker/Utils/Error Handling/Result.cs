namespace gym_tracker.Utils;

public class Result<TItem>
{
    public bool Succeded = true;
    
    public List<string> Errors = new();

    public TItem Item = default!;
}