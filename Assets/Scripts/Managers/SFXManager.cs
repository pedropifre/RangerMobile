using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

namespace SystemSFX
{

    public class SFXManager : Singleton<SFXManager>
    {
        public List<Sounds> sounds;
        public AudioClip PlaySFX(string nome)
        {
            foreach (var i in sounds)
            {
                if (i.nameSound == nome)
                {
                    return i.audio;
                    
                }
            }
            return null;

        }
    }
    [System.Serializable]
    public class Sounds
    {
        public string nameSound;
        public AudioClip audio;
    }
}