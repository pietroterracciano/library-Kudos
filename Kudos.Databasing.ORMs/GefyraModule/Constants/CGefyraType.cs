﻿using System;
using System.Linq.Expressions;
using Kudos.Databasing.ORMs.GefyraModule.Enums;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Entities;

namespace Kudos.Databasing.ORMs.GefyraModule.Constants
{
	public static class CGefyraType
	{
		public static readonly Type
			IGefyraTable = typeof(IGefyraTable),
			IGefyraColumn = typeof(IGefyraColumn),
			EGefyraJoin = typeof(EGefyraJoin),
            EGefyraPost = typeof(EGefyraPost);

		internal static readonly Type
			Expression = typeof(Expression),
			LambdaExpression = typeof(LambdaExpression);
    }
}

