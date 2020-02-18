using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class SetterUpdaterEditor : Editor
{
    private string trimGeneratedSetterName(string _PropertyName)
    {
        string prefix = _PropertyName.Substring(0, 4);

        if (prefix != "set_")
        {
            _PropertyName = System.Char.ToLowerInvariant(_PropertyName[0]) + _PropertyName.Substring(1);
        }
        else
        {
            _PropertyName = System.Char.ToLowerInvariant(_PropertyName[4]) + _PropertyName.Substring(5);
        }

        return _PropertyName;
    }

    private void executeSetters()
    {
        PropertyInfo[] Setters = serializedObject.targetObject.GetType().GetProperties();
        foreach (PropertyInfo setter in Setters)
        {
            try
            {
                MethodInfo setterMethodProperty = setter.GetSetMethod();
                if (setterMethodProperty != null)
                {
                    string propertyName = trimGeneratedSetterName(setterMethodProperty.Name);
                    setterMethodProperty.Invoke(
                        serializedObject.targetObject, new object[] {
                            serializedObject
                            .targetObject
                            .GetType()
                            .GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
                            .GetValue(serializedObject.targetObject)
                        });
                }
            }
            catch{ } //ignore because property did not exist
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        executeSetters();
    }
}
