using System;
using System.Collections.Generic;

namespace HoneyGrid2D
{
    [System.Serializable]
    public class FlexRow<T> {
        
        public readonly List<T> cells = new List<T>(); // List containing room id's
        private T initialValue;
        
        public FlexRow(int size, T initialValue) {
            this.initialValue = initialValue;

            bool shouldClone = !IsNativeType(typeof(T));
    
            for (int i = 0; i < size; i++) {
                if (shouldClone) {
                    cells.Add(CloneInitialValue(initialValue));
                } else {
                    cells.Add(initialValue);
                }
            }
        }

        private bool IsNativeType(Type type) {
            return type.IsPrimitive || type == typeof(string);
        }

        
        public void InsertCellsRight(int count) {
            for (var x = 0; x < count; x++) {
                cells.Add(initialValue);
            }
        }
        
        private T CloneInitialValue(T initialValue) {
            if (initialValue is ICloneable cloneableValue) {
                return (T)cloneableValue.Clone();
            }

            // If the type doesn't implement ICloneable, you can handle the scenario accordingly.
            // For example, you can throw an exception or return the initial value as-is.
            return default(T);

            // throw new InvalidOperationException($"The type {typeof(T)} does not implement ICloneable.");
        }

    }
}