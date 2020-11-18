using MovieMaker.Helpers;
using MovieMaker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
            };
            AddVideoTypesFilters(picker);

            var pickedFile = await picker.PickSingleFileAsync();

            if (pickedFile != null)
            {
                PanelElement element = new PanelElement();
                element.Thumbnail = await pickedFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.VideosView);
                element.MediaSource = MediaSource.CreateFromStorageFile(pickedFile);

                PanelElements.Add(element);

                await AddClipAsync(pickedFile).ConfigureAwait(true);
            }

        }

        private static void AddVideoTypesFilters(FileOpenPicker picker)
        {
            foreach (var videoFormat in Constants.Formats)
            {
                picker.FileTypeFilter.Add(videoFormat);
            }
        }
        private async Task AddClipAsync(StorageFile pickedFile)
        {
            var clip = await MediaClip.CreateFromFileAsync(pickedFile);
            composition.Clips.Add(clip);
        }
    }
}
