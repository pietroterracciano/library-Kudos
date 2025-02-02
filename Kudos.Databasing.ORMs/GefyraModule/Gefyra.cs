﻿using System;
using System.Collections;
using System.Data;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kudos.Constants;
using Kudos.Databasing.Descriptors;
using Kudos.Databasing.Enums;
using Kudos.Databasing.Interfaces;
using Kudos.Databasing.ORMs.GefyraModule.Builders;
using Kudos.Databasing.ORMs.GefyraModule.Builts;
using Kudos.Databasing.ORMs.GefyraModule.Constants;
using Kudos.Databasing.ORMs.GefyraModule.Contexts;
using Kudos.Databasing.ORMs.GefyraModule.Descriptors;
using Kudos.Databasing.ORMs.GefyraModule.Entities;
using Kudos.Databasing.ORMs.GefyraModule.Enums;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Builders;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Contexts;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Entities;
using Kudos.Reflection.Utils;
using Kudos.Types;
using Kudos.Utils;
using Kudos.Utils.Collections;
using Kudos.Utils.Numerics;
using Kudos.Utils.Texts;
using Mysqlx.Prepare;

namespace Kudos.Databasing.ORMs.GefyraModule
{
    public static class Gefyra
    {
        #region Builder

        #region public static IGefyraBuilder RequestBuilder()

        public static IGefyraBuilder RequestBuilder()
        {
            return new GefyraBuilder();
        }

        #endregion

        #endregion

        #region Context

        #region public static IGefyraContext RequestContext()

        public static IGefyraContext RequestContext(IDatabaseHandler? dh)
        {
            return new GefyraContext<Object>(ref dh);
        }

        #endregion

        #endregion

        #region Table

        #region public static IGefyraTable? GetTable<...>(...)

        public static IGefyraTable? GetTable<T>() { GefyraTable? gt; GefyraTable.Get<T>(out gt); return gt; }
        public static IGefyraTable? GetTable(Type? t) { GefyraTable? gt; GefyraTable.Get(ref t, out gt); return gt; }
        //public static IGefyraTable RequestTable(String? sName) { GefyraTable gt; GefyraTable.Request(ref sName, out gt); return gt; }
        //public static IGefyraTable RequestTable(String? sSchemaName, String? sName) { GefyraTable gt; GefyraTable.Request(ref sSchemaName, ref sName, out gt); return gt; }\

        #endregion

        #endregion

        #region Column

        #region public static IGefyraColumn? GetColumn<...>(...)

        public static IGefyraColumn? GetColumn<T>(String? sn) { GefyraColumn? gc; GefyraColumn.Get<T>(ref sn, out gc); return gc; }
        public static IGefyraColumn? GetColumn(Type? t, String? sn) { GefyraColumn? gc; GefyraColumn.Get(ref t, ref sn, out gc); return gc; }
        //public static IGefyraTable RequestTable(String? sName) { GefyraTable gt; GefyraTable.Request(ref sName, out gt); return gt; }
        //public static IGefyraTable RequestTable(String? sSchemaName, String? sName) { GefyraTable gt; GefyraTable.Request(ref sSchemaName, ref sName, out gt); return gt; }\

        #endregion

        #endregion

        #region Join

        //#region public static GefyraTable RequestTable<...>(...)

        //public static IGefyraTable RequestJoin<T>(String? sn) { GefyraJoin gt; GefyraTable.Request<T>(out gt); return gt; }
        //public static IGefyraTable RequestJoin(Type? t, String? sn) { GefyraTable gt; GefyraTable.Request(ref t, out gt); return gt; }

        //#endregion

        #endregion

        #region Parse

        //public static Task<T[]?> ParseAsync<T>(DataTable? dt, GefyraBuilt? gb = null) { return Task.Run(() => Parse<T>(dt, gb)); }
        public static T[]? Parse<T>(DataTable? dt, GefyraBuilt? gb = null)
        {
            return Parse(typeof(T), dt, gb) as T[];
        }
        public static Object[]? Parse(Type? t, DataTable? dt, GefyraBuilt? gb = null)
        {
            if (dt == null || dt.Rows.Count < 1)
                return null;

            Object[]?
               oa0 = ArrayUtils.CreateInstance(t, dt.Rows.Count);

            if (oa0 == null)
                return null;

            Int32
                iNonNullableObjects = 0;

            Object? oi;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oi = Parse(t, dt.Rows[i], gb);
                if (oi == null) continue;
                oa0[i] = oi; iNonNullableObjects++;
            }

            if (iNonNullableObjects < 1)
                return null;
            else if (dt.Rows.Count - iNonNullableObjects == 0)
                return oa0;

            Object[]?
                oa1 = ArrayUtils.CreateInstance(t, iNonNullableObjects);

            if (oa1 == null)
                return null;

            for (int i = 0; i < oa0.Length; i++)
            {
                if (oa0[i] == null) continue;
                oa1[i] = oa0[i];
            }

            return oa1;
        }

        //public static Task<T?> ParseAsync<T>(DataRow? dr, GefyraBuilt? gb = null) { return Task.Run(() => Parse<T>(dr, gb)); }
        public static T? Parse<T>(DataRow? dr, GefyraBuilt? gb = null)
        {
            return ObjectUtils.Cast<T>(Parse(typeof(T), dr, gb));
        }
        public static Object? Parse(Type? t, DataRow? dr, GefyraBuilt? gb = null)
        {
            if (dr == null)
                return null;

            GefyraTable? gt;
            GefyraTable.Get(ref t, out gt);

            if (gt == null)
                return null;

            Object?
                o =
                    ReflectionUtils.InvokeConstructor(ReflectionUtils.GetConstructor(t, CBindingFlags.Instance));

            if (o == null)
                return null;

            Metas? m;

            if (gb != null && gb.OutputColumns.Length > 0)
            {
                m = new Metas(gb.OutputColumns.Length, StringComparison.OrdinalIgnoreCase);

                for (int i = 0; i < gb.OutputColumns.Length; i++)
                {
                    if (!gb.OutputColumns[i].HasAlias) continue;
                    m.Set(gb.OutputColumns[i].Alias, gb.OutputColumns[i]);
                }
            }
            else
                m = null;

            IGefyraColumn? gci;

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                gci =
                    m != null
                        ? m.Get<IGefyraColumn>(dr.Table.Columns[i].ColumnName)
                        : null;

                if (gci == null)
                    gci = gt.GetColumn(dr.Table.Columns[i].ColumnName);

                if (gci == null || !gci.HasDeclaringMember)
                    continue;

                ReflectionUtils.SetMemberValue(o, gci.DeclaringMember, DataRowUtils.GetValue(dr, i), true);
            }

            return o;
        }

        #endregion

        #region Fit

        public static void FitInPlace<T>(IDatabaseHandler? dbh, T? o)
        {
            Task t = FitAsyncInPlace(dbh, o);
            t.Wait();
        }
        public static T? Fit<T>(IDatabaseHandler? dbh, T? o)
        {
            Task<T?> t = FitAsync(dbh, o);
            t.Wait();
            return t.Result;
        }

        public static async Task FitAsyncInPlace<T>(IDatabaseHandler? dh, T? o)
        {
            await __FitAsync(dh, o, true);
        }
        public static async Task<T?> FitAsync<T>(IDatabaseHandler? dh, T? o)
        {
            return await __FitAsync(dh, o, false);
        }
        private static async Task<T?> __FitAsync<T>
        (
            IDatabaseHandler? dh,
            T? o,
            Boolean bInPlace
        )
        { 
            if (dh == null || o == null)
                return bInPlace ? o : default(T);

            T? o0;

            if (bInPlace)
                o0 = o;
            else
            {
                o0 = ReflectionUtils.Copy(o, CBindingFlags.Instance);
                if (o0 == null) return o0;
            }

            GefyraSocket?
                gs = await GefyraSocket.GetAsync<T>(dh);

            if (gs == null)
                return bInPlace ? o : o0;

            GefyraColumnDescriptor[]? gcda;
            gs.GetColumnsDescriptors(out gcda);

            if (gcda == null)
                return bInPlace ? o : o0;

            DatabaseColumnDescriptor? dbcdi;
            Object? oi;
            for (int i=0; i<gcda.Length; i++)
            {
                if (!gcda[i].HasDeclaringMember) continue;
                oi = ReflectionUtils.GetMemberValue(o, gcda[i].DeclaringMember);
                gs.GetDatabaseColumnDescriptor(ref gcda[i], out dbcdi);
                FitInPlace(ref dbcdi, ref oi);
                ReflectionUtils.SetMemberValue(o0, gcda[i].DeclaringMember, oi);
            }

            return o0;
        }

        //internal static Object? Fit(DatabaseColumnDescriptor? dbcd, Object? o) { Object? oOut; __Fit(ref dbcd, ref o, out oOut); return oOut; }
        internal static void FitInPlace(ref DatabaseColumnDescriptor? dbcd, ref Object? o)
        {
            if (dbcd == null)
                return;
            else if (!dbcd.IsRequired)
            {
                Boolean bNull = o == null;

                if
                (
                    bNull
                    || o.Equals(dbcd.DefaultValue)
                )
                {
                    if (dbcd.IsNullable)
                        return;
                    else if (dbcd.DefaultValue != null)
                    {
                        if (bNull)
                            o = dbcd.DefaultValue;
                        return;
                    }
                }
            }

            //else if
            //(
            //    !dbcd.IsRequired
            //    &&
            //    (
            //        o == null
            //        || o.Equals(dbcd.DefaultValue)
            //    )
            //)
            //{
            //    if( dbcd.IsNullable )
            //        return;
            //    else if(dbcd.DefaultValue != null)
            //    {
            //        o = dbcd.DefaultValue;
            //        return;
            //    }
            //}

            Type?
                tDeclaring;

            if (dbcd.DataTypeDescriptor.Collation == EDatabaseDataCollation.Numerical)
                tDeclaring =
                    dbcd.IsNullable
                        ? NumericUtils.ParseToNType(dbcd.DataTypeDescriptor.DeclaringType)
                        : NumericUtils.ParseToNNType(dbcd.DataTypeDescriptor.DeclaringType);
            else
                tDeclaring = dbcd.DataTypeDescriptor.DeclaringType;

            o = ObjectUtils.ChangeType(tDeclaring, o);

            #region UInt16
            if (tDeclaring == CType.UInt16 || tDeclaring == CType.NullableUInt16)
            {
                UInt16? i = o as UInt16?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region UInt32
            else if (tDeclaring == CType.UInt32 || tDeclaring == CType.NullableUInt32)
            {
                UInt32? i = o as UInt32?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region UInt64
            else if (tDeclaring == CType.UInt64 || tDeclaring == CType.NullableUInt64)
            {
                UInt64? i = o as UInt64?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region Int16
            else if (tDeclaring == CType.Int16 || tDeclaring == CType.NullableInt16)
            {
                Int16? i = o as Int16?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region Int32
            else if (tDeclaring == CType.Int32 || tDeclaring == CType.NullableInt32)
            {
                Int32? i = o as Int32?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region Int64
            else if (tDeclaring == CType.Int64 || tDeclaring == CType.NullableInt64)
            {
                Int64? i = o as Int64?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region Single
            else if (tDeclaring == CType.Single || tDeclaring == CType.NullableSingle)
            {
                Single? i = o as Single?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region Double
            else if (tDeclaring == CType.Double || tDeclaring == CType.NullableDouble)
            {
                Double? i = o as Double?;
                if (i != null)
                {
                    if (i > dbcd.DataTypeDescriptor.MaxValue) o = dbcd.DataTypeDescriptor.MaxValue;
                    else if (i < dbcd.DataTypeDescriptor.MinValue) o = dbcd.DataTypeDescriptor.MinValue;
                }
            }
            #endregion
            #region String
            else if (tDeclaring == CType.String)
            {
                Int32? i = Int32Utils.Parse(dbcd.CurrentMaxLength);
                if (i != null) o = StringUtils.Truncate(o as String, i.Value);
            }
            #endregion
            #region Char
            else if (tDeclaring == CType.Char || tDeclaring == CType.NullableChar)
                o = o as Char?;
            #endregion

            if (o != null)
                return;
            else if(!dbcd.IsRequired)
            {
                o = dbcd.DefaultValue;
                return;
            }

            #region UInt16
            if (tDeclaring == CType.UInt16 || tDeclaring == CType.NullableUInt16)
                o = (UInt16)0;
            #endregion
            #region UInt32
            else if (tDeclaring == CType.UInt32 || tDeclaring == CType.NullableUInt32)
                o = 0U;
            #endregion
            #region UInt64
            else if (tDeclaring == CType.UInt64 || tDeclaring == CType.NullableUInt64)
                o = 0UL;
            #endregion
            #region Int16
            else if (tDeclaring == CType.Int16 || tDeclaring == CType.NullableInt16)
                o = (Int16)0;
            #endregion
            #region Int32
            else if (tDeclaring == CType.Int32 || tDeclaring == CType.NullableInt32)
                o = 0;
            #endregion
            #region Int64
            else if (tDeclaring == CType.Int64 || tDeclaring == CType.NullableInt64)
                o = 0L;
            #endregion
            #region Single
            else if (tDeclaring == CType.Single || tDeclaring == CType.NullableSingle)
                o = 0F;
            #endregion
            #region Double
            else if (tDeclaring == CType.Double || tDeclaring == CType.NullableDouble)
                o = 0D;
            #endregion
            #region String
            else if (tDeclaring == CType.String)
                o = String.Empty;
            #endregion
            #region Char
            else if (tDeclaring == CType.Char || tDeclaring == CType.NullableChar)
                o = CCharacter.Null;
            #endregion

            if (o != null)
                return;

            o = ReflectionUtils.CreateInstance(tDeclaring);
        }

        #endregion

        #region PrepareAgainst

        //public static Task<String?> PrepareAgainstAsync(String? s, EGefyraAgainst? ega) { return Task.Run(() => PrepareAgainst(s, ega)); }
        public static String? PrepareAgainst(String? s, EGefyraAgainst? ega)
        {
            if (s == null || ega != EGefyraAgainst.InBooleanMode)
                return s;

            s =
                s
                    .Replace(CCharacter.Plus, CCharacter.Space)
                    .Replace(CCharacter.Minus, CCharacter.Space)
                    .Replace(CCharacter.Asterisk, CCharacter.Space)
                    .Replace(CCharacter.Tilde, CCharacter.Space)
                    .Replace(CCharacter.LeftRoundBracket, CCharacter.Space)
                    .Replace(CCharacter.RightRoundBracket, CCharacter.Space)
                    .Replace(CCharacter.SingleQuote, CCharacter.Space)
                    .Replace(CCharacter.DoubleQuote, CCharacter.Space)
                    .Replace(CCharacter.At, CCharacter.Space)
                    ;

            String[]
                sa = s.Split(CCharacter.Space, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            StringBuilder
                sb = new StringBuilder(s.Length);

            for (int i = 0; i < sa.Length; i++)
            {
                if (sb.Length > 0) sb.Append(CCharacter.Space);
                sb.Append(CCharacter.Plus).Append(sa[i]).Append(CCharacter.Asterisk);
            }

            return sb.ToString();
        }


        #endregion

        //#region Column

        //internal static void __GetColumn(ref GefyraTable? gt, ref String? sn, ref String? sa, out GefyraColumn gc)
        //{
        //    if (gt == null)
        //    {
        //        gc = GefyraColumn.Invalid;
        //        return;
        //    }

        //    String? s;
        //    __CalculateColumnHashKey(ref sn, ref sa, out s);

        //    if(s == null)
        //    {
        //        gc = GefyraColumn.Invalid;
        //        return; 
        //    }

        //    lock (__lck)
        //    {
        //        gc = __m.Get<GefyraColumn>(s, StringComparison.OrdinalIgnoreCase);
        //        if (gc != null) return gc;

        //        MemberInfo? mi = ReflectionUtils.GetMember(DeclaringType, s);
        //        if (mi != null)
        //        {
        //            StringBuilder sb;
        //            _GetStringBuilder(out sb);

        //            GefyraColumnAttribute?
        //                gca = ReflectionUtils.GetCustomAttribute<GefyraColumnAttribute>(mi);

        //            if (
        //                gca != null
        //                && gca.Name != null
        //                && gca.Name.Length > 0
        //            )
        //                sb.Append(gca.Name);
        //            else
        //            {
        //                String scp; Type? tmvt = ReflectionUtils.GetMemberValueType(mi);
        //                GefyraTypeUtils.GetConventionalPrefix(ref tmvt, out scp);
        //                sb.Append(scp).Append(mi.Name);
        //            }

        //            String sgcn = sb.ToString();
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref sgcn, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);
        //        }
        //        else
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref s, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);

        //        return gc;
        //    }
        //}

        //private static void __CalculateColumnHashKey(ref String? sn, ref String? sa, out String? s)
        //{
        //    if (String.IsNullOrWhiteSpace(sn)) { s = null; return; }

        //    lock (__lck)
        //    {
        //        __sb.Clear();

        //        __sb
        //            .Append("gc").Append(CCharacter.Dot).Append("n").Append(CCharacter.DoubleDot).Append(sn);

        //        if (!String.IsNullOrWhiteSpace(sa))
        //        {
        //            if (__sb.Length > 0)
        //                __sb.Append(CCharacter.Pipe);

        //            __sb
        //                .Append("gc").Append(CCharacter.Dot).Append("a").Append(CCharacter.DoubleDot).Append(sa);
        //        }

        //        s = __sb.ToString();
        //    }
        //}



        //#endregion

        //internal static void __GetColumn(ref GefyraTable? gt, ref String? s, out GefyraColumn gc)
        //{
        //    if (gt == null || String.IsNullOrWhiteSpace(s))
        //    {
        //        gc = GefyraColumn.Invalid;
        //        return;
        //    }

        //    lock (__lck)
        //    {
        //        String schk;
        //        CalculateColumnHashKey(ref s, out schk);

        //        GefyraColumn? gc = m.Get<GefyraColumn>(schk, StringComparison.OrdinalIgnoreCase);
        //        if (gc != null) return gc;

        //        MemberInfo? mi = ReflectionUtils.GetMember(DeclaringType, s);
        //        if (mi != null)
        //        {
        //            StringBuilder sb;
        //            _GetStringBuilder(out sb);

        //            GefyraColumnAttribute?
        //                gca = ReflectionUtils.GetCustomAttribute<GefyraColumnAttribute>(mi);

        //            if (
        //                gca != null
        //                && gca.Name != null
        //                && gca.Name.Length > 0
        //            )
        //                sb.Append(gca.Name);
        //            else
        //            {
        //                String scp; Type? tmvt = ReflectionUtils.GetMemberValueType(mi);
        //                GefyraTypeUtils.GetConventionalPrefix(ref tmvt, out scp);
        //                sb.Append(scp).Append(mi.Name);
        //            }

        //            String sgcn = sb.ToString();
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref sgcn, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);
        //        }
        //        else
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref s, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);

        //        return gc;
        //    }
        //}

        //private static void __CalculateColumnHashKey(ref GefyraTable gt, ref String s, out String s)
        //{
        //    __sb.Clear();

        //    __sb
        //        .Append("gt").Append(CCharacter.Dot).Append("sn").Append(CCharacter.DoubleDot).Append(sSchemaName);

        //    if (__sb.Length > 0)
        //        __sb.Append(CCharacter.Pipe);

        //    __sb
        //        .Append("gt").Append(CCharacter.Dot).Append("n").Append(CCharacter.DoubleDot).Append(sName);

        //    sOut = __sb.ToString();
        //}


















        //#region public static GefyraTable GetTable<...>(...)

        //public static GefyraTable GetColumn(GefyraTable? gt, String? s)
        //{
        //    if (t == null)
        //        return GefyraTable.Invalid;

        //    lock (__lck)
        //    {
        //        Dictionary<Int32, GefyraTable> d;
        //        if (!__d.TryGetValue(t.Module.MetadataToken, out d) || d == null)
        //            __d[t.Module.MetadataToken] = d = new Dictionary<int, GefyraTable>();

        //        GefyraTable? gt;
        //        if (d.TryGetValue(t.MetadataToken, out gt) && gt != null)
        //            return gt;

        //        GefyraTableAttribute? gta;

        //        #region Recupero l'Attribute per la Class

        //        gta = ReflectionUtils.GetCustomAttribute<GefyraTableAttribute>(t);

        //        #endregion

        //        String?
        //            sSchemaName;
        //        String?
        //            sName;

        //        #region Recupero i dati dall'Attribute (se possibile)

        //        if (gta != null && gta.Name != null && gta.Name.Length > 0)
        //        {
        //            sSchemaName = gta.SchemaName;
        //            sName = gta.Name;
        //        }
        //        else
        //            sSchemaName = sName = null;

        //        if (String.IsNullOrWhiteSpace(sName))
        //            sName = CGefyraConventionalPrefix.Class + t.Name;

        //        #endregion

        //        String? sAlias = null;
        //        return d[t.MetadataToken] = gt = new GefyraTable(ref t, ref sSchemaName, ref sName, ref sAlias);
        //    }
        //}

        //#endregion

        //internal static void __GetColumn(ref GefyraTable? gt, ref String? s, out GefyraColumn gc)
        //{
        //    if (gt == null || String.IsNullOrWhiteSpace(s))
        //    {
        //        gc = GefyraColumn.Invalid;
        //        return;
        //    }

        //    lock (__lck)
        //    {
        //        String schk;
        //        CalculateColumnHashKey(ref s, out schk);

        //        GefyraColumn? gc = m.Get<GefyraColumn>(schk, StringComparison.OrdinalIgnoreCase);
        //        if (gc != null) return gc;

        //        MemberInfo? mi = ReflectionUtils.GetMember(DeclaringType, s);
        //        if (mi != null)
        //        {
        //            StringBuilder sb;
        //            _GetStringBuilder(out sb);

        //            GefyraColumnAttribute?
        //                gca = ReflectionUtils.GetCustomAttribute<GefyraColumnAttribute>(mi);

        //            if (
        //                gca != null
        //                && gca.Name != null
        //                && gca.Name.Length > 0
        //            )
        //                sb.Append(gca.Name);
        //            else
        //            {
        //                String scp; Type? tmvt = ReflectionUtils.GetMemberValueType(mi);
        //                GefyraTypeUtils.GetConventionalPrefix(ref tmvt, out scp);
        //                sb.Append(scp).Append(mi.Name);
        //            }

        //            String sgcn = sb.ToString();
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref sgcn, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);
        //        }
        //        else
        //            m.Set(schk, gc = new GefyraColumn(ref _this, ref s, ref mi, ref __s), StringComparison.OrdinalIgnoreCase);

        //        return gc;
        //    }
        //}

        //private static void __CalculateColumnHashKey(ref GefyraTable gt, ref String s, out String s)
        //{
        //    __sb.Clear();

        //    __sb
        //        .Append("gt").Append(CCharacter.Dot).Append("sn").Append(CCharacter.DoubleDot).Append(sSchemaName);

        //    if (__sb.Length > 0)
        //        __sb.Append(CCharacter.Pipe);

        //    __sb
        //        .Append("gt").Append(CCharacter.Dot).Append("n").Append(CCharacter.DoubleDot).Append(sName);

        //    sOut = __sb.ToString();
        //}
    }
}
