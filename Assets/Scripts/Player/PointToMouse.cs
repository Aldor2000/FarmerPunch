using UnityEngine;

public class PointToMouse : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    //moise point support
    Vector2 mousePos;
    public Transform playerTransform;
    public float angle;
    [SerializeField] float angleOffset = 0f;

    [SerializeField]
    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // convert mouse position to world
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // get vector from player to mouse
        Vector2 lookDir = mousePos - (Vector2)playerTransform.position;

        // calculate angle
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - angleOffset;

        // apply rotation to this object (the "fists" parent)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        
    }
}
