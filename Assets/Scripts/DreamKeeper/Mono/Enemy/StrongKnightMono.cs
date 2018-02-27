using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class StrongKnightMono : IEnemyMono
    {
        private EnemyStrongKnight strongKnight;

        public override void Initialize()
        {
            base.Initialize();
            strongKnight = EnemyMedi.Enemy as EnemyStrongKnight;
        }



    }
}