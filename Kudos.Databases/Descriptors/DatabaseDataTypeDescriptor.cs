﻿using System;
using Kudos.Databases.Constants;
using Kudos.Databases.Constants.Columns;
using Kudos.Databases.Enums;
using Kudos.Utils.Collections;
using System.Collections.Generic;
using Kudos.Constants;
using Kudos.Utils.Numerics;

namespace Kudos.Databases.Descriptors
{
	public class DatabaseDataTypeDescriptor
	{
        #region ... static ...

        private static readonly Dictionary<EDatabaseDataType, DatabaseDataTypeDescriptor>
            __d;

        private static readonly Int32
            __iInt8__MinValue,
            __iInt8__MaxValue,
            __iInt24_MinValue,
            __iInt24_MaxValue;

        private static readonly UInt32
            __iUInt8__MaxValue,
            __iUInt24_MaxValue,
            __iUInt30_MaxValue;

        public static readonly DatabaseDataTypeDescriptor
            UnsignedTinyInteger,
            TinyInteger,
            UnsignedSmallInteger,
            SmallInteger,
            UnsignedMediumInteger,
            MediumInteger,
            UnsignedInteger,
            Integer,
            UnsignedBigInteger,
            BigInteger,
            UnsignedDouble,
            Double,
            VariableChar,
            Text,
            MediumText,
            LongText,
            Json;

        static DatabaseDataTypeDescriptor()
        {
            __d = new Dictionary<EDatabaseDataType, DatabaseDataTypeDescriptor>(24);

            __iInt8__MinValue = -128;
            __iInt8__MaxValue = 127;
            __iUInt8__MaxValue = 255;

            __iInt24_MinValue = -8388607;
            __iInt24_MaxValue = 8388607;
            __iUInt24_MaxValue = UInt32Utils.NNParse(Math.Pow(2, 24)) - 1;
            __iUInt30_MaxValue = UInt32Utils.NNParse(Math.Pow(2, 30)) - 1;

            #region UnsignedTinyInteger

            __d[EDatabaseDataType.UnsignedTinyInteger] =
                UnsignedTinyInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedTinyInteger,
                        CType.UInt16,
                        UInt16.MinValue,
                        __iUInt8__MaxValue,
                        null
                    );

            #endregion

            #region TinyInteger

            __d[EDatabaseDataType.TinyInteger] =
                TinyInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.TinyInteger,
                        CType.Int16,
                        __iInt8__MinValue,
                        __iInt8__MaxValue,
                        null
                    );

            #endregion

            #region UnsignedSmallInteger

            __d[EDatabaseDataType.UnsignedSmallInteger] =
                UnsignedSmallInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedSmallInteger,
                        CType.UInt16,
                        UInt16.MinValue,
                        UInt16.MaxValue,
                        null
                    );

            #endregion

            #region SmallInteger

            __d[EDatabaseDataType.SmallInteger] =
               SmallInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.SmallInteger,
                        CType.Int16,
                        Int16.MinValue,
                        Int16.MaxValue,
                        null
                    );

            #endregion

            #region UnsignedMediumInteger

            __d[EDatabaseDataType.UnsignedMediumInteger] =
                UnsignedMediumInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedMediumInteger,
                        CType.UInt32,
                        UInt32.MinValue,
                        __iUInt24_MaxValue,
                        null
                    );

            #endregion

            #region MediumInteger

            __d[EDatabaseDataType.MediumInteger] =
                MediumInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.MediumInteger,
                        CType.Int32,
                        __iInt24_MinValue,
                        __iInt24_MaxValue,
                        null
                    );

            #endregion

            #region UnsignedInteger

            __d[EDatabaseDataType.UnsignedInteger] =
                UnsignedInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedInteger,
                        CType.UInt32,
                        UInt32.MinValue,
                        UInt32.MaxValue,
                        null
                    );

            #endregion

            #region Integer

            __d[EDatabaseDataType.Integer] =
                Integer =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Integer,
                        CType.Int32,
                        Int32.MinValue,
                        Int32.MaxValue,
                        null
                    );

            #endregion

            #region UnsignedBigInteger

            __d[EDatabaseDataType.UnsignedBigInteger] =
                UnsignedBigInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedBigInteger,
                        CType.UInt64,
                        UInt64.MinValue,
                        UInt64.MaxValue,
                        null
                    );

            #endregion

            #region BigInteger

            __d[EDatabaseDataType.BigInteger] =
                BigInteger =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.BigInteger,
                        CType.Int64,
                        Int64.MinValue,
                        Int64.MaxValue,
                        null
                    );

            #endregion

            #region UnsignedDouble

            __d[EDatabaseDataType.UnsignedDouble] =
                UnsignedDouble =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.UnsignedDouble,
                        CType.Double,
                        UInt64.MinValue,
                        System.Double.MaxValue,
                        null
                    );

            #endregion

            #region Double

            __d[EDatabaseDataType.Double] =
                Double =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.Double,
                        UInt64.MinValue,
                        System.Double.MaxValue,
                        null
                    );

            #endregion

            #region VariableChar

            __d[EDatabaseDataType.VariableChar] =
                VariableChar =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.String,
                        null,
                        null,
                        UInt16.MaxValue
                    );

            #endregion

            #region Text

            __d[EDatabaseDataType.Text] =
                Text =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.String,
                        null,
                        null,
                        UInt16.MaxValue
                    );

            #endregion

            #region MediumText

            __d[EDatabaseDataType.MediumText] =
                MediumText =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.String,
                        null,
                        null,
                        __iUInt24_MaxValue
                    );

            #endregion

            #region LongText

            __d[EDatabaseDataType.LongText] =
                LongText =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.String,
                        null,
                        null,
                        UInt32.MaxValue
                    );

            #endregion

            #region Json

            __d[EDatabaseDataType.Json] =
                Json =
                    new DatabaseDataTypeDescriptor
                    (
                        EDatabaseDataType.Double,
                        CType.String,
                        null,
                        null,
                        __iUInt30_MaxValue
                    );

            #endregion
        }

        internal static void Get(ref EDatabaseDataType? edbdt, DatabaseDataTypeDescriptor? dbdtd)
        {
            if (edbdt == null) { dbdtd = null; return; }
            __d.TryGetValue(edbdt.Value, out dbdtd);
        }

        #endregion

        //public readonly EDatabaseDataType DataType;
        public readonly Type SimplexType;
        public readonly Double? MinValue, MaxValue;
        public readonly UInt32? MaxLength;
        public readonly EDatabaseDataType ComplexType;

        private DatabaseDataTypeDescriptor(EDatabaseDataType ect, Type t, Double? dmnv, Double? dmxv, UInt32? uimxl)
        {
            ComplexType = ect;
            SimplexType = t;
            MinValue = dmnv;
            MaxValue = dmxv;
            MaxLength = uimxl;
        }
    }
}

