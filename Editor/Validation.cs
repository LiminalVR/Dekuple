﻿using System;
using System.Linq;
using System.Reflection;
using Dekuple.View.Impl;
using UnityEditor;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dekuple
{
    public static class Validation
    {
        public static BindingFlags Flags => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private const string MethodColor = "#2899e5";
        private const string EventColor = "#e529c2";

        [MenuItem("Tools/Dekuple/Validate Unity Methods", false)]
        private static void FindUnityMethods()
        {
            var views = Object.FindObjectsOfType<ViewBase>();
            foreach (Type type in views.Select(view => view.GetType()))
            {
                bool isValid = true;
                StringBuilder str = new StringBuilder($"<b>{type}</b>\n");
                if (type.GetMethod("Awake", Flags) != null)
                {
                    str.AppendLine($"└ Contains <color=red>Awake</color>, replace with <color={MethodColor}>Create</color>");
                    isValid = false;
                }

                if (type.GetMethod("Start", Flags) != null)
                {
                    str.AppendLine($"└ Contains <color=red>Start</color>, replace with <color={MethodColor}>Begin</color>");
                    isValid = false;
                }

                if (type.GetMethod("Update", Flags) != null)
                {
                    str.AppendLine($"└ Contains <color=red>Update</color>, replace with <color={MethodColor}>Step</color>");
                    isValid = false;
                }

                if (type.GetMethod("OnDestroy", Flags) != null)
                {
                    str.AppendLine($"└ Contains <color=red>OnDestroy</color>, replace with <color={EventColor}>OnDestroyed</color>");
                    isValid = false;
                }

                if (isValid)
                    continue;

                Debug.LogWarningFormat(str.ToString());
            }
        }
    }
}
