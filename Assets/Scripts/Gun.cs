using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public InputActionReference shoot;
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    //gun sway
    public float MoveAmount = 1f;
    public float MoveSpeed = 2f;
    public GameObject GUN;
    public float MoveOnX;
    public float MoveOnY;
    public Vector3 DefaultPos;
    public Vector3 NewGunPos;

    void Start()
    {
        //gun sway
        DefaultPos = transform.localPosition;
    }

    void Update()
    {
        //gun sway
        MoveOnX = Input.GetAxis("Mouse X") * Time.deltaTime * MoveAmount;
        MoveOnY = Input.GetAxis("Mouse Y") * Time.deltaTime * MoveAmount;
        NewGunPos = new Vector3(DefaultPos.x + MoveOnX, DefaultPos.y + MoveOnY, DefaultPos.z);
        GUN.transform.localPosition = Vector3.Lerp(GUN.transform.localPosition, NewGunPos, MoveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        if (shoot != null && shoot.action != null)
        {
            shoot.action.Enable();
            shoot.action.started += Shoot;
        }
    }
    private void OnDisable()
    {
        if (shoot != null && shoot.action != null)
            shoot.action.started -= Shoot;

        if (shoot != null && shoot.action != null)
            shoot.action.Disable();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        //particle effect
        muzzleFlash.Play();
        //actual shot
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            //force impact tp rigidbodies
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //partile effect on impact
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
        Debug.Log("Shot");
    }
}
