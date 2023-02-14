using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GearRef
{

    public enum GearReference
    {
        VARINHA_BASE,
        VARINHA_PONTA,
        PEITORAL,
        CAPACETE,
        PERNAS,
        TENIS
}

    public class GearBase : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    [System.Serializable]
    public class ItemSetup
    {
        public GearReference itemType;
        public SO_Gear soInt;
    }
}
