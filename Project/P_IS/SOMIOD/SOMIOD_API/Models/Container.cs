using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOMIOD_API.Models
{
    public class Container
    {
        
        public int Id { get; set; }

     
        public string Name { get; set; }

       
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;

       
        public int Parent { get; set; }

        /* Relacionamento: um container pode conter vários registros e notificações
        public ICollection<Record> Records { get; set; }
        public ICollection<Notification> Notifications { get; set; }*/
    }
}