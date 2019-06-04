using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpPower;

    public float upwardsGravity = 9;
    public float downwardsGravity = 18;

    public float minJumpTime = .3f;

    public Transform[] edgesRight;
    public Transform[] edgesLeft;
    public Transform[] edgesTop;
    public Transform[] edgesBottom;

    private Rigidbody2D _rb2d;
    private Vector2 velocity;
    bool grounded = false;

    float currentJumpTime;
    float targetGravity;
    bool justStomped;

    private List<KeyData> acquiredKeys = new List<KeyData>();

    private RespawnPoint lastRespawn;
    private Vector3 lastRespawnPos;


    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        targetGravity = downwardsGravity;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    void FixedUpdate()
    {
        HandleInput();
        HandleCollisions();
        _rb2d.MovePosition((Vector2)transform.position + velocity);
    }


    public void AddKey(KeyData key)
    {
        if (!acquiredKeys.Contains(key)) acquiredKeys.Add(key);

    }
    public bool CanOpenDoor(Door door)
    {
        if (acquiredKeys.Count > 0)
        {
            Debug.Log("Door key: " + door.key.name + ", have: " + acquiredKeys[0].name);
        }
        return acquiredKeys.Contains(door.key);
    }

    void HandleInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        velocity.x = horizontalMovement;
        if (grounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += targetGravity * Time.fixedDeltaTime;
        }

        if (Input.GetAxis("Jump") > Mathf.Epsilon)
        {
            if (grounded)
            {
                grounded = false;
                velocity.y = jumpPower;
                currentJumpTime = 0;
                targetGravity = upwardsGravity;
            }
            else if (velocity.y < 0)
            {
                targetGravity = downwardsGravity;
            }
        }
        else
        {
            if (currentJumpTime > minJumpTime || velocity.y < 0)
            {
                targetGravity = downwardsGravity;
            }
        }
        if (velocity.y > 0) currentJumpTime += Time.fixedDeltaTime;
    }

    void HandleCollisions()
    {

        List<RaycastHit2D> horizontalCollisions = new List<RaycastHit2D>();


        if (velocity.x < 0)
        {
            foreach (var edge in edgesLeft)
            {
                horizontalCollisions.Add(Physics2D.Raycast(edge.position, Vector2.left, Mathf.Abs(velocity.x), Layers.MASK_MOVEMENT));
            }
        }
        else if (velocity.x > 0)
        {
            foreach (var edge in edgesRight)
            {
                horizontalCollisions.Add(Physics2D.Raycast(edge.position, Vector2.right, velocity.x, Layers.MASK_MOVEMENT));
            }
        }
        foreach (RaycastHit2D rch in horizontalCollisions)
        {
            if (rch.collider != null)
            {
                if (Mathf.Abs(velocity.x) > rch.distance)
                {
                    velocity.x = rch.distance * Mathf.Sign(velocity.x);
                }
                CheckForCollidable(rch.collider);

            }
        }


        List<RaycastHit2D> verticalCollisions = new List<RaycastHit2D>();
        if (!grounded)
        {
            if (velocity.y <= 0)
            {
                foreach (var edge in edgesBottom)
                {
                    verticalCollisions.Add(Physics2D.Raycast(edge.position, Vector2.down, Mathf.Abs(velocity.y), Layers.MASK_MOVEMENT));
                }
            }
            else if (velocity.y > 0)
            {
                foreach (var edge in edgesTop)
                {
                    verticalCollisions.Add(Physics2D.Raycast(edge.position, Vector2.up, velocity.y, Layers.MASK_FLOOR));
                }
            }

            foreach (RaycastHit2D rch in verticalCollisions)
            {
                if (rch.collider != null)
                {
                    CheckForCollidable(rch.collider);
                    if (!justStomped)
                    {
                        if (velocity.y < 0)
                        {
                            grounded = true;
                        }
                        if (Mathf.Abs(velocity.y) > rch.distance)
                        {
                            velocity.y = rch.distance * Mathf.Sign(velocity.y);
                        }
                    }
                    justStomped = false;
                }
            }
        }
        else
        {
            foreach (var edge in edgesBottom)
            {
                verticalCollisions.Add(Physics2D.Raycast(edge.position, Vector2.down, .1f, Layers.MASK_MOVEMENT));
            }
            int collCount = 0;

            foreach (var coll in verticalCollisions)
            {
                if (coll.collider != null)
                {
                    CheckForCollidable(coll.collider);
                    collCount++;
                }
            }
            if (collCount == 0)
            {
                grounded = false;
            }
        }
    }

    public void EnemyStomped()
    {
        velocity.y = Mathf.Abs(velocity.y);
        grounded = false;
        justStomped = true;
    }


    public void Kill()
    {
        transform.position = lastRespawnPos;
    }


    public void SetRespawnPoint(RespawnPoint point)
    {
        lastRespawn = point;
        lastRespawnPos = point.transform.position;
        Debug.Log("Set respawn point");
    }

    void CheckForCollidable(Collider2D collider)
    {
        if (collider.GetComponent<ICollidable>() != null)
        {
            collider.GetComponent<ICollidable>().CollidedWithCharacterController(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var edge in edgesRight)
        {
            Gizmos.DrawRay(edge.position, Vector3.right * .1f);
        }
        foreach (var edge in edgesLeft)
        {
            Gizmos.DrawRay(edge.position, Vector3.left * .1f);
        }
        foreach (var edge in edgesTop)
        {
            Gizmos.DrawRay(edge.position, Vector3.up * .1f);
        }
        foreach (var edge in edgesBottom)
        {
            Gizmos.DrawRay(edge.position, Vector3.down * .1f);
        }
    }
}
