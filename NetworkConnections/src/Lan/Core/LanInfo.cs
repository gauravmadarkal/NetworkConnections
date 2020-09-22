﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Lan.Core
{
    /// <summary>
    /// Model class which represents the ethernet connection details
    /// </summary>
    public class LanInfo
    {
        /// <summary>
        /// lan network name
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }
    }
}
