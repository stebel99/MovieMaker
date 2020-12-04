using MovieMaker.Helpers;
using MovieMaker.Models;
using Windows.Media.Core;
using Windows.Media.Editing;

namespace MovieMaker.ViewModel
{
    public class TrimPageViewModel : NotifyPropertyChanged
    {
        private PanelElement panelElement;
        private MediaSource mediaSource;
        private MediaComposition mediaComposition;
        public TrimPageViewModel()
        {

        }
        public TrimPageViewModel(PanelElement panelElement)
        {
            PanelElement = panelElement;
            MediaComposition = new MediaComposition();
            MediaComposition.Clips.Add(PanelElement.Clip);
            MediaSource = MediaSource.CreateFromMediaStreamSource(MediaComposition.GenerateMediaStreamSource());
        }

        public PanelElement PanelElement
        {
            get
            {
                return panelElement;
            }
            set
            {
                if (value!=panelElement)
                {
                    panelElement = value;
                    OnPropertyChanged();
                }
            }
        }

        public MediaSource MediaSource
        {
            get
            {
                return mediaSource;
            }
            set
            {
                if (value != mediaSource)
                {
                    mediaSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public MediaComposition MediaComposition
        {
            get
            {
                return mediaComposition;
            }
            set
            {
                if (value != mediaComposition)
                {
                    mediaComposition = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
