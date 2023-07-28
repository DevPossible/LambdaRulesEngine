using App.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace GetVehicleEnrichment;


public class Startup
{
    private IEnvironmentHelper _envHelper;

    public Startup()
    {
        _envHelper = new EnvironmentHelper();
    }

    public Startup(IEnvironmentHelper envHelper)
    {
        _envHelper = envHelper;
    }



    public async Task<IServiceCollection> ConfigureServices(IServiceCollection serviceCollection)
    {

        serviceCollection.TryAddSingleton<IEnvironmentHelper>(_envHelper);

        //now we need to build a temporary service provide to get the AWSHelper, which could be set from the constructors or
        //may have been pre populated in the serviceCollection parameter

        var tempProvider = serviceCollection.BuildServiceProvider();
        _envHelper = tempProvider.GetRequiredService<IEnvironmentHelper>();

        return serviceCollection;
    }

}
