using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SMB_Event))]
public class Editor_SMB_Event : Editor
{
    private SerializedProperty _totalFrames;
    private SerializedProperty _currentFrames;
    private SerializedProperty _normalizedTime;
    private SerializedProperty _normalizedTimeUncapped;
    private SerializedProperty _motionTime;
    private SerializedProperty _events;
    private ReorderableList _eventsList;

    private void OnEnable()
    {
        _totalFrames = serializedObject.FindProperty("_totalFrames");            
        _currentFrames = serializedObject.FindProperty("_currentFrame");
        _normalizedTime = serializedObject.FindProperty("_normalizedTime");
        _normalizedTimeUncapped = serializedObject.FindProperty("_normalizedTimeUncapped");
        _motionTime = serializedObject.FindProperty("_motionTime");
        _events = serializedObject.FindProperty("Events");
        _eventsList = new ReorderableList(serializedObject, _events, true, true, true, true);

        _eventsList.drawHeaderCallback = DrawHeaderCallBack;
        _eventsList.drawElementCallback = DrawElementCallback;
        _eventsList.elementHeightCallback += ElementHeightCallback;
    }
    private void DrawHeaderCallBack(Rect rect)
    {
        EditorGUI.LabelField(rect, "Events");
    }
    private void DrawElementCallback(Rect rect, int index, bool isaction, bool isfocused)
    {
        rect.x += 10;
        rect.width -= 10;

        SerializedProperty element = _eventsList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty _eventName = element.FindPropertyRelative("eventName");
        SerializedProperty _timing = element.FindPropertyRelative("timing");
        float _updateFrame = element.FindPropertyRelative("onUpdateFrame").floatValue;

        string elementTitle;
        int _timingValue = _timing.enumValueIndex;
        if (_timingValue == (int)SMBTiming.OnEnter)
        {
            elementTitle = string.IsNullOrEmpty(_eventName.stringValue)
                ? "Event: *Name* (OnEnter)"
                : $"Event: {_eventName.stringValue} (OnEnter)";
        }
        else if (_timingValue == (int)SMBTiming.OnExit)
        {
            elementTitle = string.IsNullOrEmpty(_eventName.stringValue)
                ? "Event: *Name* (OnExit)"
                : $"Event: {_eventName.stringValue} (OnExit)";
        }
        else if (_timingValue == (int)SMBTiming.OnEnd)
        {
            elementTitle = string.IsNullOrEmpty(_eventName.stringValue)
                ? "Event: *Name* (OnEnd)"
                : $"Event: {_eventName.stringValue} (OnEnd)";
        }
        else
        {
            elementTitle = string.IsNullOrEmpty(_eventName.stringValue)
                ? $"Event: *Name* ({_updateFrame}) "
                : $"Event: {_eventName.stringValue} ({_updateFrame})";
        }
            
        EditorGUI.PropertyField(rect, element, new GUIContent(elementTitle) ,true);
    }
    private float ElementHeightCallback(int index)
    {
        SerializedProperty element = _eventsList.serializedProperty.GetArrayElementAtIndex(index);
        float propertyHeight = EditorGUI.GetPropertyHeight(element, true);
        return propertyHeight;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(_totalFrames);
        EditorGUILayout.PropertyField(_currentFrames);
        EditorGUILayout.PropertyField(_normalizedTime);
        EditorGUILayout.PropertyField(_normalizedTimeUncapped);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(_motionTime);
        _eventsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

}
