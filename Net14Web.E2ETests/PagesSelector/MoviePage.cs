using OpenQA.Selenium;

namespace Net14Web.E2ETests.PagesSelector
{
    internal class MoviePage
    {
        public static By BUTTON_MOVIE_PAGE = By.CssSelector("a.movie-page");
        public static By MOVIE_TITLE = By.CssSelector(".movie-title");
    }
}
