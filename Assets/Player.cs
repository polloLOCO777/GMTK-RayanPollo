using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
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
    [Tooltip("Floor Mask")]
    [SerializeField] LayerMask capaSuelo;

    [FormerlySerializedAs("TimetoDestroyPart")]
    [SerializeField] float timeToCreatePart;
    [FormerlySerializedAs("SaltarAlPrincipio")]
    [Tooltip("Skip To Top")]

    [Header("Components")]
    [FormerlySerializedAs("flip")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Animator animator;

    [Header("Prefabs")]
    [SerializeField] GameObject jumpDust;
    [Tooltip("Dust Particles")]
    [SerializeField] GameObject dustCaminar;

    [Header("Other")]
    [SerializeField] Transform dustPosition;
    float inputMovimiento;
    public static event EventHandler<PlayerActionEventArgs> OnPlayerActionEventHandler;

    public class PlayerActionEventArgs : EventArgs
    {
        public enum ActionType { Step, Jump }

        public readonly ActionType action;
        public PlayerActionEventArgs(ActionType _action)
        {
            action = _action;
        }
    }

    public static Player Instance;

    float particleTimer;
    bool canMove;
    
    private void Awake()
        => Instance = this;

    private void Start()
    {
        Time.timeScale = 1;

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
    ///     Plays jump and animation.
    ///     </para>
    /// </summary>
    void ProcesarSalto()
    {
        if (!canMove)
            return;

        // Jump
        bool isGrounded = EstaEnSuelo();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            Instantiate(jumpDust, dustPosition.position, dustPosition.rotation, HolderManager.Instance.JumpParticleHolder);

            OnPlayerAction(new(PlayerActionEventArgs.ActionType.Jump));
        }

        // Play animations
        if (isGrounded)
        {
            animator.SetBool("Jump", false);
            rigidBody.rotation = 0f;
        }
        else
            animator.SetBool("Jump", true);
    }

    /// <summary>
    ///     <para>
    ///         Moves player.               <br/>
    ///         Creates dust particles.     <br/>
    ///         Controls run animation.     <br/>
    ///         Flips player on x-axis.     <br/>
    ///     </para>
    /// </summary>
    void ProcesarMovimiento()
    {
        if (!canMove)
            return;

        // Move player
        inputMovimiento = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

        // Play animations
        if (inputMovimiento != 0)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

        // Create dust particles
        if (inputMovimiento != 0 && EstaEnSuelo() && particleTimer > timeToCreatePart)
        {
            if (HolderManager.Instance == null)
                throw new NullReferenceException("Each scene should have a ParentManager prefab. ");
            
            particleTimer = 0;
            Instantiate(dustCaminar, dustPosition.position, dustPosition.rotation, HolderManager.Instance.WalkParticleHolder);

            OnPlayerAction(new(PlayerActionEventArgs.ActionType.Step));
        }

        // Flip Player
        if (inputMovimiento > 0)
            spriteRenderer.flipX = false;
        else if (inputMovimiento < 0)
            spriteRenderer.flipX = true;
    }

    /// <summary>
    ///     Informs listeners that the player has performed an action.
    /// </summary>
    void OnPlayerAction(PlayerActionEventArgs e)
        => OnPlayerActionEventHandler?.Invoke(this, e);
}
