using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SauceTest.Models;

namespace SauceTest.Pages
{
    public class ShoppingCartPage : BasePage
    {
        public ShoppingCartPage(IPage page) : base(page) { }

        #region Properties
        public new string TitleText => titleTextSelector; 
        public List<InventoryItem> ListofItems { get; set; } = new List<InventoryItem>();
        #endregion

        #region Locators

        private const string titleTextSelector = "Your Cart"; 
        private const string continueBtnSelector = "[data-test=\"continue-shopping\"]";
        private const string checkoutBtnSelector = "[data-test=\"checkout\"]";
        private const string inventoryItemsSelector = "[data-test=\"inventory-item\"]";
        private const string inventoryItemNameSelector = "[data-test=\"inventory-item-name\"]";
        private const string inventoryItemDescSelector = "[data-test=\"inventory-item-desc\"]";
        private const string inventoryItemPriceSelector = "[data-test=\"inventory-item-price\"]";

        public ILocator CartTitleText => _page.GetByText(titleTextSelector);

        public ILocator ContinueButton => _page.Locator(continueBtnSelector);
        public ILocator CheckoutButton => _page.Locator(checkoutBtnSelector);        
        #endregion

        #region Actions


        public override async Task WaitForPageLoadAsync()
        {
            await CartTitleText.WaitForAsync();
            await ContinueButton.WaitForAsync(); 
        }

        private async Task ClickContinueAsync() => await ContinueButton.ClickAsync();

        private async Task ClickCheckoutAsync() => await CheckoutButton.ClickAsync();



        public async Task<InventoryPage> ContinueShoppingAsync()
        {
            await ClickContinueAsync();

            var inventoryPage = new InventoryPage(_page);
            await inventoryPage.WaitForPageLoadAsync();
            return inventoryPage; 

        }

        public async Task<CheckoutInformationPage> CheckoutAsync()
        {
            await ClickCheckoutAsync();

            var checkoutInfoPage = new CheckoutInformationPage(_page);
            await checkoutInfoPage.WaitForPageLoadAsync();
            return checkoutInfoPage;
        }

        
        public bool IsItemInCart(InventoryItem invItem)
        {
            var parent = _page.Locator(inventoryItemsSelector)
                            .Filter(new() { Has = _page.GetByText(invItem.Name) });
                                    
            var name = parent.Locator(inventoryItemNameSelector).TextContentAsync().Result;
            var desc = parent.Locator(inventoryItemDescSelector).TextContentAsync().Result;
            var price = parent.Locator(inventoryItemPriceSelector).TextContentAsync().Result;

            return (string.Equals(name, invItem.Name, StringComparison.CurrentCulture)) &&
                    (string.Equals(desc, invItem.Desc, StringComparison.CurrentCulture)) &&
                    (string.Equals(price, invItem.Price, StringComparison.CurrentCulture));
        }

     

        #endregion


    }
}
