using UnityEngine;
using System.Linq;

namespace tdk.Utilities
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// This method is used to hide the GameObject in the Hierarchy view.
        /// </summary>
        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        /// <summary>
        /// Gets a component of the given type attached to the GameObject. If that type of component does not exist, it adds one.
        /// </summary> 
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();

            return component;
        }

        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        /// <summary>
        /// Destroys all children of the game object
        /// </summary>
        public static void DestroyChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildren();
        }

        /// <summary>
        /// Immediately destroys all children of the given GameObject.
        /// </summary>
        public static void DestroyChildrenImmediate(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildrenImmediate();
        }

        /// <summary>
        /// Enables all child GameObjects associated with the given GameObject.
        /// </summary>
        public static void EnableChildren(this GameObject gameObject)
        {
            gameObject.transform.EnableChildren();
        }

        /// <summary>
        /// Disables all child GameObjects associated with the given GameObject.
        /// </summary>
        public static void DisableChildren(this GameObject gameObject)
        {
            gameObject.transform.DisableChildren();
        }

        /// <summary>
        /// Returns the hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        public static string Path(this GameObject gameObject)
        {
            return "/" + string.Join("/",
                gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());
        }

        /// <summary>
        /// Returns the full hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        public static string PathFull(this GameObject gameObject)
        {
            return gameObject.Path() + "/" + gameObject.name;
        }

        /// <summary>
        /// Recursively sets the provided layer for this GameObject and all of its descendants in the Unity scene hierarchy.
        /// </summary>
        public static void SetLayersRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            gameObject.transform.ForEveryChild(child => child.gameObject.SetLayersRecursively(layer));
        }
    }
}