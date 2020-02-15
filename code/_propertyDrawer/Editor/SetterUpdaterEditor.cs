using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class SetterUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PropertyInfo[] Setters = serializedObject.targetObject.GetType().GetProperties();

        foreach (PropertyInfo setter in Setters)
        {
            try
            {
                MethodInfo setterMethodProperty = setter.GetSetMethod();
                if (setterMethodProperty != null)
                {
                    string prefixCheck = setterMethodProperty.Name.Substring(0, 4);
                    string propertyName = setterMethodProperty.Name;
                    if (prefixCheck != "set_")
                    {
                        propertyName = System.Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                    }
                    else
                    {
                        propertyName = System.Char.ToLowerInvariant(setterMethodProperty.Name[4]) + setterMethodProperty.Name.Substring(5);
                    }
                    
                    setterMethodProperty.Invoke(serializedObject.targetObject, new object[] { serializedObject.targetObject.GetType().GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(serializedObject.targetObject) });
                }
            }
            catch(System.NullReferenceException nullError)
            {
                // do nothing the property is not defined...
            }
        }
    }
}
