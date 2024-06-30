using Octokit;
using Octokit.Models.Response;
using System.Collections.ObjectModel;

namespace blazorpus.BlazorPus;

public class ReleaseDeploymentsRepository
{
    private readonly GitHubClient _client;
    private string? _repo;
    private readonly string? _owner;

    public ReleaseDeploymentsRepository(IConfiguration configuration, GitHubClient gitHubClient)
    {
        _repo = configuration["GitHub:Repo"];
        _owner = configuration["GitHub:Owner"];
        _client = gitHubClient;
    }

    public async Task<IReadOnlyList<DeploymentEnvironment>> GetEnvironments()
    {
        var environments = await _client.Repository.Environment.GetAll(_owner, _repo);
        return environments.Environments;
    }

    public async Task Deploy(string gitSha, string environment)
    {

        //gitSha = "main";
        var newDeployment = new NewDeployment(gitSha)
        {
            AutoMerge = false,
            Description = "",
            Environment = environment,
            RequiredContexts = new Collection<string>() // Skip status checks
        };
        var deployment = await _client.Repository.Deployment.Create(_owner, _repo, newDeployment);
    }

    public async Task<IReadOnlyList<ReleaseDeployment>> GetDeployments()
    {
        var releaseDeployments = new List<ReleaseDeployment>();
        var deployments = await _client.Repository.Deployment.GetAll(_owner, _repo);

        foreach (var deployment in deployments)
        {
            var status = (await _client.Repository.Deployment.Status.GetAll(_owner, _repo, deployment.Id))
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ReleaseDeploymentStatus
                {
                    Status = x.State.ToString() ?? "unknown",
                    DeployedAt = x.CreatedAt
                })
                .FirstOrDefault();

            releaseDeployments.Add(new ReleaseDeployment
            {
                ReleaseName = deployment.Sha,
                DeploymentCreateDate = deployment.CreatedAt,
                EnvironmentName = deployment.Environment,                
                Status = status ?? ReleaseDeploymentStatus.NoDeploymentStatus()
            });
        }

        return releaseDeployments;
    }
}
