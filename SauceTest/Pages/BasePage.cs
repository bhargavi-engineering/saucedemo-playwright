using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage _page; 

        protected BasePage(IPage page)
        {
            _page = page;
        }

        #region Locators
        private const string titleTextSelector = "Swag Labs";
        private const string burgerMenuTextSelector = "Open Menu";
        private const string aboutLinkSelector = "[data-test=\"about-sidebar-link\"]";
        private const string logoutLinkSelector = "[data-test=\"logout-sidebar-link\"]";
        private const string inventoryLinkSelector = "[data-test=\"inventory-sidebar-link\"]";
        private const string resetLinkSelector = "[data-test=\"reset-sidebar-link\"]";
        private const string shoppingCartLinkSelector = "[data-test=\"shopping-cart-link\"]";
        public ILocator TitleText => _page.GetByText(titleTextSelector);
        public ILocator BurgerMenuButton => _page.GetByRole(AriaRole.Button, new() { Name = burgerMenuTextSelector });
        public ILocator AboutLink => _page.Locator(aboutLinkSelector);
        public ILocator LogoutLink => _page.Locator(logoutLinkSelector);
        public ILocator AllItemsLink => _page.Locator(inventoryLinkSelector);
        public ILocator ResetLink => _page.Locator(resetLinkSelector);
        public ILocator ShoppingCartLink => _page.Locator(shoppingCartLinkSelector);

        #endregion

        #region Actions

        public virtual async Task WaitForPageLoadAsync()
        {
            await TitleText.WaitForAsync();
        }
        public async Task OpenBurgerMenuAsync() => await BurgerMenuButton.ClickAsync();

        public async Task<LoginPage> LogoutAsync()
        {
            await OpenBurgerMenuAsync(); 
            await LogoutLink.ClickAsync();
            return new LoginPage(_page);
        }


        //TODO: Playwright suggests to use LocatorAssertions to assert for visibility 
        public async Task<bool> IsBurgerMenuVisible() => await BurgerMenuButton.IsVisibleAsync();

        public async Task<ShoppingCartPage> GoToShoppingCartAsync()
        {
            await ShoppingCartLink.ClickAsync();
            return new ShoppingCartPage(_page);            
        }

        #endregion
    }
}
