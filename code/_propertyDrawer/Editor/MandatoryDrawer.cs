using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MandatoryAttribute))]
public class MandatoryDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var tAttribute = attribute as MandatoryAttribute;

        Rect leftPosition = new Rect(position.x, position.y, position.width*0.75f, position.height);
        Rect rightPosition = new Rect(position.x + position.width*0.75f, position.y, position.width*0.25f, position.height);

        if(property.objectReferenceValue != null){
            EditorGUI.PropertyField(position, property, label, true);
        }
        else{
            EditorGUI.PropertyField(leftPosition, property, label, true);
            EditorGUI.HelpBox(rightPosition, string.Format("Mandatory"), MessageType.Error);
            Debug.LogError("Mandatory property was not set!");
        }
    }
}