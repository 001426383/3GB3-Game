using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public float elasticity; //[0,1]. Elasticity determines whether collision is elastic or inelastic. Higher is elastic.
    private void FixedUpdate()
    {
        elasticity = Mathf.Clamp(elasticity, 0.0f, 1.0f);
    }
}
