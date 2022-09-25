using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Hoyo.IdentityServer.Storage.MongoDB;

public class RepositoryResourceStore : IResourceStore
{
    protected IRepository Repository;

    public RepositoryResourceStore(IRepository repository) => Repository = repository;

    private IEnumerable<IdentityResource> GetAllIdentityResources() => Repository.All<IdentityResource>();
    private IEnumerable<ApiResource> GetAllApiResources() => Repository.All<ApiResource>();
    private IEnumerable<ApiScope> GetAllApiScopes() => Repository.All<ApiScope>();

    public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames) =>
        Task.FromResult(Repository.Where<ApiResource>(e => apiResourceNames.Contains(e.Name)).AsEnumerable());

    public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames) =>
        Task.FromResult(Repository.Where<ApiResource>(e => scopeNames.Contains(e.Name)).AsEnumerable());

    public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames) =>
        Task.FromResult(Repository.Where<ApiScope>(e => scopeNames.Contains(e.Name)).AsEnumerable());

    public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames) =>
        Task.FromResult(Repository.Where<IdentityResource>(e => scopeNames.Contains(e.Name)).AsEnumerable());

    public Task<Resources> GetAllResourcesAsync() =>
        Task.FromResult(new Resources(GetAllIdentityResources(), GetAllApiResources(), GetAllApiScopes()));
}