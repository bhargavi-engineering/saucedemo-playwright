using Microsoft.Playwright;

namespace SauceTest.Pages
{
    public class LoginPage: BasePage
    {        
        public LoginPage(IPage page):base(page) 
        { 
            this.WaitForPageLoadAsync().ConfigureAwait(false);
        }

        #region Locators
        private const string userNameSelector = "[data-test=\"username\"]";
        private const string passwordSelector = "[data-test=\"password\"]";
        private const string loginBtnSelector = "[data-test=\"login-button\"]";

        public ILocator UserNameTextBox => _page.Locator(userNameSelector);
        public ILocator PasswordTextBox => _page.Locator(passwordSelector);
        public ILocator LoginButton => _page.Locator(loginBtnSelector);

        #endregion

        #region Actions

        public override async Task WaitForPageLoadAsync()
        {
            await LoginButton.WaitForAsync();
        }

        private async Task EnterUserNameAsync(string username)
        {
            await UserNameTextBox.FillAsync(username);
        }

        private async Task EnterPasswordAsync(string password)
        {
            await PasswordTextBox.FillAsync(password);
        }

        public async Task<InventoryPage> LoginAsync(string username, string password)
        {
            await EnterUserNameAsync(username);
            await EnterPasswordAsync(password);
            await LoginButton.ClickAsync();

            return new InventoryPage(_page); 
        }

        #endregion

    }
}
