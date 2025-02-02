﻿using System;
using System.Text;
using Kudos.Constants;

namespace Kudos.Serving.KaronteModule.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class KaronteControllerAttribute : Attribute
    {
        public readonly String? Version;
        public readonly Boolean HasVersion;

        public KaronteControllerAttribute() : this(null, null, null, null) { }
        public KaronteControllerAttribute(Int32 iMajor) : this(iMajor, null, null, null) { }
        public KaronteControllerAttribute(Int32 iMajor, Int32 iMinor) : this(iMajor, iMinor, null, null) { }
        public KaronteControllerAttribute(Int32 iMajor, Int32 iMinor, Int32 iPatch) : this(iMajor, iMinor, iPatch, null) { }

        private KaronteControllerAttribute(Int32? iMajor, Int32? iMinor, Int32? iPatch, Object? o)
        {
            if (iMajor == null)
                Version = null;
            else if (iMinor == null)
                Version = "" + iMajor;
            else if (iPatch == null)
                Version = "" + iMajor + CCharacter.Dot + iMinor;
            else
                Version = "" + iMajor + CCharacter.Dot + iMinor + CCharacter.Dot + iPatch;

            HasVersion = Version != null;
        }
    }
}