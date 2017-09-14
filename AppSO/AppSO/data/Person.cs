﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace AppSO.data
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int PersonID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Photo { get; set; }
    }
}