using Microsoft.Toolkit.Uwp.UI.Controls;
using MovieMaker.Helpers;
using MovieMaker.Models;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieMaker.ViewModel
{
    public class TrimPageViewModel : NotifyPropertyChanged
    {
        private double min;
        private double max;
        private double minTrim;
        private double maxTrim;


        private PanelElement panelElement;
        private MediaSource mediaSource;
        private MediaComposition composition;
        private ImageSource imageSource;
        public TrimPageViewModel() : this(new PanelElement()) { }
        public TrimPageViewModel(PanelElement panelElement)
        {
            if (panelElement != null)
            {
                if (composition == null)
                {
                    composition = new MediaComposition();
                    composition.Clips.Add(panelElement.Clip);
                }

                PanelElement = panelElement;
                PanelElement.Clip = panelElement.Clip.Clone();
                MediaSource = panelElement.GetMediaSource();
                Min = 0;
                Max = panelElement.Clip.OriginalDuration.TotalSeconds;
                minTrim = PanelElement.Clip.TrimTimeFromStart.TotalSeconds;
                maxTrim = Math.Abs(PanelElement.Clip.TrimTimeFromEnd.TotalSeconds-PanelElement.Clip.OriginalDuration.TotalSeconds);
            }
        }
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                if (value != imageSource)
                {
                    imageSource = value;
                    OnPropertyChanged();
                }
            }
        }
        public PanelElement PanelElement
        {
            get
            {
                return panelElement;
            }
            set
            {
                if (value != panelElement)
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

        public double Min
        {
            get
            {
                return min;
            }
            set
            {
                if (value != min)
                {
                    min = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value != max)
                {
                    max = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MinTrim
        {
            get
            {
                return minTrim;
            }
            set
            {
                if (value != minTrim)
                {
                    minTrim = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MaxTrim
        {
            get
            {
                return maxTrim;
            }
            set
            {
                if (value != maxTrim)
                {
                    maxTrim = value;
                    OnPropertyChanged();
                }
            }
        }
        public void TrimVideo()
        {

            PanelElement.Clip.TrimTimeFromStart = TimeSpan.FromSeconds(MinTrim);
            PanelElement.Clip.TrimTimeFromEnd = TimeSpan.FromSeconds(PanelElement.Clip.OriginalDuration.TotalSeconds - MaxTrim);
            GoBack();
        }

        private void GoBack()
        {
            var frame = (Frame)Window.Current.Content;
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
