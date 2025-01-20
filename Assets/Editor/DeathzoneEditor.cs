// Custom editor for Deathzone script
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Deathzone))]
public class DeathzoneEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Get reference to the target script
		Deathzone deathzone = (Deathzone)target;

		// Draw default enum field for choosing action
		deathzone.action = (Deathzone.DeathAction)EditorGUILayout.EnumPopup("Action", deathzone.action);

		// Show the GameOver object field only if the action is "OpenGameObject"
		if (deathzone.action == Deathzone.DeathAction.OpenGameObject)
		{
			deathzone.gameOverObject = (GameObject)EditorGUILayout.ObjectField("GameOver Object", deathzone.gameOverObject, typeof(GameObject), true);
		}

		// Apply any changes to the serialized object
		if (GUI.changed)
		{
			EditorUtility.SetDirty(deathzone);
		}
	}
}
