
using Postwomen.Models;
using Refit;

namespace Postwomen.Interfaces;

[Headers("Content-Type:application/json")]
public interface IMainApi {
    [Get("/app/get")] Task<VersionModel> GetLatestAppVersion([AliasAs("AppKey")] string AppName, CancellationToken cancellationToken);

}