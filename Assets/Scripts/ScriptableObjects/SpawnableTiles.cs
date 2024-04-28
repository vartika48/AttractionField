using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "MagnetTile")]
public class SpawnableTiles : ScriptableObject
{
    [SerializeField] public EPolarity eCharge;
}
