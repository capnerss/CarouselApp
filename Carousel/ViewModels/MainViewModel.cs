using Carousel.Resources.Localization;
using Carousel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Carousel.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Greeting => AppResource.GreetingText;
        public string ChangeLanguageLabel => AppResource.ChangeLanguage;
        public string EnglishButton => AppResource.EnglishButton;
        public string RussianButton => AppResource.RussianButton;
        public string EstonianButton => AppResource.EstonianButton;

        public ICommand SetEnglishCommand { get; }
        public ICommand SetRussianCommand { get; }
        public ICommand SetEstonianCommand { get; }

        public MainViewModel()
        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetRussianCommand = new Command(() => ChangeLanguage("ru"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));

            LanguageService.LanguageChanged += OnLanguageChanged;
        }

        private void ChangeLanguage(string code)
        {
            LanguageService.ChangeLanguage(code);
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(Greeting));
            OnPropertyChanged(nameof(ChangeLanguageLabel));
            OnPropertyChanged(nameof(EnglishButton));
            OnPropertyChanged(nameof(RussianButton));
            OnPropertyChanged(nameof(EstonianButton));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
