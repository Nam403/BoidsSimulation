using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Boundery")]
public class Boundery : ScriptableObject
{
    private float xLimit;
    private float yLimit;
    public float XLimit
    {
        get
        {
            CalculateLimit();
            return xLimit;
        }
    }
    public float YLimit
    {
        get
        {
            CalculateLimit();
            return yLimit;
        }
    }
    private void CalculateLimit()
    {
        yLimit = Camera.main.orthographicSize + 1f;
        xLimit = yLimit * Screen.width / Screen.height + 1f;
    }
}
