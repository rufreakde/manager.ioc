using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

public static class SerializedPropertyExtensions
{

    public static object GetPropertyValue(this SerializedProperty property)
    {
        System.Type parentType = property.serializedObject.targetObject.GetType();
        System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
        return fi.GetValue(property.serializedObject.targetObject);
    }
    public static void SetPropertyValue(this SerializedProperty property, object value)
    {
        System.Type parentType = property.serializedObject.targetObject.GetType();
        System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
        //this FieldInfo contains the type.
        fi.SetValue(property.serializedObject.targetObject, value);
    }
}
