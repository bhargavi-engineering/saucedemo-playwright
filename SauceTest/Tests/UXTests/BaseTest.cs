using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Tests.UXTests
{
    public abstract class BaseTest:PageTest
    {
        protected TestConfig Config => ConfigurationHelper.GetConfig();

        [SetUp]
        public async Task BaseSetUp()
        {            
            //Start tracing
            await Context.Tracing.StartAsync(new()
            {
                Title = $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            //Navigate to site
            await Page.GotoAsync(Config.SiteUrl);

        }

        [TearDown]
        public async Task TearDown()
        {
            // This will produce upon failure e.g.:
            // bin/Debug/net8.0/playwright-traces/PlaywrightTests.Tests.Test1.zip
            var failed = TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Error ||
                         TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Failure ||
                         TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Inconclusive;

            if (failed)
            {
                var fileName = $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.{DateTime.Now}";

                //Strip filename of invalid path characters
                var invalidChars = Path.GetInvalidFileNameChars();
                string safeName = string.Join('_', fileName.Split(invalidChars));                
                
                //Create Trace Path
                var tracePath = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "playwright-traces",
                    $"{safeName}.zip"
                );

                //Create directory if not exists
                Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);

                //Stop tracing and output the Trace Path
                await Context.Tracing.StopAsync(new() { Path = tracePath });
                TestContext.Out.WriteLine($"Trace can be found here: {tracePath}");
            }
            else
            {
                await Context.Tracing.StopAsync(new() { Path = null });
            }



        }
    }
}
