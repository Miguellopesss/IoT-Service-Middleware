using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SOMIOD_API.Models
{
    public class Application
    {
       
        public int Id { get; set; }

      
        public string Name { get; set; }

        
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;

        /* Relacionamento: uma aplicação pode conter vários containers
        public ICollection<Container> Containers { get; set; }*/
    }
}