using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ExperimentalCharacterController : MonoBehaviour
{

    public float speed = 5;
    public float jumpPower = 10;
    public float horizontalDrag = .1f;
    public int maxJumps = 1;

    public float jumpingGravityScale;
    public float fallingGravityScale;

    public float minJumpTime = .3f;


    private Rigidbody2D _rb2D;
    private int _remainingJumps;
    private float _horizontalSpeed;
    private bool _grounded = false;
    private bool _jumping = false;
    private float _currentJumpTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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


        if (_remainingJumps > 0 && _jumpTap)
        {
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool floorContact = false;
        foreach (var contactPoint in collision.contacts)
        {
            float dot = Vector2.Dot(-contactPoint.normal, Vector2.up);
            if (dot < -.5f) floorContact = true;
        }
        if (floorContact)
        {
            _grounded = true;
            _remainingJumps = maxJumps;
        }
    }
}
