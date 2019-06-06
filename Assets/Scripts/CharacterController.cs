using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpPower;
    [Range(0, 1)]
    public float jumpMultPerExtraJump = .75f;
    public float upwardsGravity = 9;
    public float downwardsGravity = 18;
    public float terminalVelocity;

    public float minJumpTime = .3f;
    public float jumpCooldown = .15f;

    public ParticleSystem respawnParticles;
    public ParticleSystem playerDeathParticles;
    public AudioSource playerDeathSoundPrefab;

    public Transform[] edgesRight;
    public Transform[] edgesLeft;
    public Transform[] edgesTop;
    public Transform[] edgesBottom;

    public int maxJumps = 1;

    public float respawnTime;

    private int currentJumps = 1;

    private Rigidbody2D _rb2d;

    public Vector2 velocity;
    bool grounded = false;

    float currentJumpTime;
    float targetGravity;
    bool jumping;
    bool justStomped;

    private List<KeyData> acquiredKeys = new List<KeyData>();

    private RespawnPoint lastRespawn;
    private Vector3 lastRespawnPos;

    private bool locked = false;
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

    public void Lock(bool locked = true)
    {
        this.locked = locked;
    }

    void FixedUpdate()
    {
        if (locked) return;
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
            velocity.y = Mathf.Clamp(velocity.y + targetGravity * Time.fixedDeltaTime, -terminalVelocity, terminalVelocity);
        }

        if (Input.GetAxis("Jump") > Mathf.Epsilon)
        {
            if (jumping == false || grounded)
            {
                if (currentJumps > 0)
                {
                    jumping = true;
                    grounded = false;
                    velocity.y = jumpPower * Mathf.Pow(jumpMultPerExtraJump, maxJumps - currentJumps);
                    currentJumpTime = 0;
                    targetGravity = upwardsGravity;
                    currentJumps--;
                }
            }
            if (velocity.y < 0)
            {
                targetGravity = downwardsGravity;
            }
        }
        else
        {
            jumping = false;
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
                            currentJumps = maxJumps;
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

        Destroy(Instantiate(playerDeathSoundPrefab), playerDeathSoundPrefab.clip.length);

        Destroy(Instantiate(playerDeathParticles, transform.position, Quaternion.identity).gameObject, respawnTime);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Lock();
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        transform.position = lastRespawnPos;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        Lock(false);
    }



    public void SetRespawnPoint(RespawnPoint point)
    {
        if (Vector3.Distance(point.transform.position, lastRespawnPos) <= 1) return;
        if (lastRespawn == null)
        {
            respawnParticles = Instantiate(respawnParticles, point.transform.position, Quaternion.identity);
        }
        respawnParticles.Stop(true);
        respawnParticles.transform.position = point.transform.position;
        respawnParticles.Play(true);
        lastRespawn = point;
        lastRespawnPos = point.transform.position;
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
