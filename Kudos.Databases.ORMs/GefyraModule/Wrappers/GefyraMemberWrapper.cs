﻿using Kudos.Databases.Enums.Columns;
using Kudos.Utils;
using Kudos.Utils.Members;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.Databases.ORMs.GefyraModule.Wrappers
{
    internal class GefyraMemberWrapper : AGefyraWrapper<MemberInfo>
    {
        private readonly Type? _tMember;

        internal GefyraMemberWrapper(ref MemberInfo? oMember) : base(ref oMember)
        {
            _tMember = MemberUtils.GetValueType(oMember);
        }

        internal override Object? PrepareValue(ref Object? oObject)
        {
            //if (_tMember == null || oObject == null)
            //    return null;

            //if (IsSerializable(ref oObject))
            //    oObject = JSONUtils.Deserialize(_tMember, oObject);

            //switch (Type)
            //{
            //    case EDBColumnType.Json:
            //        oValue = JSONUtils.Serialize(oValue);
            //        break;
            //    default:
            //        if (IsSerializable(ref oValue))
            //            oValue = JSONUtils.Serialize(oValue);

            //        oValue = ObjectUtils.ChangeType(oValue, ValueType);

            //        switch (Type)
            //        {
            //            case EDBColumnType.VariableChar:
            //            case EDBColumnType.Text:
            //            case EDBColumnType.MediumText:
            //            case EDBColumnType.LongText:
            //                if (MaxLength != null)
            //                    oValue = StringUtils.Truncate(oValue as String, MaxLength.Value);
            //                break;
            //        }
            //        break;
            //}

            //if (oValue != null)
            //    return;
            //else if (DefaultValue != null)
            //    oValue = DefaultValue;
            //else if (!IsNullable)
            //    switch (Type)
            //    {
            //        case EDBColumnType.Json:
            //            oValue = __sDefaultNonNullableJSONValue;
            //            break;
            //        case EDBColumnType.UnsignedTinyInteger:
            //        case EDBColumnType.TinyInteger:
            //        case EDBColumnType.UnsignedSmallInteger:
            //        case EDBColumnType.SmallInteger:
            //        case EDBColumnType.UnsignedMediumInteger:
            //        case EDBColumnType.MediumInteger:
            //        case EDBColumnType.UnsignedInteger:
            //        case EDBColumnType.Integer:
            //        case EDBColumnType.UnsignedBigInteger:
            //        case EDBColumnType.BigInteger:
            //            oValue = 0;
            //            break;
            //        case EDBColumnType.UnsignedDouble:
            //        case EDBColumnType.Double:
            //            oValue = 0.0d;
            //            break;
            //        case EDBColumnType.Boolean:
            //            oValue = false;
            //            break;
            //        case EDBColumnType.VariableChar:
            //        case EDBColumnType.Text:
            //        case EDBColumnType.MediumText:
            //        case EDBColumnType.LongText:
            //            oValue = String.Empty;
            //            break;
            //    }

            return null;
        }
    }
}
