namespace LookupZipPattern;

using App.CloudServices.Helpers;
using App.Core;
using App.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;


public class Startup
{
    private IEnvironmentHelper _envHelper;
    private IAwsHelper _awsHelper;

    public Startup()
    {
        _envHelper = new EnvironmentHelper();
        _awsHelper = new AwsHelper(_envHelper);
    }

    public Startup(IEnvironmentHelper envHelper, IAwsHelper awsHelper)
    {
        _envHelper = envHelper;
        _awsHelper = awsHelper;
    }

    public async Task<IServiceCollection> ConfigureServices(IServiceCollection serviceCollection)
    {

        serviceCollection.TryAddSingleton<IEnvironmentHelper>(_envHelper);
        serviceCollection.TryAddSingleton<IAwsHelper>(_awsHelper);

        //now we need to build a temporary service provide to get the AWSHelper, which could be set from the constructors or
        //may have been pre populated in the serviceCollection parameter

        var tempProvider = serviceCollection.BuildServiceProvider();
        _awsHelper = tempProvider.GetRequiredService<IAwsHelper>();
        _envHelper = tempProvider.GetRequiredService<IEnvironmentHelper>();


        return serviceCollection;
    }

}
