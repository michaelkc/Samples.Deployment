namespace blazorpus.BlazorPus;

public record ReleaseDeployment
{
    public string EnvironmentName { get; set; }
    public string ReleaseName { get; set; }
    public DateTimeOffset DeploymentCreateDate { get; set; }
    public ReleaseDeploymentStatus Status { get; set; }

    public static ReleaseDeployment NoDeployment(string releaseName, string environmentName)
    {
        return new ReleaseDeployment
        {
            ReleaseName = releaseName,
            EnvironmentName = environmentName,
            DeploymentCreateDate = DateTimeOffset.MinValue,
            Status = ReleaseDeploymentStatus.NoDeploymentStatus()
        };
    }
}
