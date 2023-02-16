using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using System.IO;


public class CalculateDamage : Singleton<CalculateDamage>
    {
        public _SOPlayer sOPlayer;

         private void Start()
        {
            sOPlayer = gameObject.GetComponent<LineDraw>().SOPlayer;
        }

        public float damage()
        {
            float damageTotal = 0;
            damageTotal += sOPlayer.varinhaBase.damageMultiply;
            damageTotal += sOPlayer.varinhaPonta.damageMultiply;
            damageTotal += sOPlayer.tenis.damageMultiply;
            damageTotal += sOPlayer.Capacete.damageMultiply;
            damageTotal += sOPlayer.Peitoral.damageMultiply;
            damageTotal += sOPlayer.Pernas.damageMultiply;
            return damageTotal;
        }
    }

