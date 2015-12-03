using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpandListView2 {
    class OneData {
        public string Title { get; set; }
        public string MediaUrl { get; set; }
        public string SourceUrl { get; set; }
        public string DisplayUrl { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string ContentType { get; set; }
        public bool Expand { get; set; }
        public string FileSize { get; set; }

        public string Size {
            get {
                var fsize = Int64.Parse(FileSize);
                return string.Format("W:{0}×H:{1} {0}Kbyte", Width,Height, fsize / 1000);
            }
        }

        public OneData() {
            Expand = false;
        }
    }
}
