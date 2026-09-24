using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{

    // The Pool class controls access to the pooled objects, maintaining a list of available objects and a collection of objects that have already been requested from the pool and are still in use. The pool also ensures that objects that have been released are returned to a suitable state, ready for the next time they are requested. 
    public class Pool : MonoBehaviour
    {
        private static List<GameObject> _available = new List<GameObject>();
        private static List<GameObject> _inUse = new List<GameObject>();

        private static GameObject prefab;

        //this variable is a small "hack" around accessing the "prefab" variable above: unity doesn't show static variables in the inspector, so to get around it I made non-static "Prefab" through which I can then set "prefab" in Start
        public GameObject Prefab;

        private void Start()
        {
            prefab = Prefab;
        }

        /// <summary>
        /// Method that returns us an object from the pool. If no object is available it then creates a new one
        /// </summary>
        /// <returns></returns>
        public static GameObject GetObject()
        {
            lock (_available)
            {
                if (_available.Count != 0)
                {
                    GameObject instance = _available[0];
                    instance.SetActive(true);
                    _inUse.Add(instance);
                    _available.Remove(instance);
                    
                    return instance;
                }
                else
                {
                    
                    GameObject instance = Instantiate(prefab);
                    _inUse.Add(instance);
                                      
                    return instance;
                }
                
                return null;
            }

            
            
        }

        /// <summary>
        /// Releases an object back into the pool, so we can then re-use it 
        /// </summary>
        /// <param name="go"></param>
        public static void ReleaseObject(GameObject go)
        {
            lock (_available)
            {
                if (_inUse.Contains(go))
                {
                    _available.Add(go);
                    _inUse.Remove(go);
                    CleanUp(go);
                }
                
                
                
                return;
            }
        }

        /// <summary>
        /// Used if needed to clean up info from the gameobject, or do some processing once it goes back into the pool
        /// </summary>
        /// <param name="go"></param>
        private static void CleanUp(GameObject go)
        {
            go.SetActive(false);
            return;
        }
    }
}