﻿using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Clausoles;
using Kudos.Databasing.ORMs.GefyraModule.Types;

namespace Kudos.Databasing.ORMs.GefyraModule.Interfaces.Builders
{
    public interface
        IGefyraSelectClausoleBuilder 
    :
        IGefyraCountClausole,
        IGefyraFromClausole
    {
    }
}