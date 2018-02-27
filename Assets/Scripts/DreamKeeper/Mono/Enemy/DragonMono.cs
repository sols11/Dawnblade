using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class DragonMono : IEnemyMono
    {
        public ParticleSystem[] BreathFire;
        public Light FireLight;

        public void StartFire()
        {
            foreach(var p in BreathFire)
                p.enableEmission = true;
            FireLight.enabled = true;
        }

        public void StopFire()
        {
            foreach (var p in BreathFire)
                p.enableEmission = false;
            FireLight.enabled = false;
        }

    }
}