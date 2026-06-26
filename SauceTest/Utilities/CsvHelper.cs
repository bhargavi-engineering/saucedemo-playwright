using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;


namespace SauceTest.Utilities
{
    public class CsvTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetTestCases(string csvFilePath, Type modelType)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter =",",
            };

            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, config);

            // Dynamically read records as the specified model type
            var records = csv.GetRecords(modelType);

            foreach (var record in records)
            {
                yield return new TestCaseData(record)
                    .SetName($"{modelType.Name}_{records}");
            }
        }
    }
}

