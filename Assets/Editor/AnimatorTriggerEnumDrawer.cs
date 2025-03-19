using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimatorTriggerEnumAttribute))]
public class AnimatorTriggerEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AnimatorTriggerEnumAttribute triggerEnum = (AnimatorTriggerEnumAttribute)attribute;
        Animator animator = triggerEnum.animator;

        if (animator != null)
        {
            List<string> triggerNames = new List<string>();
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    triggerNames.Add(parameter.name);
                }
            }

            if (triggerNames.Count > 0)
            {
                int selectedIndex = Mathf.Max(triggerNames.IndexOf(property.stringValue), 0);
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, triggerNames.ToArray());
                property.stringValue = triggerNames[selectedIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "No trigger parameters found");
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Animator not assigned");
        }
    }
}