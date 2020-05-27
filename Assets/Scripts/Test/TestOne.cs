using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOne : MonoBehaviour
{
    [SerializeField] Vector2 raycastDirection = new Vector2(0, 0);
    [SerializeField] float raycastLength = 1.0f;
    [SerializeField] Vector2 raycastOriginDistance = new Vector2(0, 0);
    [SerializeField] bool displayRayLine = false;
    [SerializeField] Vector2 directionSwap;
    public float speed = 1.0f;
    public Vector3 exampleTarget;

    void Start()
    {
        
    }

    void Update()
    {
        //Movement();
        //FireRaycast();
        //FireManyRayCasts();
        //FindMagnitude();
        //Swap();
        StartCoroutine(MovementWithMoveToward());
    }

    IEnumerator MovementWithMoveToward()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            while(Vector3.Distance(transform.position, exampleTarget) > speed)
            {
                Debug.LogFormat("distance = {0}", Vector3.Distance(transform.position, exampleTarget));
                transform.position = Vector3.MoveTowards(transform.position, exampleTarget, speed);
                yield return new WaitForSeconds(.5f);
            }
            if (Vector3.Distance(transform.position, exampleTarget) < speed)
            {
                Debug.LogFormat("Done");
                transform.position = exampleTarget;
            }
        }
        
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 position = transform.position;

        position.x += .1f * horizontal;
        position.y += .1f * vertical;
        transform.position = position;
    }

    void FireRaycast()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            List<GameObject> gameobjectHitByRaycast = new List<GameObject>();
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + raycastOriginDistance.x, transform.position.y + raycastOriginDistance.y), raycastDirection, raycastLength);
            if (displayRayLine)
                Debug.DrawRay(transform.position, raycastDirection, Color.white, raycastLength);


            if (hit.collider != null)
                gameobjectHitByRaycast.Add(hit.collider.gameObject);
            if (gameobjectHitByRaycast.Count > 0)
            {
                foreach (var a in gameobjectHitByRaycast)
                    Debug.LogFormat("gameobjectHitByRaycast memberlist: {0}", a);
            }
        }
        
    }

    void FireManyRayCasts()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            List<GameObject> gameobjectHitByRaycast = new List<GameObject>();
            RaycastHit2D[] manyHits = Physics2D.RaycastAll(new Vector2(transform.position.x + raycastOriginDistance.x, transform.position.y + raycastOriginDistance.y), raycastDirection, raycastLength);
            //Debug.DrawRay(transform.position, raycastDirection, Color.white, raycastLength);
            if (displayRayLine)
                Debug.DrawLine(transform.position, raycastDirection * raycastLength);
            if (manyHits.Length > 0)
            {
                for (int i = 0; i < manyHits.Length; i++)
                {
                    if (manyHits[i].collider != null)
                        gameobjectHitByRaycast.Add(manyHits[i].collider.gameObject);
                }
            }
            foreach(var a in gameobjectHitByRaycast)
                Debug.LogFormat("gameobjectHitByRaycastAll memberlist: {0}", a);
        }
    }

    void FindMagnitude()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.LogFormat("Magnitude = {0}", transform.position.magnitude);
        }
    }

    void Swap()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Moving(gameObject, directionSwap));
        }
    }

    IEnumerator Moving(GameObject go, Vector2 direction)
    {
        Vector2 finalPosition = new Vector2(go.transform.position.x + direction.x, go.transform.position.y + direction.y);
        Debug.LogFormat("Distance = {0}, final = {1}", Vector2.Distance(go.transform.position, finalPosition), finalPosition);

        for (int i = 0; i < 10; i++)
        {
            Vector2 currentPosition = go.transform.position;
            currentPosition = currentPosition + direction / 10;
            go.transform.position = currentPosition;
            Debug.LogFormat("Distance = {0}", Vector2.Distance(go.transform.position, finalPosition));
            yield return new WaitForSeconds(.5f);
        }

        //while (Vector2.Distance(go.transform.position, finalPosition) > 0)
        //{

        //}
        go.transform.position = finalPosition;
    }
}
