using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceAllocation.Domain
{
    public class ShowEntity : BaseEntity
    {
        public string Designer { get; set; }
        public DateTime Date { get; set; }
        public string FashionModelsName { get; set; }

        //public List<string> fashionModels;

        //public List<string> FashionModels
        //{
        //    get { return fashionModels; }
        //    set { fashionModels = value; }
        //}
    }
}
