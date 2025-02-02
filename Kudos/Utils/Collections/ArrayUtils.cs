﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Kudos.Constants;
using Kudos.Reflection.Utils;

namespace Kudos.Utils.Collections
{
    public abstract class ArrayUtils : CollectionUtils
    {
        #region Cast(...)

        public static new Array Cast(Object? o) { return o as Array; }

        #endregion

        #region CreateInstance<...>(...)

        public static T[]? CreateInstance<T>(int i) { return CreateInstance(typeof(T), i) as T[]; }
        public static Object[]? CreateInstance(Type? t, int i) { if (t != null && i > -1) try { return Array.CreateInstance(t, i) as Object[]; } catch { } return null; }

        #endregion

        #region RemoveNullEntries()

        public static T[]? RemoveNullEntries<T>(T?[]? oa)
        {
            return ObjectUtils.Cast<T[]>(RemoveNullEntries(oa as Object?[]));
        }
        public static Object[]? RemoveNullEntries(Object?[]? oa)
        {
            if (oa == null || oa.Length < 1) return null;

            List<Object> lo = new List<object>(oa.Length);

            for (int i = 0; i < oa.Length; i++)
            {
                if (oa[i] == null) continue;
                lo.Add(oa[i]);
            }

            return lo.ToArray();
        }

        #endregion

        #region GetValue(...)

        public static T? GetFirstValue<T>(Array? a) { return ObjectUtils.Cast<T>(GetFirstValue(a)); }
        public static Object? GetFirstValue(Array? a) { return GetValue(a, 0); }
        public static T? GetLastValue<T>(Array? a) { return ObjectUtils.Cast<T>(GetLastValue(a)); }
        public static Object? GetLastValue(Array? a) { return a != null ? GetValue(a, a.Length -1) : null; }
        public static T? GetValue<T>(Array? a, int i) { return ObjectUtils.Cast<T>(GetValue(a, i)); }
        public static Object? GetValue(Array? a, int i) { return IsValidIndex(a, i) ? a.GetValue(i) : null; }

        #endregion

        #region Shift(...)

        public static T? Shift<T>(T[]? a0, out T[]? a1)
        {
            if (a0 == null || a0.Length < 1)
            {
                a1 = null;
                return default(T?);
            }

            a1 = new T[a0.Length - 1];

            if (a1.Length > 0)
                Array.Copy(a0, 1, a1, 0, a1.Length);

            return a0[0];
        }

        #endregion

        #region public static T?[] UnShift(...)

        public static T?[] UnShift<T>(T? o, T?[]? a)
        {
            int i = a != null ? a.Length : 0;

            T?[] a1 = new T[1 + i];
            a1[0] = o;

            if (i > 0)
                Array.Copy(a, 0, a1, 1, a.Length);

            return a1;
        }

        #endregion

        #region public static T[]? Append<T>(...)

        public static T?[]? Append<T>(T?[]? t0, T? t1) { return Append<T>(t0, new T?[] { t1 }); }
        public static T?[]? Append<T>(T? t0, T?[]? t1) { return Append<T>(new T?[] { t0 }, t1); }
        public static T?[]? Append<T>(T? t0, T? t1) { return Append<T>(new T?[] { t0 }, new T?[] { t1 }); }
        public static T?[]? Append<T>(T?[]? ta0, T?[]? ta1)
        {
            if (ta0 == null && ta1 == null)
                return null;
            else if (ta0 != null && ta1 == null)
                return ta0;
            else if (ta0 == null && ta1 != null)
                return ta1;

            T[]
                ta = new T[ta0.Length + ta1.Length];

            Array.Copy(
                ta0,
                0,
                ta,
                0,
                ta0.Length
            );

            Array.Copy(
                ta1,
                0,
                ta,
                ta0.Length,
                ta1.Length
            );

            return ta;
        }

        #endregion

        #region public static T[]? Prepend<T>(...)

        public static Byte[]? Prepend(Byte[]? ta0, Byte[]? ta1)
        {
            return Append(ta1, ta0);
        }

        #endregion

        #region public static void Split<T>(...)

        public static void Split<T>(T[]? tai, int i, out T[]? tao0, out T[]? tao1)
        {
            if (!IsValidIndex(tai, i))
            {
                tao0 = null;
                tao1 = tai;
                return;
            }

            tao0 = new T[i];
            tao1 = new T[tai.Length - i];

            Array.Copy(tai, 0, tao0, 0, tao0.Length);
            Array.Copy(tai, i, tao1, 0, tao1.Length);
        }

        #endregion

        #region public static Type? GetArgumentType(...)

        public new static Type? GetArgumentType(ICollection? o)
        {
            return o != null
                ? GetArgumentType(o.GetType())
                : null;
        }

        public new static Type? GetArgumentType(Type? t)
        {
            return ReflectionUtils.GetMemberValueType(ReflectionUtils.GetMethod(t, "Get"));
        }

        #endregion
    }
}
