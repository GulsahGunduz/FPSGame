using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwatController : MonoBehaviour
{
    NavMeshAgent navmesh;
    Animator animator;

    GameObject target;
    float shootDistance = 7;
    float closeDistance = 12;
    GameObject closeTarget;

    bool close;

    GameObject[] activePointLists;

    public GameObject[] discoverPoints1;
    public GameObject[] discoverPoints2;
    public GameObject[] discoverPoints3;
    public bool isDiscover;
    bool discoverLocked;
    Coroutine discover;

    Vector3 startPoint;

    private void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPoint = transform.position;

    }
    void DiscoverControl()
    {
        int rand = Random.Range(1, 3);

        if(rand == 1)
        {
            activePointLists = discoverPoints1;
        }
        else if(rand == 2)
        {
            activePointLists = discoverPoints2;
        }
        else if (rand == 3)
        {
            activePointLists = discoverPoints3;
        }

        discover = StartCoroutine(SetDiscoverPoints());

    }

    IEnumerator SetDiscoverPoints()
    {
        discoverLocked = false;
        isDiscover = true;
        animator.SetBool("walk", true);
        int totalPoints = activePointLists.Length-1;
        int i = 0;

        while (true && !isDiscover)
        {

            if(Vector3.Distance(transform.position, discoverPoints1[i].transform.position)  <= 1f )
            {
                if(totalPoints > i)
                {
                    ++i;
                    navmesh.SetDestination(activePointLists[i].transform.position);
                }
                else
                {
                    navmesh.stoppingDistance = 1;
                    navmesh.SetDestination(startPoint);

                    if (navmesh.remainingDistance <= 1)
                    {
                        animator.SetBool("walk", false);
                        isDiscover = false;
                        StopCoroutine(discover);
                    }
                }
            }
            else
            {
                if (totalPoints > i)
                {
                    navmesh.SetDestination(activePointLists[i].transform.position);
                }
            }
            yield return null;
        }
    }

    private void LateUpdate()
    {
        if (discoverLocked)
        {
            discover = StartCoroutine(SetDiscoverPoints());
        }

        WalkDistance();
        ShootDistance();

    }

    void ShootDistance()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootDistance);
     

        foreach (var objects in hitColliders)
        {
            if (objects.gameObject.CompareTag("Player"))
            {
                animator.SetBool("walk", false);
                navmesh.isStopped = true;
                animator.SetBool("shoot", true);
                
            }
            else
            {
                animator.SetBool("shoot", false);
            }
        }
    }
   
    void WalkDistance()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, closeDistance);
        
        foreach (var objects in hitColliders)
        {
            if (objects.gameObject.CompareTag("Player"))
            {
                animator.SetBool("walk", true);
                target = objects.gameObject;

                navmesh.SetDestination(objects.transform.position);
                close = true;
            }
            else
            {
                if (close)
                {
                    target = null;

                    if (transform.position != startPoint)
                    {
                        navmesh.stoppingDistance = 1;
                        navmesh.SetDestination(startPoint);

                        if (navmesh.remainingDistance <= 1)
                        {
                            animator.SetBool("walk", false);
                        }

                    }
                    close = false;
                }

            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, closeDistance);
    }

    private void Update()
    {
    }



}
