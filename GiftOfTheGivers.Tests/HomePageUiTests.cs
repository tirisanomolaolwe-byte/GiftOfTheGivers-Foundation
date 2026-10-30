
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GiftOfTheGivers.Tests;

[TestClass]
public class HomePageUiTests
{
    [TestMethod]
    public void HomePage_LoadsSuccessfully()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1365,900");

        using var driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl("http://localhost:5164");

        Assert.IsTrue(
            driver.Title.Length > 0,
            "The home page should have a page title.");
    }
}
