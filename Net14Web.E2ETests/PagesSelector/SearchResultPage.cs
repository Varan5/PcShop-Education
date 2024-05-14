using OpenQA.Selenium;

namespace Net14Web.E2ETests.PagesSelector
{
    public class SearchResultPage
    {
        public static By City => By.Name("city");
        public static By SubmitButton => By.CssSelector("[type=submit]");
    }
}
