using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveKeyboard : MonoBehaviour
{
    public float speed;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _speedMult = 100.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(_horizontalInput * speed * Time.deltaTime * _speedMult, 0);
    }
}
