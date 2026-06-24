using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SauceTest.Models;
using System.Reflection.Metadata.Ecma335;

namespace SauceTest.Pages
{
    public class InventoryPage: BasePage
    {
        public InventoryPage(IPage page): base(page) { }

        #region Properties
        
        #endregion

        #region Locators
        private const string titleTextSelector = "Products";
        private const string inventoryItemsSelector = "[data-test=\"inventory-item\"]";
        private const string inventoryItemNameSelector = "[data-test=\"inventory-item-name\"]";
        private const string inventoryItemDescSelector = "[data-test=\"inventory-item-desc\"]";
        private const string inventoryItemPriceSelector = "[data-test=\"inventory-item-price\"]";
        private const string addToCartButtonNameSelector = "Add to cart";

        public ILocator ProductsTitleText => _page.GetByText(titleTextSelector);
        public ILocator InventoryItemsDiv => _page.Locator(inventoryItemsSelector);
        public ILocator AddToCartButton => _page.GetByRole(AriaRole.Button, 
                                                           new() { Name = addToCartButtonNameSelector });
        
        #endregion

        #region Actions

        public override async Task WaitForPageLoadAsync()
        {
            await ProductsTitleText.WaitForAsync();
        }


        public async Task<InventoryItem> AddToCartInventoryItemAsync(int inventoryItemIndex)
        {
            if (inventoryItemIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(inventoryItemIndex),
                    "Index cannot be negative.");

            var count = await InventoryItemsDiv.CountAsync();
            if (inventoryItemIndex >= count)
                throw new ArgumentOutOfRangeException(nameof(inventoryItemIndex),
                    $"Index {inventoryItemIndex} is out of range. " +
                    $"Only {count} items found on the page.");

            var inventoryItemDiv = InventoryItemsDiv.Nth(inventoryItemIndex);
            var inventoryItem = await GetSelectedInventoryItemDetailsAsync(inventoryItemDiv); 
            await inventoryItemDiv.GetByRole(AriaRole.Button, new() { Name = addToCartButtonNameSelector}).ClickAsync();
            return inventoryItem; 
        }

        public async Task<int> GetNumberOfItemsAsync()
        {
            var count = await InventoryItemsDiv.CountAsync().ConfigureAwait(false);
            return count; 
        }
        private async Task<InventoryItem> GetSelectedInventoryItemDetailsAsync(ILocator inventoryItemParentDiv)
        {
            return new InventoryItem
            {
                ImageLink = await inventoryItemParentDiv.Locator("img").GetAttributeAsync("src"),
                Name = await inventoryItemParentDiv.Locator(inventoryItemNameSelector).TextContentAsync(),
                Desc = await inventoryItemParentDiv.Locator(inventoryItemDescSelector).TextContentAsync(),
                Price = await inventoryItemParentDiv.Locator(inventoryItemPriceSelector).TextContentAsync()
            };
            
        }

        #endregion
    }
}
