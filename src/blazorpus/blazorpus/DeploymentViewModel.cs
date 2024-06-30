namespace blazorpus.BlazorPus;

public class DeploymentViewModel
{
    public ReleaseDeploymentStatus DevStatus { get; set; }
    public ReleaseDeploymentStatus DevTestStatus { get; set; }
    public ReleaseDeploymentStatus SiStatus { get; set; }
    public ReleaseDeploymentStatus ProdStatus { get; set; }

    public string EnvironmentName { get; set; }
    public ReleaseDeployment Release { get; set; }
    public DateTimeOffset DeploymentDate { get; set; }
}
