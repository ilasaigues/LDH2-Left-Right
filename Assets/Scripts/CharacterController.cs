using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{

    public float speed = 5;
    public float jumpPower = 10;
    public float horizontalDrag = .1f;
    public int maxJumps = 1;

    public float jumpingGravityScale;
    public float fallingGravityScale;

    public float minJumpTime = .3f;
    public float respawnTime = 2;

    public ParticleSystemReference deathParticles;
    public ParticleSystem playerTrail;
    public SoundValue deathSound;

    private Rigidbody2D _rb2D;
    private int _remainingJumps;
    private float _horizontalSpeed;
    private bool _grounded = false;
    private bool _jumping = false;
    private float _currentJumpTime = 0;
    private bool _locked;
    private List<KeyData> _acquiredKeys = new List<KeyData>();
    bool _jumpedThisFrame = false;

    public void AddKey(KeyData key)
    {
        if (!_acquiredKeys.Contains(key)) _acquiredKeys.Add(key);
    }
    public bool CanOpenDoor(Door door)
    {
        return _acquiredKeys.Contains(door.key);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (_locked) return;
        _jumpedThisFrame = false;

        Vector2 _velocity = _rb2D.velocity;
        bool _jumpTap = false;

        if (Input.GetAxisRaw("Jump") != 0)
        {
            if (_jumping == false) _jumpTap = _jumping = true;
        }
        else
        {
            _jumping = false;
            if (_currentJumpTime > minJumpTime || _velocity.y < 0)
            {
                _rb2D.gravityScale = fallingGravityScale;
            }
        }


        if ((_remainingJumps > 0) && _jumpTap)
        {
            _jumpedThisFrame = true;
            _rb2D.gravityScale = jumpingGravityScale;
            _velocity.y = jumpPower;
            _currentJumpTime = 0;
            _remainingJumps--;
            _grounded = false;
        }

        if (_velocity.y < 0)
        {
            _rb2D.gravityScale = fallingGravityScale;
        }

        float targetHorizontalSpeed = Input.GetAxisRaw("Horizontal") * speed;

        if (_velocity.y > 0) _currentJumpTime += Time.fixedDeltaTime;

        _velocity.x = Mathf.Lerp(_velocity.x, targetHorizontalSpeed, horizontalDrag);
        _rb2D.velocity = _velocity;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        ICollidable collidable = collision.collider.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.CollidedWithCharacterController(this);
        }
        if (_jumpedThisFrame) return;
        bool floorContact = false;
        foreach (var contactPoint in collision.contacts)
        {
            if ((contactPoint.normal.y) > Mathf.Abs(contactPoint.normal.x)) floorContact = true;
        }
        if (floorContact)
        {
            _grounded = true;
            _remainingJumps = maxJumps;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollidable collidable = collision.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.CollidedWithCharacterController(this);
        }
    }

    public void Lock(bool setLock = true)
    {
        _locked = setLock;
        _rb2D.simulated = !_locked;
    }

    public void EnemyStomped()
    {
        Vector2 _velocity = _rb2D.velocity;
        _velocity.y = Mathf.Abs(_velocity.y);
        _rb2D.velocity = _velocity;
    }


    public void Kill()
    {
        playerTrail.Stop();
        Director.GetManager<SoundManager>().PlaySound(deathSound);
        Director.GetManager<ParticlesManager>().SpawnParticles(deathParticles.Value, transform.position, respawnTime);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Lock();
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        playerTrail.Play();
        transform.position = Director.GetManager<RespawnManager>().GetRespawnPosition();
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        Lock(false);
    }


}
