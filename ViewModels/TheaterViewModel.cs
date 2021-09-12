using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TeslaCamTheater.Models;
using TeslaCamTheater.Services;

namespace TeslaCamTheater.ViewModels
{
    public class TheaterViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Event> _Events;
        public ObservableCollection<Event> Events
        {
            get => _Events;
            set => SetField(ref _Events, value);
        }

        private Event _SelectedEvent;
        public Event SelectedEvent
        {
            get => _SelectedEvent;
            set
            {
                SetField(ref _SelectedEvent, value);
            }
        }

        private Clipset _SelectedClipset;
        public Clipset SelectedClipset
        {
            get => _SelectedClipset;
            set
            {
                if (value != null)
                    Debug.WriteLine("Selected clipset has " + (value.HasEvent ? "an" : "NO") + " event." + (value.HasEvent ? " Offset:" + value.EventOffset : ""));
                SetField(ref _SelectedClipset, value);
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetField(ref _IsLoading, value);
        }

        private float _CurrentPosition;
        public float CurrentPosition
        {
            get => _CurrentPosition;
            set => SetField(ref _CurrentPosition, value);
        }

        public void StartInitialization(string folder)
        {
            Task.Run(async () =>
            {
                IsLoading = true;

                var events = await FileService.LoadEvents(folder);
                Events = new ObservableCollection<Event>(events);
                CurrentPosition = 0;

                IsLoading = false;
            });
        }

        public void SelectFirstClip()
        {
            if (SelectedEvent != null)
            {
                SelectedClipset = SelectedEvent.Clips.First();
            }
        }

        public bool TryAdvanceToNextClip()
        {
            if (SelectedEvent == null)
            {
                return false;
            }
            if (SelectedClipset == null)
            {
                SelectedClipset = SelectedEvent.Clips.First();
                return true;
            }
            var clipPos = SelectedEvent.Clips.IndexOf(SelectedClipset);
            if (clipPos + 1 < SelectedEvent.Clips.Count)
            {
                SelectedClipset = SelectedEvent.Clips[clipPos + 1];
                return true;
            }
            return false;
        }

        #region INotifyPropertyChanged Impl

        protected void SetField<T>(ref T prop, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(prop, value))
            {
                prop = value;
                OnPropertyChanged(propertyName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}
