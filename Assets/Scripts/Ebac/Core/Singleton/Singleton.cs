using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ebac.Core.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instantiate;

        private void Awake()
        {
            if (Instantiate == null)
                Instantiate = GetComponent<T>();
            else
                Destroy(gameObject);
        }
    }
}
