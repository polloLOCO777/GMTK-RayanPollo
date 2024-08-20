using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HolderManager : Singleton<HolderManager>
{
    [field: SerializeField] public Transform WalkParticleHolder { get; private set; }
    [field: SerializeField] public Transform JumpParticleHolder { get; private set; }
    [field: SerializeField] public Transform ProxyHolder { get; private set; }
}
