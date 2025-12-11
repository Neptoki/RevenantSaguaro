using UnityEngine;
using UnityEngine.InputSystem;

public class EldritchBlast : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference ability;

    [Header("Spell Settings")]
    public GameObject blastPrefab;
    public Transform castPoint;
    public float launchForce = 20f;

    [Header("Cooldown Settings")]
    public float cooldownTime = 10f;
    private float cooldownTimer = 0f;

    [Header("Audio")]
    public AudioSource castAudio;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

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
        if (ability.action.IsPressed())
            Debug.Log("Ability input detected!");


        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (IsPressed() && cooldownTimer <= 0f)
        {
            CastBlast();
            cooldownTimer = cooldownTime;
        }

        //gun sway
        MoveOnX = Input.GetAxis("Mouse X") * Time.deltaTime * MoveAmount;
        MoveOnY = Input.GetAxis("Mouse Y") * Time.deltaTime * MoveAmount;
        NewGunPos = new Vector3(DefaultPos.x + MoveOnX, DefaultPos.y + MoveOnY, DefaultPos.z);
        GUN.transform.localPosition = Vector3.Lerp(GUN.transform.localPosition, NewGunPos, MoveSpeed * Time.deltaTime);
    }

    void CastBlast()
    {
        GameObject blast = Instantiate(blastPrefab, castPoint.position, castPoint.rotation);

        Rigidbody rb = blast.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(castPoint.forward * launchForce, ForceMode.Impulse);

        if (castAudio != null)
        {
            castAudio.pitch = Random.Range(minPitch, maxPitch);
            castAudio.Play();
        }

        Debug.Log("Eldritch Blast cast!");
    }

    private bool IsPressed()
    {
        if (ability == null || ability.action == null)
            return false;

        try
        {
            return ability.action.ReadValue<float>() > 0f;
        }
        catch
        {
            return ability.action.phase == InputActionPhase.Started ||
                   ability.action.phase == InputActionPhase.Performed;
        }
    }

    private void OnEnable()
    {
        if (ability != null && ability.action != null)
            ability.action.Enable();
    }

    private void OnDisable()
    {
        if (ability != null && ability.action != null)
            ability.action.Disable();
    }
}