﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public abstract class BaseInEntity : BaseEntity
    {
        public BASEINFO BASEINFO { get; set; } 
    }
}