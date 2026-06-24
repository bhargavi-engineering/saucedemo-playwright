using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Pages
{
    public class CheckoutInformationPage : BasePage
    {
        public CheckoutInformationPage(IPage page): base(page) { }
                
        #region Locators
        private const string titleTextSelector = "Checkout: Your Information"; 
        private const string checkoutTitleSelector = "[data-test=\"title\"]";
        private const string firstNameSelector = "[data-test=\"firstName\"]";
        private const string lastNameSelector = "[data-test=\"lastName\"]";
        private const string postalCodeSelector = "[data-test=\"postalCode\"]";
        private const string cancelBtnSelector = "[data-test=\"cancel\"]";
        private const string continueBtnSelector = "[data-test=\"continue\"]";

        public ILocator FirstNameTextBox => _page.Locator(firstNameSelector);
        public ILocator LastNameTextBox => _page.Locator(lastNameSelector);
        public ILocator PostalCodeTextBox => _page.Locator(postalCodeSelector);
        public ILocator CheckoutTitleText => _page.GetByText(titleTextSelector);
        public ILocator ContinueButton => _page.Locator(continueBtnSelector);
        public ILocator CancelButton => _page.Locator(cancelBtnSelector);

        #endregion

        #region Actions

        public async override Task WaitForPageLoadAsync()
        {
            await CheckoutTitleText.WaitForAsync(); 
            await CancelButton.WaitForAsync(); 
        }

        private async Task ClickContinueAsync() => await ContinueButton.ClickAsync();

        private async Task ClickCancelAsync() => await CancelButton.ClickAsync();

        private async Task FillCheckoutInfoAsync(string firstName, string lastName, string postalCode)
        {
            await FirstNameTextBox.FillAsync(firstName);
            await LastNameTextBox.FillAsync(lastName);
            await PostalCodeTextBox.FillAsync(postalCode);
        }

        public async Task<CheckoutOverviewPage> CompleteCheckoutAsync(string firstName, string lastName, string postalCode)
        {
            await FillCheckoutInfoAsync(firstName, lastName, postalCode);
            await ClickContinueAsync();

            var checkoutOverviewPage = new CheckoutOverviewPage(_page);
            await checkoutOverviewPage.WaitForPageLoadAsync();
            return checkoutOverviewPage; 
        }

        public async Task<ShoppingCartPage> CancelCheckoutAsync()
        {
            await ClickCancelAsync();

            var shoppingCartPage = new ShoppingCartPage(_page);
            await shoppingCartPage.WaitForPageLoadAsync();
            return shoppingCartPage;
        }
        #endregion
    }
}
