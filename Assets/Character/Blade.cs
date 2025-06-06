using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public float prepAngle = -60f;
    public float strikeAngle = 90f;
    public float swingSpeed = 4f;
    public KeyCode attackKey = KeyCode.Mouse0;

    public bool isDamaging { get; private set; } = false;

    private Quaternion initialRotation;
    private float swingTimer = 0f;
    private bool isSwinging = false;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isSwinging)
        {
            isSwinging = true;
            swingTimer = 0f;
        }

        if (isSwinging)
        {
            swingTimer += Time.deltaTime * swingSpeed;

            float angle;
            if (swingTimer < 0.5f)
            {
                float t = swingTimer / 0.5f;
                angle = Mathf.Lerp(0f, prepAngle, t);
                isDamaging = false;
            }
            else if (swingTimer < 1f)
            {
                float t = (swingTimer - 0.5f) / 0.5f;
                angle = Mathf.Lerp(prepAngle, strikeAngle, t);
                isDamaging = true; // zasahuje jen pøi úderu
            }
            else
            {
                transform.localRotation = initialRotation;
                isSwinging = false;
                isDamaging = false;
                return;
            }

            transform.localRotation = initialRotation * Quaternion.Euler(angle, 0f, 0f);
        }
    }
}
