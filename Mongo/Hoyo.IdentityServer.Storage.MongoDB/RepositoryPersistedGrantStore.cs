using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Hoyo.IdentityServer.Storage.MongoDB;

public class RepositoryPersistedGrantStore : IPersistedGrantStore
{
    protected IRepository Repository;

    public RepositoryPersistedGrantStore(IRepository repository) => Repository = repository;

    public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter) => Task.FromResult(Repository.Where<PersistedGrant>(c => c.SubjectId == filter.SubjectId).AsEnumerable());

    public Task<PersistedGrant> GetAsync(string key) => Task.FromResult(Repository.Single<PersistedGrant>(i => i.Key == key));

    public Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        Repository.Delete<PersistedGrantFilter>(i => i.SubjectId == filter.SubjectId && i.ClientId == filter.ClientId && i.Type == filter.Type);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        Repository.Delete<PersistedGrant>(i => i.Key == key);
        return Task.CompletedTask;
    }

    public Task StoreAsync(PersistedGrant grant)
    {
        Repository.Add(grant);
        return Task.CompletedTask;
    }
}
