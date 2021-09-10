using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TeslaCamTheater.Models;
using TeslaCamTheater.Services;

namespace TeslaCamTheater
{
    /// <summary>
    /// Interaction logic for Theater.xaml
    /// </summary>
    public partial class Theater : Window
    {
        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register(nameof(Events), typeof(ObservableCollection<Event>), typeof(Theater), new PropertyMetadata(null));
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(Theater), new PropertyMetadata(false));
        public static readonly DependencyProperty CurrentClipsetProperty = DependencyProperty.Register("CurrentClipset", typeof(Clipset), typeof(Theater), new PropertyMetadata(null));

        public Clipset CurrentClipset
        {
            get { return (Clipset)GetValue(CurrentClipsetProperty); }
            set { SetValue(CurrentClipsetProperty, value); }
        }

        public ObservableCollection<Event> Events
        {
            get { return (ObservableCollection<Event>)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public Theater(string camPath)
        {
            InitializeComponent();
            IsLoading = true;

            Closing += (_, __) =>
            {
                new Welcome().Show();
            };

            Task.Run(async () =>
            {
                var events = await FileService.LoadEvents(camPath);
                Dispatcher.Invoke(() => Events = new ObservableCollection<Event>(events));
                Dispatcher.Invoke(() => CurrentClipset = Events.First().Clips.First());
                Dispatcher.Invoke(() => IsLoading = false);
            });
        }
    }
}
