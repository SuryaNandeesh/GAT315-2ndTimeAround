using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(InputListener))]
//public class InputListenerEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();

//		var inputActionRefProp = serializedObject.FindProperty("inputActionReference");
//		EditorGUILayout.PropertyField(inputActionRefProp);

//		InputListener relay = (InputListener)target;
//		var inputAction = relay.InputEvent?.inputActionReference?.action;

//		if (inputAction != null)
//		{
//			string type = inputAction.expectedControlType;

//			EditorGUILayout.Space();
//			EditorGUILayout.LabelField("Events for Input Type: " + type, EditorStyles.boldLabel);

//			switch (type)
//			{
//				case "Vector2":
//					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnVector2Input"));
//					break;
//				case "Axis":
//				case "Float":
//					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFloatInput"));
//					break;
//				case "Button":
//					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnButtonPressed"));
//					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnButtonReleased"));
//					break;
//				default:
//					EditorGUILayout.HelpBox("Unsupported input type: " + type, MessageType.Warning);
//					break;
//			}
//		}
//		else
//		{
//			EditorGUILayout.HelpBox("Assign an InputActionReference to configure events.", MessageType.Info);
//		}

//		serializedObject.ApplyModifiedProperties();
//	}
//}
