using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecordLabelApi.Data
{
    public class Release
    {
        [Key]
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CatalogNr { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string FrontCover { get; set; }
    }
}
