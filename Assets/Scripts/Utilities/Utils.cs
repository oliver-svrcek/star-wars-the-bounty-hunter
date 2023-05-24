using Exceptions;
using UnityEngine;

namespace Utilities
{
    public static class Utils
    {
        public static T GetComponentOrThrow<T>(string gameObjectHierarchyPath) where T : Component
        {
            var gameObject = GetGameObjectOrThrow(gameObjectHierarchyPath);
            return GetComponentOrThrow<T>(gameObject);
        }
        
        public static T GetComponentOrThrow<T>(GameObject gameObject) where T : Component
        {
            T component;
            
            if (gameObject is null)
            {
                throw new MissingGameObjectException("GameObject is null.");
            }
            if ((component = gameObject.GetComponent<T>()) is null)
            {
                throw new MissingComponentException(gameObject.name + " game object is missing " + typeof(T) + " component.");
            }

            return component;
        }
        
        public static T GetComponentOrThrow<T>(GameObject gameObject, string gameObjectHierarchyPath) where T : Component
        {
            var childGameObject = GetGameObjectOrThrow(gameObject, gameObjectHierarchyPath);
            return GetComponentOrThrow<T>(childGameObject);
        }

        public static T GetComponentInChildrenOrThrow<T>(string gameObjectHierarchyPath) where T : Component
        {
            var gameObject = GetGameObjectOrThrow(gameObjectHierarchyPath);
            return GetComponentInChildrenOrThrow<T>(gameObject);
        }
        
        public static T GetComponentInChildrenOrThrow<T>(GameObject gameObject) where T : Component
        {
            T component;
            
            if (gameObject is null)
            {
                throw new MissingGameObjectException("GameObject is null.");
            }
            if ((component = gameObject.GetComponentInChildren<T>()) is null)
            {
                throw new MissingComponentException(gameObject.name + " game object is missing " + typeof(T) + " component.");
            }

            return component;
        }

        public static GameObject GetGameObjectOrThrow(string gameObjectHierarchyPath)
        {
            GameObject gameObject;

            if ((gameObject = GameObject.Find(gameObjectHierarchyPath)) is null)
            {
                throw new MissingGameObjectException(gameObjectHierarchyPath + " game object was not found in game object hierarchy.");
            }

            return gameObject;
        }
        
        public static GameObject GetGameObjectOrThrow(GameObject gameObject, string gameObjectHierarchyPath)
        {
            if (gameObject is null)
            {
                throw new MissingGameObjectException("GameObject is null.");
            }
            
            var childGameObject = gameObject.transform.Find(gameObjectHierarchyPath).gameObject;
            if (childGameObject is null)
            {
                throw new MissingGameObjectException(gameObjectHierarchyPath + " game object was not found in " + gameObject.name + " game object " + "hierarchy.");
            }
            
            return childGameObject;
        }
        
        public static T GetResourceOrThrow<T>(string resourcePath) where T : Object
        {
            T resource;
        
            if ((resource = Resources.Load<T>(resourcePath)) is null)
            {
                throw new MissingResourceException(resourcePath + " resource was not found in Resources folder.");
            }
        
            return resource;
        }
    }
}