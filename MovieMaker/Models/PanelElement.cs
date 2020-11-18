using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Storage.FileProperties;

namespace MovieMaker.Models
{
    public class PanelElement
    {
        public Guid Id { get; set; }
        public MediaSource MediaSource { get; set; }
        public StorageItemThumbnail Thumbnail { get; set; }

        public PanelElement()
        {
            Id = Guid.NewGuid();
        }

    }
}
