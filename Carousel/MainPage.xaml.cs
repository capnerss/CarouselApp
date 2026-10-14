using Carousel.Resources.Localization;
using Carousel.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Carousel
{
    public partial class MainPage : ContentPage
    {
        public bool onAuto = true;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Greeting => AppResource.GreetingText;
        public string ChangeLanguageLabel => AppResource.ChangeLanguage;
        public string EnglishButton => AppResource.EnglishButton;
        public string RussianButton => AppResource.RussianButton;
        public string EstonianButton => AppResource.EstonianButton;

        public ICommand SetEnglishCommand { get; }
        public ICommand SetRussianCommand { get; }
        public ICommand SetEstonianCommand { get; }
        public class CarouselItem

        {
            public string Title { get; set; }

            public string ImageUrl { get; set; }

        }

        private CarouselView carouselView;

        // List asendati ObservableCollectioniga

        private ObservableCollection<CarouselItem> items;

        private int position = 0;

        public MainPage()

        {
            SetEnglishCommand = new Command(() => ChangeLanguage("en"));
            SetRussianCommand = new Command(() => ChangeLanguage("ru"));
            SetEstonianCommand = new Command(() => ChangeLanguage("et"));
            Title = "Karussell";
            // Initsialiseerime ObservableCollectioni
            items = new ObservableCollection<CarouselItem>

        {
            new CarouselItem { Title = "Päikesetõus", ImageUrl = "https://picsum.photos/id/1015/600/400" },
            new CarouselItem { Title = "Metsavaikus", ImageUrl = "https://picsum.photos/id/1016/600/400" },
            new CarouselItem { Title = "Järvepeegel", ImageUrl = "https://picsum.photos/id/1018/600/400" }

        };
            // Karusselli loomine (kood on sama, mis eelmises versioonis)

            carouselView = new CarouselView

            {
                ItemsSource = items,
                HeightRequest = 350,
                PeekAreaInsets = new Thickness(40, 0, 40, 0),
                ItemTemplate = new DataTemplate(() =>

                {

                    var frame = new Frame

                    {
                        CornerRadius = 15,
                        HasShadow = true,
                        Padding = 0,
                        Margin = new Thickness(5),
                        BackgroundColor = Colors.Black
                    };

                    var grid = new Grid();
                    var image = new Image { Aspect = Aspect.AspectFill };
                    image.SetBinding(Image.SourceProperty, "ImageUrl");

                    var gradient = new BoxView

                    {

                        Background = new LinearGradientBrush

                        {
                            StartPoint = new Point(0, 1),
                            EndPoint = new Point(0, 0),
                            GradientStops = new GradientStopCollection

                        {

                            new GradientStop(Colors.Black.WithAlpha(0.7f), 0),
                            new GradientStop(Colors.Transparent, 1)

                        }

                        }

                    };

                    var label = new Label

                    {
                        TextColor = Colors.White,
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(15),
                        VerticalOptions = LayoutOptions.End
                    };


                    label.SetBinding(Label.TextProperty, "Title");
                    grid.Children.Add(image);
                    grid.Children.Add(gradient);
                    grid.Children.Add(label);


                    frame.Content = grid;
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += async (sender, e) =>
                    {
                        
                        var tappedFrame = (Frame)sender;
                        var selectedItem = (CarouselItem)tappedFrame.BindingContext;

                        if (selectedItem != null)
                        {
                            
                            onAuto = false;

                            
                            await Application.Current.MainPage.DisplayAlert("Picture Selected", $"You clicked on: {selectedItem.Title}", "OK");
                        }
                    };

                    // Attach the gesture to the frame
                    frame.GestureRecognizers.Add(tapGesture);

                    return frame;

                })

            };



            var indicatorView = new IndicatorView

            {
                IndicatorColor = Colors.LightGray,
                SelectedIndicatorColor = Colors.DarkSlateBlue,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)
            };

            carouselView.IndicatorView = indicatorView;

            var lisaNupp = new Button

            {
                Text = "Auto ON",
                BackgroundColor = Colors.DarkSlateBlue,
                TextColor = Colors.White,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0)
            };
            var etNupp = new ImageButton

            {
                Source = "https://www.vohandumaraton.ee/wp-content/uploads/2017/12/estonia-flag-button-square-icon-256.png",
                BackgroundColor = Colors.DarkSlateBlue,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0),
                WidthRequest = 16,
                HeightRequest = 16,

            };//.SetBinding("SetEstonianCommand");
            etNupp.SetBinding(ImageButton.CommandProperty, "SetEstonianCommand");
            var enNupp = new ImageButton

            {
                Source = "https://cdn.britannica.com/44/344-050-94536674/Flag-England.jpg",
                BackgroundColor = Colors.DarkSlateBlue,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0),
                WidthRequest = 16,
                HeightRequest = 16,
            };
            enNupp.SetBinding(ImageButton.CommandProperty, "SetEnglishCommand");


            // Nupu vajutamise sündmus

            lisaNupp.Clicked += (sender, e) =>

            {
                if (onAuto == false) {
                    onAuto = true;
                    lisaNupp.Text = "Auto ON";
                    Device.StartTimer(TimeSpan.FromSeconds(4), () =>

                    {
                        if (onAuto == false)
                        {
                            return false;
                        }

                        else
                        {
                            position = (position + 1) % items.Count;

                            carouselView.Position = position;

                            return true;
                        }

                    });
                }
                else
                {
                    onAuto = false;
                    lisaNupp.Text = "Auto OFF";
                    Device.StartTimer(TimeSpan.FromSeconds(4), () =>

                    {
                        if (onAuto == false)
                        {
                            return false;
                        }

                        else
                        {
                            position = (position + 1) % items.Count;

                            carouselView.Position = position;

                            return true;
                        }

                    });
                }

            };

            // Automaatne kerimine
            Device.StartTimer(TimeSpan.FromSeconds(4), () =>

            {
                if (onAuto == false)
                {
                    return false;
                }

                else
                {
                    position = (position + 1) % items.Count;

                    carouselView.Position = position;

                    return true;
                }

            });

            var label2 = new Label

            {
                Text = "Placeholder",
                TextColor = Colors.Black,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(15),
            };


            label2.SetBinding(Label.TextProperty, "EnglishButton");




            Content = new ScrollView

            {

                Content = new VerticalStackLayout

                {
                    Padding = 20,
                    Spacing = 20, // Jätab elementide vahele ilusa tühimiku
                    Children =

                {
                    label2,
                    etNupp,
                    enNupp,

                    carouselView,

                    indicatorView,

                    lisaNupp

                }

                }

            };

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