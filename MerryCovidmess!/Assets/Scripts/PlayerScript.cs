using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float Speed = 5;
    [SerializeField] private float SprintModifier = 1.5f;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _projectile = null;
    [SerializeField] private Transform _firingPoint = null;
    [SerializeField] private Transform _bulletStorage;

    [SerializeField] private float _fireDelay = 0.01f;

    private float _lastTime = 0f;

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

        // Are we sprinting?
        if (Input.GetKey(KeyCode.LeftShift)) {  if (!_isSprinting) { _isSprinting = true; Speed = Speed * SprintModifier; }  }
        else { if (_isSprinting) { _isSprinting = false; Speed = Speed / SprintModifier; } }

        // Are we firing? 
        if (Input.GetKey(KeyCode.Mouse0))
        { 
            // Has the fire delay finished. 
            if (Time.time - _lastTime > _fireDelay) { _lastTime = Time.time; Fire(); }
            
        }

        // Movement
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxisRaw("Horizontal");
        _inputs.z = Input.GetAxisRaw("Vertical");

        // Aimings
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDif));
        Vector3 lookDirection = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        transform.LookAt(lookDirection);

        _anim.SetFloat("Speed", _rig.velocity.magnitude);

    }

    private void Fire()
    {
        // Ensure there is a projectile set otherwise reuturn
        if (_projectile == null) { Debug.LogError("No projectile Set!"); return; }

        GameObject bullet = GameObject.Instantiate(_projectile);
        bullet.transform.position = _firingPoint.transform.position;
        bullet.transform.SetParent(_bulletStorage);

        bullet.GetComponent<Rigidbody>().AddForce(_firingPoint.transform.forward * 150f);
        //bullet.transform.SetParent(_bulletStorage);
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(_inputs.x, 0f, _inputs.z).normalized * Time.fixedDeltaTime * Speed; 
        _rig.velocity = new Vector3(move.x, _rig.velocity.y, move.z);

    }
}
