using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOMIOD_API.Models
{
    public class Record
    {
       
        public int Id { get; set; }

     
        public string Name { get; set; }

    
        public string Content { get; set; } // Conteúdo do registro

     
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;

       
        public int Parent { get; set; }
    }
}