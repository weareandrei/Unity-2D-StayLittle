using UnityEngine;
using UnityEditor;

namespace HoneyGrid2D {
    public abstract class Grid2DDrawer<T> : PropertyDrawer {
        private static float LineHeight => EditorGUIUtility.singleLineHeight;
        
        private const float FirstLineMargin = 16f;
        private const float LastLineMargin = 32f;
        
        private static readonly Vector2 CellSpacing = new Vector2(5f, 5f);
        
        private SerializedProperty gridSizeProperty;
        private SerializedProperty rowsProperty;
        private SerializedProperty thisProperty;
        private Vector2Int cellSizeProperty = new Vector2Int(16, 16);

        protected abstract float GetCellWidth();
        protected abstract float GetCellHeight();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            thisProperty = property;
            
            // Prepare properties : 
            GetRowsProperty(property);
            GetGridSizeProperty(property);
            
            if (gridSizeProperty == null || gridSizeProperty.vector2IntValue.x == 0 || rowsProperty == null) {
                return;
            }
            
            position = EditorGUI.IndentedRect(position);
            
            EditorGUI.BeginProperty(position, label, property);

            var foldoutRect = new Rect(position) {
                height = LineHeight
            };
            EditorGUI.indentLevel = 0;
            
            label.tooltip = $"Size: {gridSizeProperty.vector2IntValue.x}x{gridSizeProperty.vector2IntValue.y}";

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label, 
                menuAction: ShowHeaderContextMenu);
            EditorGUI.EndFoldoutHeaderGroup();

            position.y += LineHeight;
            
            if (property.isExpanded)
            {
                position.y += FirstLineMargin;

                DisplayGrid(position);
            }
            EditorGUI.EndProperty();
        }
        
        private void DisplayGrid(Rect position) {
            var cellRect = new Rect(position.x, position.y, 
                20, 20);
                
            for (var y = 0; y < gridSizeProperty.vector2IntValue.y; y++) {
                for (var x = 0; x < gridSizeProperty.vector2IntValue.x; x++) {
                    var pos = new Rect(cellRect) {
                        x = cellRect.x + (cellRect.width + CellSpacing.x) * x,
                        y = cellRect.y + (cellRect.height + CellSpacing.y) * y
                    };

                    var property = GetRowAt(y).GetArrayElementAtIndex(x);
                    EditorGUI.PropertyField(pos, property, GUIContent.none);
                }
            }
        }
        
        // This function will get the Grid2D's rows property and put it
        //  inside of the rowsProperty variable
        private void GetRowsProperty(SerializedProperty property) =>
            TryFindPropertyRelative(property, "rows", out rowsProperty);

        private void GetGridSizeProperty(SerializedProperty property) =>
            TryFindPropertyRelative(property, "gridSize", out gridSizeProperty);
        
        private SerializedProperty GetRowAt(int idx) {
            return rowsProperty.GetArrayElementAtIndex(idx).FindPropertyRelative("cells");
        }
        
        private void TryFindPropertyRelative(SerializedProperty parent, string relativePropertyPath, 
            out SerializedProperty prop) {
            prop = parent.FindPropertyRelative(relativePropertyPath);
        }

        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var height = base.GetPropertyHeight(property, label);

            GetGridSizeProperty(property);

            if (property.isExpanded) {
                height += FirstLineMargin;

                height += gridSizeProperty.vector2IntValue.y * (cellSizeProperty.y + CellSpacing.y) - CellSpacing.y;

                height += LastLineMargin;
            }

            return height;
        }
        
        
        
        protected void ShowHeaderContextMenu(Rect position) {
            var menu = new GenericMenu();
            menu.AddSeparator(""); // An empty string will create a separator at the top level
            menu.DropDown(position);
        }

    }
}
