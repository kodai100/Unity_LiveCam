using UnityEngine;
using System.Collections;

namespace kodai100.LiveCamCore.Splines
{
    public static class ArrayUtil
    {
        public static void Insert<T>(ref T[] list, T newone, int at)
        {
            var expanded = new T[list.Length + 1];
            System.Array.Copy(list, expanded, at);
            System.Array.Copy(list, at, expanded, at + 1, list.Length - at);
            expanded[at] = newone;
            list = expanded;
        }

        public static T Remove<T>(ref T[] list, int at)
        {
            var oldone = list[at];
            System.Array.Copy(list, at + 1, list, at, list.Length - (at + 1));
            System.Array.Resize(ref list, list.Length - 1);
            return oldone;
        }
    }
}