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
using Windows.Storage.Pickers;
using Windows.UI.Core;

namespace MovieMaker.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PanelElement> PanelElements;

        private PanelElement selectedPanelElement;
        private bool panelElementIsSelected;

        private MediaComposition composition;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel()
        {
            composition = new MediaComposition();
            PanelElements = new ObservableCollection<PanelElement>();
        }
        public MainPageViewModel(ObservableCollection<PanelElement> panelElements)
        {
            PanelElements = panelElements;
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
                element.Thumbnail = await pickedFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.VideosView);

                var clip = await AddClipAsync(pickedFile).ConfigureAwait(true);

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
        private async Task<MediaClip> AddClipAsync(StorageFile pickedFile)
        {
            MediaClip clip;
            bool isPhoto = PickedFileIsPhoto(pickedFile);

            if (isPhoto)
            {
                clip = await MediaClip.CreateFromImageFileAsync(pickedFile, TimeSpan.FromSeconds(5));
            }
            else
            {
                clip = await MediaClip.CreateFromFileAsync(pickedFile);
            }
            
            composition.Clips.Add(clip);
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
    }
}
