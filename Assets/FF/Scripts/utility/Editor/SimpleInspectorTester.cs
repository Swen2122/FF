#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class SimpleInspectorTester : EditorWindow
{
    private Object targetObject;

    [MenuItem("Tools/Simple Variable Tester")]
    public static void ShowWindow()
    {
        GetWindow<SimpleInspectorTester>("Variable Tester");
    }

    private void OnGUI()
    {
        GUILayout.Label("Drag any MonoBehaviour or ScriptableObject", EditorStyles.boldLabel);
        targetObject = EditorGUILayout.ObjectField("Target", targetObject, typeof(Object), true);

        if (targetObject == null)
            return;

        EditorGUILayout.Space(10);

        var type = targetObject.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        GUILayout.Label("Fields", EditorStyles.boldLabel);
        foreach (var field in fields)
        {
            if (field.IsPrivate && field.GetCustomAttribute<SerializeField>() == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(field.Name, GUILayout.Width(150));

            object value = field.GetValue(targetObject);

            if (field.FieldType == typeof(bool))
            {
                bool newValue = EditorGUILayout.Toggle((bool)value);
                if (newValue != (bool)value)
                    field.SetValue(targetObject, newValue);
            }
            else if (field.FieldType == typeof(int))
            {
                int newValue = EditorGUILayout.IntField((int)value);
                if (newValue != (int)value)
                    field.SetValue(targetObject, newValue);
            }
            else if (field.FieldType == typeof(float))
            {
                float newValue = EditorGUILayout.FloatField((float)value);
                if (Mathf.Abs(newValue - (float)value) > Mathf.Epsilon)
                    field.SetValue(targetObject, newValue);
            }
            else if (field.FieldType == typeof(string))
            {
                string newValue = EditorGUILayout.TextField((string)value);
                if (newValue != (string)value)
                    field.SetValue(targetObject, newValue);
            }
            else if (typeof(Object).IsAssignableFrom(field.FieldType))
            {
                Object newValue = EditorGUILayout.ObjectField((Object)value, field.FieldType, true);
                if (newValue != (Object)value)
                    field.SetValue(targetObject, newValue);
            }
            else
            {
                EditorGUILayout.LabelField($"({field.FieldType.Name})", GUILayout.Width(100));
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Methods", EditorStyles.boldLabel);

        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            if (method.GetParameters().Length == 0 && !method.IsSpecialName)
            {
                if (GUILayout.Button($"Invoke: {method.Name}"))
                {
                    method.Invoke(targetObject, null);
                }
            }
        }

        EditorUtility.SetDirty(targetObject);
    }
}
#endif
