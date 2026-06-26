using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SauceTest.Models; 

namespace SauceTest.Pages
{
    public class CheckoutOverviewPage: BasePage
    {
        public CheckoutOverviewPage(IPage page): base(page)
        {
            this.WaitForPageLoadAsync().ConfigureAwait(false);
        }

        #region Locators
        private const string titleTextSelector = "Checkout: Overview"; 
        private const string cancelBtnSelector = "[data-test=\"cancel\"]";
        private const string finishBtnSelector = "[data-test=\"finish\"]";
        private const string inventoryItemsSelector = "[data-test=\"inventory-item\"]";
        private const string inventoryItemNameSelector = "[data-test=\"inventory-item-name\"]";
        private const string inventoryItemDescSelector = "[data-test=\"inventory-item-desc\"]";
        private const string inventoryItemPriceSelector = "[data-test=\"inventory-item-price\"]";

        public ILocator FinishButton => _page.Locator(finishBtnSelector);
        public ILocator CancelButton => _page.Locator(cancelBtnSelector);
        public ILocator CheckoutTitleText => _page.GetByText(titleTextSelector);


        #endregion

        #region Actions
        public async override Task WaitForPageLoadAsync()
        {
            await CheckoutTitleText.WaitForAsync();
            await FinishButton.WaitForAsync();
        }

        private async Task ClickFinishAsync() => await FinishButton.ClickAsync();

        private async Task ClickCancelAsync() => await CancelButton.ClickAsync();

        public async Task<InventoryPage> CancelCheckoutAsync()
        {
            await ClickCancelAsync();

            var inventoryPage = new InventoryPage(_page);
            await inventoryPage.WaitForPageLoadAsync();
            return inventoryPage;
        }

        public bool IsItemBeingCheckedOut(InventoryItem item)
        {
            var parent = _page.Locator(inventoryItemsSelector)
                            .Filter(new() { Has = _page.GetByText(item.Name) });

            var name = parent.Locator(inventoryItemNameSelector).TextContentAsync().Result;
            var desc = parent.Locator(inventoryItemDescSelector).TextContentAsync().Result;
            var price = parent.Locator(inventoryItemPriceSelector).TextContentAsync().Result;

            return (string.Equals(name, item.Name, StringComparison.CurrentCulture)) &&
                    (string.Equals(desc, item.Desc, StringComparison.CurrentCulture)) &&
                    (string.Equals(price, item.Price, StringComparison.CurrentCulture));
        }
        #endregion
    }
}
