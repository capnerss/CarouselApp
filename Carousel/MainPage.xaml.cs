using Carousel.Resources.Localization;
using Carousel.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Carousel
{
	public partial class MainPage : ContentPage, INotifyPropertyChanged
	{
		public bool onAuto = true;
		public new event PropertyChangedEventHandler? PropertyChanged;

		private string currentLanguage = "et"; 

		public string Greeting => AppResource.GreetingText;
		public string ChangeLanguageLabel => AppResource.ChangeLanguage;
		public string EnglishButton => AppResource.EnglishButton;
		public string RussianButton => AppResource.RussianButton;
		public string EstonianButton => AppResource.EstonianButton;

		public ICommand SetEnglishCommand { get; }
		public ICommand SetRussianCommand { get; }
		public ICommand SetEstonianCommand { get; }

		
		public class CarouselItem : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler? PropertyChanged;

			public string Title { get; set; } = string.Empty;
			public string ImageUrl { get; set; } = string.Empty;
			public string DescriptionEn { get; set; } = string.Empty;
			public string DescriptionEt { get; set; } = string.Empty;
			public string HelloWorld { get; set; } = string.Empty;
			public int YearCreated { get; set; }

			
			private string currentLang = "et";
			public string Description => currentLang == "et" ? DescriptionEt : DescriptionEn;

			public void UpdateLanguage(string lang)
			{
				currentLang = lang;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
			}
		}

		private CarouselView carouselView;
		private ObservableCollection<CarouselItem> items;
		private int position = 0;

		public MainPage()
		{
			BindingContext = this;

			SetEnglishCommand = new Command(() => ChangeLanguage("en"));
			SetRussianCommand = new Command(() => ChangeLanguage("ru"));
			SetEstonianCommand = new Command(() => ChangeLanguage("et"));

			Title = "Programmeerimiskeeled";

			
			items = new ObservableCollection<CarouselItem>
			{
				new CarouselItem
				{
					Title = "C#",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png",
					DescriptionEn = "Powerful object-oriented language",
					DescriptionEt = "Võimas objektorienteeritud keel",
					YearCreated = 2000,
					HelloWorld = "Console.WriteLine(\"Hello, World!\");"
				},
				new CarouselItem
				{
					Title = "Python",
					ImageUrl = "https://images.icon-icons.com/2699/PNG/512/python_logo_icon_168886.png",
					DescriptionEn = "Great for data and automation",
					DescriptionEt = "Suurepärane andmetöötluseks ja automatiseerimiseks",
					YearCreated = 1991,
					HelloWorld = "print(\"Hello, World!\")"
				},
				new CarouselItem
				{
					Title = "JavaScript",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6a/JavaScript-logo.png",
					DescriptionEn = "The language of the web",
					DescriptionEt = "Veebiarenduse põhikeel",
					YearCreated = 1995,
					HelloWorld = "console.log(\"Hello, World!\");"
				},
				new CarouselItem
				{
					Title = "Java",
					ImageUrl = "https://logos-world.net/wp-content/uploads/2022/07/Java-Logo.jpg",
					DescriptionEn = "Write once, run anywhere",
					DescriptionEt = "Kirjuta kord, käivita igal pool",
					YearCreated = 1995,
					HelloWorld = "System.out.println(\"Hello, World!\");"
				},
				new CarouselItem
				{
					Title = "C++",
					ImageUrl = "https://download.logo.wine/logo/C%2B%2B/C%2B%2B-Logo.wine.png",
					DescriptionEn = "High-performance system language",
					DescriptionEt = "Suure jõudlusega süsteemikeel",
					YearCreated = 1985,
					HelloWorld = "std::cout << \"Hello, World!\";"
				}
			};

			
			carouselView = new CarouselView
			{
				ItemsSource = items,
				HeightRequest = 380,
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

					
					var image = new Image
					{
						Aspect = Aspect.AspectFit,
						Margin = new Thickness(20),
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center
					};
					image.SetBinding(Image.SourceProperty, nameof(CarouselItem.ImageUrl));

					
					var gradient = new BoxView
					{
						Background = new LinearGradientBrush
						{
							StartPoint = new Point(0, 1),
							EndPoint = new Point(0, 0),
							GradientStops = new GradientStopCollection
							{
								new GradientStop(Colors.Black.WithAlpha(0.85f), 0),
								new GradientStop(Colors.Transparent, 1)
							}
						}
					};

					
					var textStack = new VerticalStackLayout
					{
						Spacing = 4,
						Margin = new Thickness(15),
						VerticalOptions = LayoutOptions.End
					};

					var titleLabel = new Label
					{
						TextColor = Colors.White,
						FontSize = 24,
						FontAttributes = FontAttributes.Bold
					};
					titleLabel.SetBinding(Label.TextProperty, nameof(CarouselItem.Title));

					var descLabel = new Label
					{
						TextColor = Colors.LightGray,
						FontSize = 14,
						LineBreakMode = LineBreakMode.WordWrap
					};
					descLabel.SetBinding(Label.TextProperty, nameof(CarouselItem.Description));

					textStack.Children.Add(titleLabel);
					textStack.Children.Add(descLabel);

					grid.Children.Add(image);
					grid.Children.Add(gradient);
					grid.Children.Add(textStack);

					frame.Content = grid;

					
					var tapGesture = new TapGestureRecognizer();
					tapGesture.Tapped += async (sender, e) =>
					{
						var tappedFrame = (Frame)sender;
						var selectedItem = (CarouselItem)tappedFrame.BindingContext;

						if (selectedItem != null)
						{
							onAuto = false; 

							string message = currentLanguage == "et"
								? $"Loomise aasta: {selectedItem.YearCreated}\n\n\"Hello World\" näide:\n{selectedItem.HelloWorld}"
								: $"Year created: {selectedItem.YearCreated}\n\n\"Hello World\" example:\n{selectedItem.HelloWorld}";

							await Application.Current.MainPage.DisplayAlert(selectedItem.Title, message, "OK");
						}
					};

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
				Margin = new Thickness(0, 10, 0, 0)
			};

			var etNupp = new ImageButton
			{
				Source = "https://www.vohandumaraton.ee/wp-content/uploads/2017/12/estonia-flag-button-square-icon-256.png",
				BackgroundColor = Colors.DarkSlateBlue,
				CornerRadius = 10,
				Margin = new Thickness(0, 10, 0, 0),
				WidthRequest = 36,
				HeightRequest = 36
			};
			etNupp.SetBinding(ImageButton.CommandProperty, nameof(SetEstonianCommand));

			var enNupp = new ImageButton
			{
				Source = "https://cdn.britannica.com/44/344-050-94536674/Flag-England.jpg",
				BackgroundColor = Colors.DarkSlateBlue,
				CornerRadius = 10,
				Margin = new Thickness(0, 10, 0, 0),
				WidthRequest = 36,
				HeightRequest = 36
			};
			enNupp.SetBinding(ImageButton.CommandProperty, nameof(SetEnglishCommand));

			lisaNupp.Clicked += (sender, e) =>
			{
				onAuto = !onAuto;
				lisaNupp.Text = onAuto ? "Auto ON" : "Auto OFF";
			};

			
			Device.StartTimer(TimeSpan.FromSeconds(4), () =>
			{
				if (!onAuto) return true;
				if (items.Count > 0)
				{
					position = (position + 1) % items.Count;
					carouselView.Position = position;
				}
				return true;
			});

			var label2 = new Label
			{
				TextColor = Colors.Black,
				FontSize = 20,
				FontAttributes = FontAttributes.Bold,
				Margin = new Thickness(10, 0)
			};
			label2.SetBinding(Label.TextProperty, nameof(Greeting));

			Content = new ScrollView
			{
				Content = new VerticalStackLayout
				{
					Padding = 20,
					Spacing = 15,
					Children =
					{
						label2,
						new HorizontalStackLayout
						{
							Spacing = 10,
							Children = { etNupp, enNupp }
						},
						carouselView,
						indicatorView,
						lisaNupp
					}
				}
			};
		}

		private void ChangeLanguage(string code)
		{
			currentLanguage = code;
			LanguageService.ChangeLanguage(code);

			
			if (items != null)
			{
				foreach (var item in items)
				{
					item.UpdateLanguage(code);
				}
			}

			OnLanguageChanged();
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