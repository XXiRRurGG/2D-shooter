using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public EnemyController2D controller;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public Enemy enemyData;
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private float currentDirection = 0f;
    private bool jump;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

                if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                if (jumpEnabled)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jump = true;
                            }
        }
        if(direction.x != 0)
        {
            currentDirection = Mathf.Sign(direction.x) * 1;
            Debug.Log("Direction:"+currentDirection);
        }
                controller.Move(speed*currentDirection*Time.fixedDeltaTime,false,jump);
        jump = false;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}