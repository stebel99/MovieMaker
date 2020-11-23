using MovieMaker.Helpers;
using MovieMaker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieMaker.ViewModel
{
    public class MainPageViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<PanelElement> PanelElements;

        private PanelElement selectedPanelElement;
        private bool panelElementIsSelected;

        private MediaSource mediaSource;

        private MediaComposition composition;

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public MainPageViewModel()
        {
            composition = new MediaComposition();
            PanelElements = new ObservableCollection<PanelElement>();
        }
        public MainPageViewModel(ObservableCollection<PanelElement> panelElements)
        {
            PanelElements = panelElements;
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

        public PanelElement SelectedPanelElement
        {
            get { return selectedPanelElement; }
            set
            {
                if (selectedPanelElement != value && value != null)
                {
                    selectedPanelElement = value;
                    OnPropertyChanged();
                    PanelElementIsSelected = true;
                    PanelElementChanged();
                }
            }
        }

        public bool PanelElementIsSelected
        {
            get { return panelElementIsSelected; }
            set
            {
                if (panelElementIsSelected != value)
                {
                    panelElementIsSelected = value;
                    OnPropertyChanged();
                }
            }
        }


        public async Task PickFileAndAddClipAsync()
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
            };

            AddTypesFilters(picker, Constants.VideoFormats);
            AddTypesFilters(picker, Constants.PhotoFormats);

            picker.ViewMode = PickerViewMode.Thumbnail;


            var pickedFile = await picker.PickSingleFileAsync();

            if (pickedFile != null)
            {
                PanelElement element = new PanelElement();
                element.Name = pickedFile.Name;
                element.Clip = await AddClipAsync(pickedFile, element).ConfigureAwait(true);

                PanelElements.Add(element);
            }
        }
        private static void AddTypesFilters(FileOpenPicker picker, string[] formats)
        {
            foreach (var format in formats)
            {
                picker.FileTypeFilter.Add(format);
            }
        }
        private async Task<MediaClip> AddClipAsync(StorageFile pickedFile, PanelElement element)
        {
            MediaClip clip;
            bool isPhoto = PickedFileIsPhoto(pickedFile);

            if (isPhoto)
            {
                using (StorageItemThumbnail thumbnail = await pickedFile.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);

                        element.Thumbnail = bitmapImage;
                    }
                }
                clip = await MediaClip.CreateFromImageFileAsync(pickedFile, TimeSpan.FromSeconds(5));
            }
            else
            {
                using (StorageItemThumbnail thumbnail = await pickedFile.GetThumbnailAsync(ThumbnailMode.VideosView))
                {
                    if (thumbnail != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);

                        element.Thumbnail = bitmapImage;
                    }
                }
                clip = await MediaClip.CreateFromFileAsync(pickedFile);
            }
            
            return clip;
        }

        private static bool PickedFileIsPhoto(StorageFile pickedFile)
        {
            bool result = false;

            foreach (var format in Constants.PhotoFormats)
            {
                if (format == pickedFile.FileType)
                {
                    result = true;
                }
            }

            return result;
        }

        public void PanelElementChanged()
        {
            var clip = SelectedPanelElement.Clip.Clone();
            var mediaComposition = new MediaComposition();
            mediaComposition.Clips.Add(clip);

            MediaSource = MediaSource.CreateFromMediaStreamSource(mediaComposition.GenerateMediaStreamSource());
        }
    }
}
