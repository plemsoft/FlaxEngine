// Copyright (c) 2012-2022 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit Double2 value type properties.
    /// </summary>
    [CustomEditor(typeof(Double2)), DefaultEditor]
    public class Double2Editor : CustomEditor
    {
        /// <summary>
        /// The X component editor.
        /// </summary>
        protected DoubleValueElement XElement;

        /// <summary>
        /// The Y component editor.
        /// </summary>
        protected DoubleValueElement YElement;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var grid = layout.CustomContainer<UniformGridPanel>();
            var gridControl = grid.CustomControl;
            gridControl.ClipChildren = false;
            gridControl.Height = TextBox.DefaultHeight;
            gridControl.SlotsHorizontally = 2;
            gridControl.SlotsVertically = 1;

            LimitAttribute limit = null;
            var attributes = Values.GetAttributes();
            if (attributes != null)
            {
                limit = (LimitAttribute)attributes.FirstOrDefault(x => x is LimitAttribute);
            }

            XElement = grid.DoubleValue();
            XElement.SetLimits(limit);
            XElement.DoubleValue.ValueChanged += OnValueChanged;
            XElement.DoubleValue.SlidingEnd += ClearToken;

            YElement = grid.DoubleValue();
            YElement.SetLimits(limit);
            YElement.DoubleValue.ValueChanged += OnValueChanged;
            YElement.DoubleValue.SlidingEnd += ClearToken;
        }

        private void OnValueChanged()
        {
            if (IsSetBlocked)
                return;

            var isSliding = XElement.IsSliding || YElement.IsSliding;
            var token = isSliding ? this : null;
            var value = new Double2(XElement.DoubleValue.Value, YElement.DoubleValue.Value);
            SetValue(value, token);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (HasDifferentValues)
            {
                // TODO: support different values for ValueBox<T>
            }
            else
            {
                var value = (Double2)Values[0];
                XElement.DoubleValue.Value = value.X;
                YElement.DoubleValue.Value = value.Y;
            }
        }
    }
}