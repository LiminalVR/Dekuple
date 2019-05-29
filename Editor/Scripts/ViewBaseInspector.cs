﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dekuple.Agent;
using Dekuple.Model;
using Dekuple.View.Impl;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ViewBase), true)]
public class ViewBaseInspector
    : Editor
{
    private enum ESelectedInspector
    {
        Unity,
        View,
        Agent,
        Model
    }

    protected ViewBase _View;
    protected IModel _Model;
    protected IAgent _Agent;

    private ESelectedInspector _viewSelection = ESelectedInspector.Unity;

    private GUIStyle _memberLabelStyle;

    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    public override void OnInspectorGUI()
    {
        if (Preferences.Prefs != null)
        {
            if (!Preferences.Prefs.UseViewBaseInspector)
            {
                DrawDefaultInspector();
                return;
            }
        }

        InitStyles();

        Debug.Log(target);
        _View = (ViewBase)target;
        Debug.Log(_View);
        _Agent = _View.AgentBase;
        Debug.Log(_Agent);
        _Model = _View.Model;
        Debug.Log(_Model);

        GUILayout.Space(2);

        GUIStyle toolbarStyle = new GUIStyle("ToolbarButtonFlat");
        GUIStyle activeToolbarStyle = new GUIStyle(toolbarStyle) { normal = toolbarStyle.active, fontStyle = FontStyle.Bold };

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Unity", _viewSelection == ESelectedInspector.Unity ? activeToolbarStyle : toolbarStyle, GUILayout.ExpandWidth(true)))
            _viewSelection = ESelectedInspector.Unity;
        GUI.enabled = _View != null;
        if (GUILayout.Button("View", _viewSelection == ESelectedInspector.View ? activeToolbarStyle : toolbarStyle, GUILayout.ExpandWidth(true)))
            _viewSelection = ESelectedInspector.View;
        GUI.enabled = _Agent != null;
        if (GUILayout.Button("Agent", _viewSelection == ESelectedInspector.Agent ? activeToolbarStyle : toolbarStyle, GUILayout.ExpandWidth(true)))
            _viewSelection = ESelectedInspector.Agent;
        GUI.enabled = _Model != null;
        if (GUILayout.Button("Model", _viewSelection == ESelectedInspector.Model ? activeToolbarStyle : toolbarStyle, GUILayout.ExpandWidth(true)))
            _viewSelection = ESelectedInspector.Model;
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        switch (_viewSelection)
        {
            case ESelectedInspector.Unity:
                DrawDefaultInspector();
                break;
            case ESelectedInspector.View:
                DrawViewDetails();
                break;
            case ESelectedInspector.Agent:
                DrawAgentDetails();
                break;
            case ESelectedInspector.Model:
                DrawModelDetails();
                break;
        }
    }

    private void InitStyles()
    {
        _memberLabelStyle = new GUIStyle("ObjectPickerResultsEven");
    }

    public virtual void DrawModelDetails() => DrawViaReflection(_Model);
    public virtual void DrawAgentDetails() => DrawViaReflection(_Agent);
    public virtual void DrawViewDetails() => DrawViaReflection(_View);

    private void DrawViaReflection<T>(T reference)
    {
        if (reference == null)
            return;

        Type type = reference.GetType();
        FieldInfo[] fields = type.GetFields(Flags);
        PropertyInfo[] properties = type.GetProperties(Flags);
        MemberInfo[] members = fields.Select(f => f as MemberInfo).Union(properties.Select(p => p as MemberInfo)).ToArray();

        GUILayout.BeginVertical();
        for (var i = 0; i < members.Length; i++)
        {
            var member = members[i];
            if (member.IsDefined(typeof(ObsoleteAttribute), true))
                continue;

            object value = null;
            if (member is FieldInfo field)
                value = field.GetValue(reference);
            if (member is PropertyInfo property)
                value = property.GetValue(reference);
            var memberName = member.Name;

            if (i%2 == 0)
                GUI.backgroundColor = Color.white*0.95f;
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(memberName, value?.ToString(), _memberLabelStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
        }
        GUILayout.EndVertical();
    }
}
