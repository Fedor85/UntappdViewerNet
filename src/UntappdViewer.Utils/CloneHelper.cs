using System;
using System.Reflection;

namespace UntappdViewer.Utils
{
    public static class CloneHelper
    {
        public static T Clone<T>(T source)
        {
            Type typeSource = source.GetType();
            FieldInfo[] fields = typeSource.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object copyObject = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeSource);
            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(copyObject, fields[i].GetValue(source));

            return (T)copyObject;
        }
    }
}