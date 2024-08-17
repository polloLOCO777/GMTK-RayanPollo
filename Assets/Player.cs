using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Velocidad;
    public float fuerzaSalto;
    public float fuerzaSaltoLava;
    public LayerMask capaSuelo;

    public static Player Instance;

    public bool Mover;

    public GameObject JumpDust;
    public Transform JumpTr;
    public GameObject DustCaminar;

    public float TimeToPart;
    public float TimetoDestroyPart;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator anim;

    public bool SaltarAlPrincipio;

    public SpriteRenderer flip;




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (SaltarAlPrincipio)
        {
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
        Mover = true;

    }

    void Update()
    {
        TimeToPart += Time.deltaTime;
        ProcesarMovimiento();
        ProcesarSalto();
    }


    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider == true;

    }

    void ProcesarSalto()
    {
        if (Mover)
        {
            if (Input.GetKeyDown(KeyCode.Space) && EstaEnSuelo())
            {
                rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
                Instantiate(JumpDust, JumpTr.position, JumpTr.rotation);
            }


            if (EstaEnSuelo())
            {
                anim.SetBool("Jump", false);
                rigidBody.rotation = 0f;
            }
            else
            {
                anim.SetBool("Jump", true);
            }
        }

    }

    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (Mover)
        {
            rigidbody.velocity = new Vector2(inputMovimiento * Velocidad, rigidbody.velocity.y);

            if (inputMovimiento != 0)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }

            if (inputMovimiento != 0 && EstaEnSuelo())
            {
                if (TimeToPart > TimetoDestroyPart)
                {
                    TimeToPart = 0;
                    Instantiate(DustCaminar, JumpTr.position, JumpTr.rotation);
                }
            }

            if (inputMovimiento > 0)
            {
                flip.flipX = false;
            }

            if (inputMovimiento < 0)
            {
                flip.flipX = true;

            }
        }

    }
}
