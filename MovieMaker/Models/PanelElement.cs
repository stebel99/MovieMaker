using MovieMaker.Helpers;
using Windows.Media.Editing;
using Windows.Storage.FileProperties;

namespace MovieMaker.Models
{
    public class PanelElement : NotifyPropertyChanged
    {
        private MediaClip clip;
        private StorageItemThumbnail thumbnail;

        public MediaClip Clip {
            get => clip;
            set
            {
                if (clip != value)
                {
                    clip = value;
                    OnPropertyChanged();
                }
            } 
        }
        public StorageItemThumbnail Thumbnail
        {
            get => thumbnail;
            set
            {
                if (thumbnail != value)
                {
                    thumbnail = value;
                    OnPropertyChanged();
                }
            }
        }

        public PanelElement()
        {
        }

    }
}
