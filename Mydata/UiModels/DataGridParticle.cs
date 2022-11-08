using System;

namespace Mydata.UiModels
{
    public class DataGridParticle
    {
        public string Date { get; set; }
        public string Branch { get; set; }
        public string Series { get; set; }
        public string Client { get; set; }
        public string PtyParDescription { get; set; }
        public string Amount { get; set; }

        public Guid DataGridId { get; set; }
       
    }
}
