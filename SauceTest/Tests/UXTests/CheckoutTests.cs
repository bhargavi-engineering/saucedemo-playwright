using SauceTest.Pages;

namespace SauceTest.Tests.UXTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CheckoutTests : BaseTest
    {
        //TODO: Use TestCaseSource to source from external data source 
        [TestCase("standard_user", "secret_sauce", "john", "doe", "12345", 1)]
        public async Task CheckoutTest_SingleRandomItem(string username, string password, string firstname,
                                        string lastname, string postal, int itemNumber)
        {
            //Login
            var loginPage = new LoginPage(Page);
            var inventoryPage = await loginPage.LoginAsync(username, password);

            //Add inventory item to cart
            Assert.That(await inventoryPage.GetNumberOfItemsAsync(), Is.GreaterThanOrEqualTo(1), 
                "No product items exist on page");
            var item = await inventoryPage.AddToCartInventoryItemAsync(itemNumber);            

            //Go to Shopping cart
            var cartPage = await inventoryPage.GoToShoppingCartAsync();
            Assert.That(cartPage.IsItemInCart(item), Is.True, 
                $"Item: {item.Name} was not present in the Shopping Cart Page");

            //Complete Checkout 
            var checkoutInfo = await cartPage.CheckoutAsync();
            var checkoutOverview = await checkoutInfo.CompleteCheckoutAsync(firstname, lastname, postal);
            Assert.That(checkoutOverview.IsItemBeingCheckedOut(item), Is.True,
                $"Item: {item.Name} was not present in the Checkout Overview Page");
        }

    }
}
