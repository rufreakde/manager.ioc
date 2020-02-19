using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true)]
public class SetterUpdaterEditor : Editor
{
    enum PropertyState
    {
        firstSave,
        modified,
        unchanged
    }

    private readonly Dictionary<string, object> storedReferenceValues = new Dictionary<string, object>();
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

    private PropertyState compareProperty(string key, object value)
    {
        object reference;
        if( storedReferenceValues.TryGetValue(key, out reference))
        {
            if(reference != value)
            {
                storedReferenceValues[key] = value;
                return PropertyState.modified;
            }
            else
            {
                return PropertyState.unchanged;
            }
        }
        else
        {
            storedReferenceValues.Add(key, value );
            return PropertyState.firstSave;
        }
    }

    private void executeSetters()
    {
        PropertyInfo[] setters = serializedObject.targetObject.GetType().GetProperties();
        foreach (PropertyInfo setter in setters)
        {
            try
            {
                MethodInfo setterMethodProperty = setter.GetSetMethod();
                if (setterMethodProperty != null)
                {                   
                    string propertyName = trimGeneratedSetterName(setterMethodProperty.Name);
                    var propertyValue = serializedObject
                            .targetObject
                            .GetType()
                            .GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
                            .GetValue(serializedObject.targetObject);
                    var state = compareProperty(propertyName, propertyValue);

                    if (state == PropertyState.modified || state == PropertyState.firstSave){                      
                        setterMethodProperty.Invoke(
                            serializedObject.targetObject, new object[] {
                                propertyValue
                            });
                    }
                }
            }
            catch{ } //ignore because property does not exist
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        executeSetters();
    }
}
