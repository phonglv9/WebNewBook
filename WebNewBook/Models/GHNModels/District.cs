﻿namespace WebNewBook.Models.GHNModels
{
    public class District
    {
        public int code { get; set; }
        public string ?message { get; set; }
        public IEnumerable<Data> ?data { get; set; }
        public class Data
        {
            public int DistrictID { get; set; }
            public int ProvinceID { get; set; }           
            public string ?DistrictName { get; set; }         
            //public string Code { get; set; }
            //public int Type { get; set; }
            //public int SupportType { get; set; }
            //public List<string> NameExtension { get; set; }
            //public int IsEnable { get; set; }
            //public string CreatedAt { get; set; }
            //public string UpdatedAt { get; set; }
            //public bool CanUpdateCOD { get; set; }
          
            //public int Status { get; set; }
        }
    }
}
