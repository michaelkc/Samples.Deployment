namespace blazorpus.BlazorPus;

public record DisplayEnvironment
{
    public string Name { get; set; }

    public int GetOrder()
    {
        return this.Name switch
        {
            "Dev" => 1,
            "DevTest" => 2,
            "SI" => 3,
            "Prod" => 4,
            _ => 5
        };
    }
}

