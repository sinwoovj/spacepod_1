using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class ParticleManager : MonoBehaviour
{
    public GameObject explosionParticle;

    public void PlayParticle(BirdDir dir, int pos)
    {
        GameObject particle = explosionParticle;
        string a = (dir == BirdDir.Left ? "L" : "R") + pos.ToString();
        GameObject explosion = Instantiate(particle,  GameObject.Find(a).transform.position + new Vector3(dir == BirdDir.Left ? 2 : -2,0, dir == BirdDir.Left ? -1 : 0), Quaternion.identity);
        explosion.GetComponentInParent<ParticleSystem>().Play();
        Debug.Log(a);
    }
}
