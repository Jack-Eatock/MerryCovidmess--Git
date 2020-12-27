using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [SerializeField] private float Speed = 5;
    [SerializeField] private float SprintModifier = 1.5f;

    private Rigidbody _rig;
    Vector3 _inputs = Vector3.zero;
    private float cameraDif;

    private bool _isSprinting = false;


    // Start is called before the first frame update
    void Start()
    {
        cameraDif = Camera.main.transform.position.y - transform.transform.position.y;
        _rig = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) {  if (!_isSprinting) { _isSprinting = true; Speed = Speed * SprintModifier; }  }
        else { if (_isSprinting) { _isSprinting = false; Speed = Speed / SprintModifier; } }


        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxisRaw("Horizontal");
        _inputs.z = Input.GetAxisRaw("Vertical");

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDif));
        Vector3 lookDirection = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        transform.LookAt(lookDirection);

    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(_inputs.x, 0f, _inputs.z).normalized * Time.fixedDeltaTime * Speed; 
        _rig.velocity = new Vector3(move.x, _rig.velocity.y, move.z);
    }
}
