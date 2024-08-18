using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Properties")]

    [Header("Player Movement")]
    [FormerlySerializedAs("Velocidad")]
    [Tooltip("Velocity")]
    [SerializeField] float velocidad;
    [Tooltip("Jump Strength")]
    [SerializeField] float fuerzaSalto;
    [Tooltip("Lava Jump Strength")]
    [SerializeField] float fuerzaSaltoLava;
    [Tooltip("Floor Mask")]
    [SerializeField] LayerMask capaSuelo;

    float particleTimer; // timeToPart
    [FormerlySerializedAs("TimetoDestroyPart")]
    [SerializeField] float timeToCreatePart;
    [FormerlySerializedAs("SaltarAlPrincipio")]
    [Tooltip("Skip To Top")]
    [SerializeField] bool saltarAlPrincipio;

    [Header("Components")]
    [FormerlySerializedAs("flip")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform dustPosition;

    [Header("Prefabs")]
    [SerializeField] GameObject jumpDust;
    [Tooltip("Dust Particles")]
    [SerializeField] GameObject dustCaminar;

    public static Player Instance;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator anim;

    bool canMove;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Time.timeScale = 1;
        
        if (saltarAlPrincipio)
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        canMove = true;
    }

    void Update()
    {
        particleTimer += Time.deltaTime;
        ProcesarMovimiento();
        ProcesarSalto();
    }

    /// <summary>
    ///     Checks if the player is grounded.
    /// </summary>
    /// <returns>
    ///     True if the player is grounded.
    /// </returns>
    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider == true;
    }

    /// <summary>
    ///     <para>
    ///     Performs jump on space. <br/>
    ///     Controls jump animation.
    ///     </para>
    /// </summary>
    void ProcesarSalto()
    {
        if (!canMove)
            return;

        bool isGrounded = EstaEnSuelo();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            Instantiate(jumpDust, dustPosition.position, dustPosition.rotation);
        }

        if (isGrounded)
        {
            anim.SetBool("Jump", false);
            rigidBody.rotation = 0f;
        }
        else
            anim.SetBool("Jump", true);
    }

    /// <summary>
    ///     <para>
    ///         Moves player horizontally.  <br/>
    ///         Creates dust particles.     <br/>
    ///         Controls run animation.     <br/>
    ///         Flips player on x-axis.     <br/>
    ///     </para>
    /// </summary>
    void ProcesarMovimiento()
    {
        if (!canMove)
            return;

        // horizontalInput
        float inputMovimiento = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

        if (inputMovimiento != 0)
            anim.SetBool("Run", true);
        else
            anim.SetBool("Run", false);

        if (inputMovimiento != 0 && EstaEnSuelo())
        {
            if (particleTimer > timeToCreatePart)
            {
                particleTimer = 0;
                Instantiate(dustCaminar, dustPosition.position, dustPosition.rotation);
            }
        }

        // Flip Player
        if (inputMovimiento > 0)
            spriteRenderer.flipX = false;
        else if (inputMovimiento < 0)
            spriteRenderer.flipX = true;
    }
}
