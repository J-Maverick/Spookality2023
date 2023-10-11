
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnpointRandomizer : UdonSharpBehaviour
{
    public float distance = 1f;
    void Start()
    {
        Vector3 pos = transform.position;
        pos.x += Random.Range(-distance, distance);
        pos.z += Random.Range(-distance, distance);
        transform.position = pos;
    }
}
