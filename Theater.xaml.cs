using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LibVLCSharp.Shared;
using TeslaCamTheater.ViewModels;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace TeslaCamTheater
{
    /// <summary>
    /// Interaction logic for Theater.xaml
    /// </summary>
    public partial class Theater : Window
    {
        public TheaterViewModel ViewModel => (TheaterViewModel)Root.DataContext;

        private bool IsSliderDragging { get; set; } = false;

        LibVLC VLC { get; set; }

        public Theater(string path)
        {
            InitializeComponent();
            Initialize(path);
        }

        public void Initialize(string camPath)
        {
            Loaded += (_, __) =>
            {
                ViewModel.StartInitialization(camPath);
            };

            Core.Initialize();
            VLC = new LibVLC();

            CamViewF.Loaded += (_, __) =>
            {
                CamViewF.MediaPlayer = new MediaPlayer(VLC);
                CamViewF.MediaPlayer.PositionChanged += (_, __) =>
                {
                    if (!IsSliderDragging)
                    {
                        Dispatcher.Invoke(() => ViewModel.CurrentPosition = CamViewF.MediaPlayer.Position);
                    }
                };
                CamViewF.MediaPlayer.EndReached += (_, __) =>
                {
                    Dispatcher.InvokeAsync(async () =>
                    {
                        await Task.Run(() =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                SelectNextClip();
                            });
                        });
                    });
                };
            };
            CamViewB.Loaded += (_, __) =>
            {
                CamViewB.MediaPlayer = new MediaPlayer(VLC);
            };
            CamViewL.Loaded += (_, __) =>
            {
                CamViewL.MediaPlayer = new MediaPlayer(VLC);
            };
            CamViewR.Loaded += (_, __) =>
            {
                CamViewR.MediaPlayer = new MediaPlayer(VLC);
            };

            Closing += (_, __) =>
            {
                new Welcome().Show();
            };

            Scrubber.PreviewMouseDown += (_, __) =>
            {
                IsSliderDragging = true;
                Pause();
            };

            Scrubber.ValueChanged += (_, __) =>
            {
                if (IsSliderDragging)
                {
                    ViewModel.CurrentPosition = (float)Scrubber.Value;
                    Seek(ViewModel.CurrentPosition);
                }
            };

            Scrubber.PreviewMouseUp += (_, __) =>
            {
                IsSliderDragging = false;
                Play();
            };

            ViewModel.PropertyChanged += (s, e) =>
            {
                if (IsSliderDragging && e.PropertyName == nameof(TheaterViewModel.CurrentPosition))
                {
                    Seek(ViewModel.CurrentPosition);
                }
            };
        }

        private void Play()
        {
            CamViewF.MediaPlayer.Play();
            CamViewB.MediaPlayer.Play();
            CamViewL.MediaPlayer.Play();
            CamViewR.MediaPlayer.Play();
        }

        private void Seek(float timePct)
        {
            CamViewF.MediaPlayer.Position = timePct;
            CamViewB.MediaPlayer.Position = timePct;
            CamViewL.MediaPlayer.Position = timePct;
            CamViewR.MediaPlayer.Position = timePct;
        }

        private void Pause()
        {
            CamViewF.MediaPlayer.Pause();
            CamViewB.MediaPlayer.Pause();
            CamViewL.MediaPlayer.Pause();
            CamViewR.MediaPlayer.Pause();
        }

        private void SelectNextClip()
        {
            var curSel = ClipList.SelectedIndex;

            if (curSel + 1 < ViewModel.SelectedEvent.Clips.Count)
            {
                curSel++;
                ClipList.SelectedIndex = curSel;
            }
        }
        private void EventList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectFirstClip();
            ViewModel.CurrentPosition = 0;
            LoadVideoTimeline();
            Play();
        }

        private void Clip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedClipset != null)
            {
                ViewModel.CurrentPosition = 0;
                LoadVideoTimeline();
                Play();
            }
        }

        private void LoadVideoTimeline()
        {
            CamViewF.MediaPlayer.Media = new Media(VLC, new Uri(ViewModel.SelectedClipset.FrontCamera, UriKind.Absolute));
            CamViewF.MediaPlayer.Pause();
            CamViewF.MediaPlayer.Position = 0;

            CamViewB.MediaPlayer.Media = new Media(VLC, new Uri(ViewModel.SelectedClipset.RearCamera, UriKind.Absolute));
            CamViewB.MediaPlayer.Pause();
            CamViewB.MediaPlayer.Position = 0;

            CamViewL.MediaPlayer.Media = new Media(VLC, new Uri(ViewModel.SelectedClipset.LeftRepeaterCamera, UriKind.Absolute));
            CamViewL.MediaPlayer.Pause();
            CamViewL.MediaPlayer.Position = 0;

            CamViewR.MediaPlayer.Media = new Media(VLC, new Uri(ViewModel.SelectedClipset.RightRepeaterCamera, UriKind.Absolute));
            CamViewR.MediaPlayer.Pause();
            CamViewR.MediaPlayer.Position = 0;
        }
    }
}
