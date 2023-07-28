using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Helpers
{
    public class ZipHelper
    {
        public bool IsZipMatch(string zipCode, string pattern)
        {
            var zipInt = int.Parse(zipCode);

            var rangeList = pattern.Split(",")
                .Select(s => s.Replace(" ", ""))
                .Where(s => !string.IsNullOrWhiteSpace(s));
            var ranges = rangeList.Select(GetZipRange).ToArray();

            var result = ranges.Any(r => (zipInt >= r.Start) && (zipInt <= r.End));

            return result;
        }

        public ZipRange GetZipRange(string rangePattern)
        {
            var parts = rangePattern.Split("-");
            if (parts.Length == 1)
            {
                return new ZipRange() { Start = GetZipStartFromPattern(parts[0]), End = GetZipEndFromPattern(parts[0]) }; // 0,0
            }
            else
            {
                return new ZipRange() { Start = GetZipStartFromPattern(parts[0]), End = GetZipEndFromPattern(parts[1]) }; // 0,1
            }
        }

        private int GetZipStartFromPattern(string pattern)
        {
            pattern = pattern.TrimEnd('*').PadRight(5, '0');
            return int.Parse(pattern);
        }

        private int GetZipEndFromPattern(string pattern)
        {
            pattern = pattern.TrimEnd('*').PadRight(5, '9');
            return int.Parse(pattern);
        }

    }
}
