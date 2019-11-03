using System;

namespace CoreApp.Entities.DB
{
    public partial class RandomObject
    {
        public int RandomObjectID { get; set; }
        public string RandomString { get; set; }
        public DateTimeOffset RandomDateTimeOffset { get; set; }
        public int SeedID { get; set; }
        public int RandomInt { get; set; }

        public virtual Seed Seed { get; set; }
    }
}
