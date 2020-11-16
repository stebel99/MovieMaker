using MovieMaker.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.Pickers;

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
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
            };
            AddVideoTypesFilters(picker);

            var pickedFile = await picker.PickSingleFileAsync();

            if (pickedFile != null)
            {
                await AddClipAsync(pickedFile).ConfigureAwait(true);
            }

        }
        private static void AddVideoTypesFilters(FileOpenPicker picker)
        {
            foreach (var videoFormat in Constants.VideoFormats)
            {
                picker.FileTypeFilter.Add(videoFormat);
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
