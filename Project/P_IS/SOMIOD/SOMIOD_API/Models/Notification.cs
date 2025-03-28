﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOMIOD_API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int? Parent { get; set; }
        public int Event { get; set; }
        public string Endpoint { get; set; }
        public bool Enabled { get; set; }
    }

}