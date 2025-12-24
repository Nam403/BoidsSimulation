using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/List GameObject Variable")]
public class ListBoidVariable : ScriptableObject
{
    public List<Transform> boidTransforms = new List<Transform>();
}