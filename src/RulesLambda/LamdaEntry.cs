using Amazon.Lambda.Core;
using App.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RulesLambda.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RulesLambda;

// This is the entry point class and function that AWS will use to execute C# this code.
// In order to provide dependency injection and allow unit testing, the core code 
// is actually held within "LambdaFunction"
// The setup class is used to define services to be used with dependency injection.

public class LambdaEntry
{
    private IServiceCollection _initialServices;

    public LambdaEntry()
    {
        _initialServices = new ServiceCollection();        
    }

    //constructor for unit testing / mock
    public LambdaEntry(IServiceCollection initialServices)
    {
        _initialServices = initialServices;
    }


    public async Task<string> MainEntryPoint(ExecuteWorkflow input, ILambdaContext context)
    {
        _initialServices.TryAddTransient<LambdaFunction>();
        _initialServices.TryAddSingleton<ILambdaContext>(context);
        _initialServices.TryAddSingleton<ILambdaLogger>(context.Logger);

        var _services = (await new Startup().ConfigureServices(_initialServices)).BuildServiceProvider();

        var logger = _services.GetRequiredService<ILambdaLogger>();
        var envHelper = _services.GetRequiredService<IEnvironmentHelper>();
        try
        {
            logger.LogInformation($"Starting Lambda: {envHelper.CurrentLambdaName}");
            var app = _services.GetService<LambdaFunction>();
            if (app == null) throw new Exception("Could not start LambdaFunction");
            return await app.RunAsync(input, context);
        }
        finally
        {
            logger.LogInformation($"Exiting Lambda: {envHelper.CurrentLambdaName}");
        }
    }
}
