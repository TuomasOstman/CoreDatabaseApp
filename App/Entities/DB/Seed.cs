namespace CoreApp.Entities
{
    using CoreApp.Entities.DB;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Seed
    {
        public Seed()
        {
            this.RandomObject = new HashSet<RandomObject>();
        }
        [Key]
        public int SeedID { get; set; }
        public int SeedValue { get; set; }

        public virtual ICollection<RandomObject> RandomObject { get; set; }
    }
}