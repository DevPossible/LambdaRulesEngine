using App.Core.Helpers;

namespace RulesLambda.RulesExtensions
{
    public static class Util
    {
        public static bool IsZipMatch(object zipCode, object pattern)
        {
            var zipHelper = new ZipHelper();
            return zipHelper.IsZipMatch(zipCode.ToString(), pattern.ToString());
        }

    }
}
