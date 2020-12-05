using MovieMaker.Helpers;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieMaker.Models
{
    public class PanelElement : NotifyPropertyChanged
    {
        private MediaClip clip;
        private BitmapImage thumbnail;
        private string name;
        private FileType fileType;
        private StorageFile storageFile;

        public MediaSource GetMediaSource () => MediaSource.CreateFromStorageFile(StorageFile);
        public bool IsPicture { get => this.FileType == FileType.Picture; }
        public bool IsVideo { get => this.FileType == FileType.Video; }

        public FileType FileType
        {
            get => fileType;
            set
            {
                if (fileType != value)
                {
                    fileType = value;
                    OnPropertyChanged();
                }
            }
        }
        public StorageFile StorageFile
        {
            get => storageFile;
            set
            {
                if (storageFile != value)
                {
                    storageFile = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public MediaClip Clip {
            get => clip;
            set
            {
                if (clip != value)
                {
                    clip = value;
                    OnPropertyChanged(nameof(PanelElement));

                }
            } 
        }
        public BitmapImage Thumbnail
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
