namespace blazorpus.BlazorPus;

public record ReleaseDeploymentStatus
{
    public string Status { get; set; }
    public DateTimeOffset? DeployedAt { get; set; }

    internal static ReleaseDeploymentStatus NoDeploymentStatus()
    {
        return new ReleaseDeploymentStatus
        {
            Status = "",
            DeployedAt = null
        };
    }
}
