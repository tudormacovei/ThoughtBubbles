using System;

namespace tdk.Utilities
{
#if UNITY_EDITOR
    public static class SuppressOnValidate
    {
        public static void SafeOnValidate(Action onValidateAction)
        {
            UnityEditor.EditorApplication.delayCall += _OnValidate;


            void _OnValidate()
            {
                UnityEditor.EditorApplication.delayCall -= _OnValidate;

                onValidateAction();
            }
        }
    }
#endif
}
