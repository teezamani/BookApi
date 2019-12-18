using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCoreAPI.Model
{
    public class Review
    {
        public int Id { get; set; }
        public string HeadLine { get; set; }
        public string ReviewTest { get; set; }
        public int Rating { get; set; }
    }
}
