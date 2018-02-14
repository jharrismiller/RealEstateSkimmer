using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public class PropertyTags
    {
        public int PropertyId { get; set; }
        public byte PropertyTagId { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
        [ForeignKey("PropertyTagId")]
        public virtual PropertyTag PropertyTag { get; set; }
    }
}
