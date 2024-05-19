﻿using Kudos.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.Databases.Enums
{
    [Flags]
    public enum EDatabaseInformationSchemaType
    {
        Tables = CBinaryFlag._1,
        Columns = CBinaryFlag._2
    }
}