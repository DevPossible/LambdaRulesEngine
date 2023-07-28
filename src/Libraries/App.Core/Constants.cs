using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core
{
    public static class Constants
    {
        public static readonly string ENVVAR_AWS_REGION = "AWS_REGION";
        public static readonly string ENVVAR_LAMBDA_NAME = "AWS_LAMBDA_FUNCTION_NAME";

        public static readonly string PRM_RULESENGINE_BUCKET;

        static Constants()
        {
            PRM_RULESENGINE_BUCKET = $"/dev/app/rulesengine/workflow_bucket_name";
        }

    }

}
