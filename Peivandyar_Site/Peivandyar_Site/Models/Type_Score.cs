﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Type_Score
    {
        public Type_Score() { }

        public int id { get; set; }

        public string caption { get; set; }

        public string comment { get; set; }

    }
}