using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;

namespace MovieMaker.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private MediaComposition composition;
        private MediaSource mediaSource;

        private bool compositionIsNotEmpty;


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MediaSource MediaSource
        {
            get { return mediaSource; }
            set
            {
                mediaSource = value;
                OnPropertyChanged(nameof(MediaSource));
            }
        }

        public bool CompositionIsNotEmpty
        {
            get { return compositionIsNotEmpty; }
            set
            {
                compositionIsNotEmpty = value;
                OnPropertyChanged(nameof(CompositionIsNotEmpty));
            }
        }


        public MainPageViewModel()
        {
            composition = new MediaComposition();
            mediaSource = null;
            compositionIsNotEmpty = false;
        }


        public async Task PickFileAndAddClip()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");

            var pickedFile = await picker.PickSingleFileAsync();

            if (pickedFile == null)
            {
                return;
            }

            await AddClip(pickedFile);
        }

        public async Task AddClip(StorageFile pickedFile)
        {
            var clip = await MediaClip.CreateFromFileAsync(pickedFile);

            composition.Clips.Add(clip);
            CompositionIsNotEmpty = true;

            var mediaStreamSource = composition.GenerateMediaStreamSource();
            MediaSource = MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
        }
    }
}
