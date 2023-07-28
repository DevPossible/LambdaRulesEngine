using System.Dynamic;
using App.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RulesLambda.RulesExtensions
{
    public static class Fn
    {
        //because of the following reasons ...
        // 1. static class contructors cannot take parameters
        // 2. there is basically NO other good way to inject dependencies into static classes
        // 3. this class MUST be a static class for integration into the rules engine
        // 4. region needs to be set externally
        //
        //... Region is exposed as a public thread-safe property that must be set by the calling code
        private static object lockObj = new object();
        private static string _region;
        public static string Region { 
            get {
                if (_region == null) throw new Exception($"Cannot retrieve current AWS region.");
                return _region; 
            }
            set { 
                lock (lockObj) { _region = value; }; 
            } 
        }



        static Fn()
        {
        }

        public static bool IsZipPairApproved(object origin, object destination)
        {
            var name = "EvalApprovedZipPair";
            var input = new { Origin = origin.ToString(), Destination = destination.ToString() };
            var response = AWS.ExecuteLambda(_region, name, input);
            dynamic? responseObj = JsonConvert.DeserializeObject<ExpandoObject>(response, new ExpandoObjectConverter());
            return responseObj?.Result;
        }

        public static bool IsShipperEntityIncluded(object list, object entityId)
        {
            var name = "EvalShipperEntity";
            var input = new { List = list.ToString(), EntityId = entityId.ToString() };
            var response = AWS.ExecuteLambda(_region, name, input);
            dynamic? responseObj = JsonConvert.DeserializeObject<ExpandoObject>(response, new ExpandoObjectConverter());
            return responseObj?.Result;
        }

        public static string LookupZipPattern(object names)
        {
            var name = "LookupZipPattern";
            var input = new { Names = names.ToString() };
            var response = AWS.ExecuteLambda(_region, name, input);
            dynamic? responseObj = JsonConvert.DeserializeObject<ExpandoObject>(response, new ExpandoObjectConverter());
            return responseObj?.Pattern;
        }

        public static string GetVehicleType(object vin)
        {

            var name = "GetVehicleEnrichment";
            var input = new { Vin = vin.ToString() };
            var response = AWS.ExecuteLambda(_region, name, input);
            var responseObj = JsonConvert.DeserializeObject<VinLookupResponse>(response);

            return responseObj?.Results.FirstOrDefault(r => r.Variable == "Vehicle Type")?.Value.ToString().Trim() ?? string.Empty;
        }

        public static string GetVehicleData(object vin, object field)
        {

            var name = "GetVehicleEnrichment";
            var input = new { Vin = vin.ToString() };
            var response = AWS.ExecuteLambda(_region, name, input);
            var responseObj = JsonConvert.DeserializeObject<VinLookupResponse>(response);

            var fieldStr = field.ToString() ?? "";
            //if the user passed in number, match on the VariableID field, otherwise match on the variable field
            if (fieldStr.All(char.IsNumber))
            {
                return responseObj?.Results.FirstOrDefault(r => r.VariableId == int.Parse(fieldStr))?.Value.ToString().Trim() ?? string.Empty;
            }
            else
            {
                return responseObj?.Results.FirstOrDefault(r => r.Variable == fieldStr)?.Value.ToString().Trim() ?? string.Empty;
            }

        }

        public class VinLookupValueRecord
        {
            public string Value { get; set; }
            public string ValueId { get; set; }
            public string Variable { get; set; }
            public int VariableId { get; set; }
        }

        public class VinLookupResponse
        {
            public int Count { get; set; }
            public string Message { get; set; }
            public VinLookupValueRecord[] Results { get; set; }
        }


    }
}
