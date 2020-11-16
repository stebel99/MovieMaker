using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;

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
                if (mediaSource != value)
                {
                    mediaSource = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CompositionIsNotEmpty
        {
            get { return compositionIsNotEmpty; }
            set
            {
                if (compositionIsNotEmpty != value)
                {
                    compositionIsNotEmpty = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPageViewModel()
        {
            composition = new MediaComposition();
            mediaSource = null;
            compositionIsNotEmpty = false;
        }

        public async Task PickFileAndAddClipAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary,
            };
            picker.FileTypeFilter.Add(".mp4");

            var pickedFile = await picker.PickSingleFileAsync();

            if (pickedFile != null)
            {
                await AddClipAsync(pickedFile).ConfigureAwait(true);
            }

        }

        private async Task AddClipAsync(StorageFile pickedFile)
        {
            var clip = await MediaClip.CreateFromFileAsync(pickedFile);

            composition.Clips.Add(clip);
            CompositionIsNotEmpty = true;

            var mediaStreamSource = composition.GenerateMediaStreamSource();
            MediaSource = MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
        }
    }
}
