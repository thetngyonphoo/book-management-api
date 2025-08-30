using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Application.Dto
{
    public class DataListRequestModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }       
        public Params? param { get; set; }
    }
    public class Params
    {
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
       
    }
}
