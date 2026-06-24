using SauceTest.Models;
using System.Text.Json;

public static class ConfigurationHelper
{
    private static TestConfig? _config;

    public static TestConfig GetConfig()
    {
        if (_config != null) return _config;

        // Check environment variable first, fall back to TestRunParameters, then default to "local"
        var environment = (
                            Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ??
                            TestContext.Parameters["Environment"] ??
                            "local"
                          ).ToLower();
        TestContext.Out.WriteLine($"Running tests against environment: {environment}");

        var configFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "TestConfig",
            $"config.{environment}.json"
        );

        if (!File.Exists(configFile))
            throw new FileNotFoundException(
                $"Config file not found for environment '{environment}': {configFile}");

        var json = File.ReadAllText(configFile);
        _config = JsonSerializer.Deserialize<TestConfig>(json)
            ?? throw new InvalidOperationException("Failed to deserialize config file.");

        return _config;
    }
}