using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace EditorUtilities
{
    public class Utilities
    {
        private readonly float _startX;
        private float _x;
        private float _y;

        public Utilities(float x, float y)
        {
            _x = x;
            _startX = x;
            _y = y;
        }

        private const float LABEL_WIDTH = 100f;
        private const float FIELD_WIDTH = 300f;
        private const float TOP_MARGIN = 5f;
        private static readonly Color LINE_SEPARATOR_COLOR = new Color(1f, 1f, 1f, 0.2f);
        private const float LINE_SEPARATOR_THICKNESS = 1.5f;
        private const float FOLDOUT_MARGIN = 10f;

        private static readonly float LINE_HEIGHT = EditorGUIUtility.singleLineHeight;

        private void LabelField(string label)
        {
            EditorGUI.LabelField(
                new Rect(_x, _y, LABEL_WIDTH, LINE_HEIGHT),
                label
            );
        }

        public void Label(string label)
        {
            LabelField(label);
            NewLine();
        }

        public Enum EnumField(Enum value, string label)
        {
            LabelField(label);

            value = EditorGUI.EnumPopup(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value
            );

            NewLine();
            return value;
        }

        public Enum EnumFlagsField(Enum value, string label)
        {
            LabelField(label);

            value = EditorGUI.EnumFlagsField(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value
            );

            NewLine();
            return value;
        }

        public int IntField(int value, string label)
        {
            LabelField(label);

            value = EditorGUI.IntField(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value);

            NewLine();
            return value;
        }

        public float SliderField(float value, float min, float max, string label)
        {
            LabelField(label);

            value = EditorGUI.Slider(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value, min, max);

            NewLine();
            return value;
        }

        // TODO: generic object field
        public T ScriptableObjectField<T>(T value, string label) where T : ScriptableObject
        {
            LabelField(label);

            value = (T) EditorGUI.ObjectField(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value,
                typeof(T),
                false);

            NewLine();
            return value;
        }

        public string TextField(string value, string label, int lines = 1)
        {
            LabelField(label);

            value = EditorGUI.TextArea(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, lines * LINE_HEIGHT),
                value,
                EditorStyles.textArea);

            NewLine(lines);
            return value;
        }

        public bool BoolField(bool value, string label)
        {
            LabelField(label);

            value = EditorGUI.ToggleLeft(
                new Rect(_x + LABEL_WIDTH, _y, 20f, LINE_HEIGHT),
                string.Empty,
                value);

            NewLine();
            return value;
        }

        // public T ObjectField<T> (T value, string label) where T : Object
        // {
        //     LabelField(label);
        //     
        //     value = (T) EditorGUI.ObjectField(
        //         new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
        //         value,
        //         typeof(T),
        //         false);
        //     
        //     NewLine();
        //     return value;
        // }

        public int IntPopupField(int value, string label, string[] names, int[] values)
        {
            LabelField(label);

            value = EditorGUI.IntPopup(
                new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                value,
                names,
                values
            );

            NewLine();
            return value;
        }

        public bool FoldoutField(bool value, string label)
        {
            value = EditorGUI.Foldout(
                new Rect(_x + FOLDOUT_MARGIN, _y, FIELD_WIDTH, LINE_HEIGHT),
                value,
                label,
                true,
                EditorStyles.foldout);
            
            NewLine();
            return value;
        }
        
        private void NewLine(int lines = 1)
        {
            _y += (LINE_HEIGHT) * lines;
            TopMargin();
            _x = _startX;
        }

        public void TopMargin()
        {
            _y += TOP_MARGIN;
        }

        public void DrawLineSeparator()
        {
            const float width = LABEL_WIDTH + FIELD_WIDTH;
            var line = new Rect(_x, _y, width, LINE_SEPARATOR_THICKNESS);
            EditorGUI.DrawRect(line, LINE_SEPARATOR_COLOR);
        }
    }

}