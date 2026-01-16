using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class SkyBoxChoice : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes;
    [SerializeField] private int selectedIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplySkybox();
    }

    void OnValidate()
    {
        ApplySkybox();
    }

    private void ApplySkybox()
    {
        if (skyboxes == null || skyboxes.Length == 0)
        {
            return;
        }

        int clampedIndex = Mathf.Clamp(selectedIndex, 0, skyboxes.Length - 1);
        if (selectedIndex != clampedIndex)
        {
            selectedIndex = clampedIndex;
        }
        Material selectedSkybox = skyboxes[clampedIndex];
        if (selectedSkybox == null)
        {
            return;
        }

        RenderSettings.skybox = selectedSkybox;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkyBoxChoice))]
public class SkyBoxChoiceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty skyboxesProp = serializedObject.FindProperty("skyboxes");
        SerializedProperty selectedIndexProp = serializedObject.FindProperty("selectedIndex");

        EditorGUILayout.PropertyField(skyboxesProp, true);

        int count = skyboxesProp.arraySize;
        if (count > 0)
        {
            string[] options = new string[count];
            for (int i = 0; i < count; i++)
            {
                SerializedProperty element = skyboxesProp.GetArrayElementAtIndex(i);
                Object obj = element.objectReferenceValue;
                options[i] = obj != null ? obj.name : $"Skybox {i}";
            }

            int clampedIndex = Mathf.Clamp(selectedIndexProp.intValue, 0, count - 1);
            int newIndex = EditorGUILayout.Popup("Selected Skybox", clampedIndex, options);
            if (newIndex != selectedIndexProp.intValue)
            {
                selectedIndexProp.intValue = newIndex;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign at least one skybox to enable selection.", MessageType.Info);
            selectedIndexProp.intValue = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
