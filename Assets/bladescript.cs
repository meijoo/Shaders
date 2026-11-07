using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class bladescript : MonoBehaviour
{
    public GameObject blade;
    private Material bladeMaterial;

    [SerializeField, Min(0.001f)] private float extendSpeed = 0.1f;
    private bool weaponActive;
    private const float SCALE_MIN = 0f;
    private float scaleMax;
    private float extendDelta;
    private float scaleCurrent;
    private float localScaleX;
    private float localScaleZ;

    void Start()
    {
        localScaleX = transform.localScale.x;
        localScaleZ = transform.localScale.z;
        scaleMax = transform.localScale.y;
        scaleCurrent = scaleMax;
        extendDelta = scaleMax / Mathf.Max(extendSpeed, 0.001f);
        weaponActive = true;

        if (blade != null)
        {
            bladeMaterial = blade.GetComponent<Renderer>().material;
        }
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
#endif

        if (spacePressed)
        {
            extendDelta = weaponActive ? -Mathf.Abs(extendDelta) : Mathf.Abs(extendDelta);
        }

        scaleCurrent += extendDelta * Time.deltaTime;
        scaleCurrent = Mathf.Clamp(scaleCurrent, SCALE_MIN, scaleMax);

        transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);
        weaponActive = scaleCurrent > 0f;

        if (blade != null)
        {
            if (weaponActive && !blade.activeSelf)
            {
                blade.SetActive(true);
            }
            else if (!weaponActive && blade.activeSelf)
            {
                blade.SetActive(false);
            }
        }

        if (bladeMaterial != null)
        {
            Color rainbow = Color.HSVToRGB(Mathf.Repeat(Time.time * 0.2f, 1f), 1f, 1f);
            if (bladeMaterial.HasProperty("_BladeColor"))
                bladeMaterial.SetColor("_BladeColor", rainbow);
            else
                bladeMaterial.color = rainbow;
        }
    }
}
