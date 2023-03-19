using System;
using System.Collections.Generic;

namespace DYClubs.Models
{
    public partial class ArtistStyle
    {
        public int ArtistId { get; set; }
        public string StyleName { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Style StyleNameNavigation { get; set; }
    }
}
