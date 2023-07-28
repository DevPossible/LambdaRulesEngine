using App.CloudServices.Helpers;
using App.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RulesLambda;

public class Startup
{
    private IEnvironmentHelper _envHelper;
    private IAwsHelper _awsHelper;

    public Startup()
    {
        _envHelper = new EnvironmentHelper();
        _awsHelper = new AwsHelper(_envHelper);
    }

    //constructor used for dependency injection and testing with mocks
    public Startup(IEnvironmentHelper envHelper, IAwsHelper awsHelper)
    {
        _envHelper = envHelper;
        _awsHelper = awsHelper;
    }


    public async Task<IServiceCollection> ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IEnvironmentHelper>(_envHelper);
        serviceCollection.TryAddSingleton<IAwsHelper>(_awsHelper);
        
        return serviceCollection;
    }

}
