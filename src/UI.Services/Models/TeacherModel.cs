﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Services.Models
{
    [Serializable]
    public class TeacherModel
    {
        public int id { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int ilosc_godzin { get; set; }
        public char[][] dostepnoscArr { get; set; }
    }
}
