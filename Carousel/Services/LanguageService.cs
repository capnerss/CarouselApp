using Carousel.Resources.Localization;
using System.Globalization;


namespace Carousel.Services
{
    public static class LanguageService
    {
        public static event Action? LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            AppResource.Culture = culture;
            LanguageChanged?.Invoke();
            Preferences.Set("AppLanguage", languageCode);
        }
    }
}
