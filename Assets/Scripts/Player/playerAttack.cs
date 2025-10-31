using UnityEngine;

public class playerAttack : MonoBehaviour
{


    [SerializeField] private Animator anim;
    

    [Header("Attack Settings")]
    [SerializeField] private float punchCoolDown = 0.4f;
    [SerializeField] private float punchDamage = 50f;
    bool attackRight = false;
    float punchTimer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(punchTimer  > 0f)
        {
            punchTimer -= Time.deltaTime;
        }

        //handle Attack
        HandleAttack();
        
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && punchTimer <= 0f)
        {
            // alternate attack side
            attackRight = !attackRight;

            // set animator parameters
            anim.SetBool("isAttacking", true);
            anim.SetInteger("attackingSide", attackRight ? 1 : 0);

            // restart cooldown
            punchTimer = punchCoolDown;

            // stop attack after a short delay (so the anim can play)
            Invoke(nameof(StopAttack), 0.3f);
        }
    }

    void StopAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){
            other.GetComponent<Enemy>().TakeDamage(punchDamage);
            Debug.Log("enemy hit");
        }
    }


}
