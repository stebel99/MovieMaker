using MovieMaker.Helpers;
using MovieMaker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieMaker.ViewModel
{
    public class ArrangeClipsViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<PanelElement> PanelElements;
        public ArrangeClipsViewModel(ObservableCollection<PanelElement> panelElements)
        {
            this.PanelElements = panelElements;
        }
        public void ArrangeClips()
        {
            var frame = (Frame)Window.Current.Content;
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
