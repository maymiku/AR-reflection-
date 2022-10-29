using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public int maxBounce = 5;
    public int rayDist = 300;
    public Transform start;
    private LineRenderer lr;
    public bool reflectOnlyMirror = false;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, start.position);
    }

    void Update()
    {
        CastLaser(transform.position, transform.forward);
    }

    void CastLaser(Vector3 pos, Vector3 dir)
    {
        lr.SetPosition(0, start.position);
        
        for(int i = 0; i< maxBounce; i++)
        {
            Ray ray = new Ray(pos, dir);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, rayDist , 1))
            {
                pos = hit.point;
                dir = Vector3.Reflect(dir, hit.normal);
                lr.SetPosition(i + 1, hit.point);

                if(hit.transform.tag != "Mirror" && reflectOnlyMirror)
                {
                    for(int j = i+1; j<= maxBounce; j++)
                    {
                        lr.SetPosition(j, hit.point);
                    }
                    break;
                }
            }
            else
            {
                lr.SetPosition(i + 1, pos + dir.normalized * rayDist);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }
}
