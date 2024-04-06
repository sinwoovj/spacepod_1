using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBird : MonoBehaviour
{
    public bool reachDestination = false;
    public float birdSpeed = 3;

    public void MoveStart(BirdDir dir, int pos) // dir: 방향[L,R] , pos: 위치 [1,2,3,4,5]
    {
        GameObject.Find("ParticleManager").GetComponent<ParticleManager>().PlayParticle(dir, pos);
        StartCoroutine(MovingBird(dir));
    }

    IEnumerator MovingBird(BirdDir birdDir)
    {
        while (!reachDestination)
        {
            transform.position += (birdDir == BirdDir.Left ? Vector3.right : Vector3.left) * birdSpeed * Time.deltaTime;
            yield return null;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        reachDestination = true;
        Destroy(gameObject);
    }
}

public enum BirdDir
{
    Left, Right
}