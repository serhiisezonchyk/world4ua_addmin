using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NetTopologySuite.Geometries;


namespace world4admin.ua.Models
{
    [Table("world_boundaries",Schema ="public")]
    public class empClass
    {
        [Key]
        public int gid { get; set; }

        [NotMapped]
        public string status { get; set; }
        [NotMapped]

        public string color_code { get; set; }
        [NotMapped]

        public string region { get; set; }
        [NotMapped]

        public string iso3 { get; set; }
        [NotMapped]
        public string continent { get; set; }
        public string name { get; set; }

        [NotMapped]
        public string iso_3166_1_ { get; set; }
        [NotMapped]
        public string french_shor { get; set; }
        [NotMapped]
        public System.Data.Entity.Spatial.DbGeometry geom { get; set; }

        public int? fugitive_status { get; set; }

       
        public string general_info { get; set; }

        public string entry_doc { get; set; }

        public string reg_doc { get; set; }

        public string transport { get; set; }

        public string housing { get; set; }

        public string nutrition { get; set; }

        public string pets { get; set; }

        public string charity { get; set; }

        public string add_info { get; set; }
    }
}