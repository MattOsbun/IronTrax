﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Api.Activity.Models
{
    public class ActivityForCreate
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public string TimeLogged = DateTime.Now.ToString();
        public string Channel { get; set; }
    }

    public class ActivityForUpdate
    {
        public string Description { get; set; }
    }

    public class ActivityTableEntity : TableEntity
    {
        public string TimeLogged { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Channel { get; set; }
    }
}
