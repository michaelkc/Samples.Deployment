namespace blazorpus.BlazorPus;

public class DeploymentMatrixService
{
    private readonly ReleaseDeploymentsRepository releaseDeploymentsRepo;

    public DeploymentMatrixService(ReleaseDeploymentsRepository releaseDeploymentsRepo)
    {
        this.releaseDeploymentsRepo = releaseDeploymentsRepo;
    }

    private async Task<IReadOnlyList<DisplayEnvironment>> GetDisplayEnvironments() =>
        (await releaseDeploymentsRepo.GetEnvironments())
            .Select(x => new DisplayEnvironment { Name = x.Name })
            .ToList();

    public async Task<List<List<ReleaseDeployment>>> Get()
    {
        var environments = (await GetDisplayEnvironments());
        var deployments = await releaseDeploymentsRepo.GetDeployments2();
        var groupedByRelease = deployments
            .GroupBy(d => d.ReleaseName)
            .OrderByDescending(g => g.Min(d => d.DeploymentCreateDate))
            .ToList();

        var matrix = new List<List<ReleaseDeployment>>();

        foreach (var releaseGroup in groupedByRelease)
        {
            var row = new List<ReleaseDeployment>();
            foreach (var env in environments.OrderBy(x => x.GetOrder()))
            {
                var deployment = releaseGroup.FirstOrDefault(d => d.EnvironmentName == env.Name)
                                 ?? ReleaseDeployment.NoDeployment(releaseGroup.Key, env.Name);
                row.Add(deployment);
            }
            matrix.Add(row);
        }

        return matrix;
    }
}


