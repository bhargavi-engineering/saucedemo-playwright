using Microsoft.Playwright;
using SauceTest.Pages;

namespace SauceTest.Tests.UXTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class LoginTests : BaseTest
    {
        
        [TestCaseSource(typeof(CsvTestCaseSource),
                        nameof(CsvTestCaseSource.GetTestCases), 
                        new object[] {"Data\\Logins.csv", typeof(LoginModel) }) ]
        public async Task LoginValidUsers(LoginModel login)
        {
            var loginPage = new LoginPage(Page);
            var inventoryPage = await loginPage.LoginAsync(login.Username, login.Password);
            Assert.That(await inventoryPage.GetNumberOfItemsAsync(), Is.GreaterThanOrEqualTo(1),
                           "No product items exist on page");
        }
    }
}
