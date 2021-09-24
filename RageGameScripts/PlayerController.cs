using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script that handles the player controls.
public class PlayerController : MonoBehaviour
{

    GameManager gameManager;
    AchievementManager achievementManager;
    Rigidbody2D rb;
    Vector2 dist;
    bool onGround;
    bool canCount;
    float internalCooldown;
    Vector2 lastPos;
    Vector2 mousePos;
    float algidPoint;
    LineRenderer lineRenderer;
    bool easyMode;
    [Header("Ground Check Settings")]
    public Transform groundCheckPos;
    public float distance;
    public float radius;
    public LayerMask layerMask;
    [Header("Force Settings")]
    public float maxValue;
    public float minValue;
    public float force;
    public float onAirMovement;
    [Header("Booster Settings")]
    public string boosterLayer;
    public float boosterForce;
    [Header("Particles")]
    public ParticleSystem jump;
    public ParticleSystem bump;
    [Header ("Animator")]
    public Animator animator;
    public bool charging;
    public bool release;
    [Header("Audio")]
    public AudioSource jumpSound;
    public AudioSource bumpSound;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        achievementManager = FindObjectOfType<AchievementManager>();
    }
    
    void Start(){
        internalCooldown = 2f;
        lineRenderer.positionCount = 2;
        easyMode = false;
    }
    
    void Update(){
        GroundCheck();
        UpdateMousePosition();
        Controls();
        AnimatorUpdate();
        CheckPosition();
        CalculateDistance();
        
    }

    /// <summary>
    /// Calculate jumped & fallen distance to add it to the stats.
    /// </summary>
    void CalculateDistance(){
        if(rb.velocity.y > 0f){
            algidPoint = transform.position.y;
            canCount = true;
        }if(onGround && canCount && rb.velocity.y == 0f){
            float climbed = algidPoint - lastPos.y;
            float fallen = lastPos.y - transform.position.y;
            gameManager.AddDistanceClimbed(climbed);
            achievementManager.GetAchievement(Achievement.fall, (int)fallen);
            canCount = false;
        }
    }

    /// <summary>
    /// Returns player to last position if stucked on air.
    /// </summary>
    void CheckPosition(){
        if(rb.velocity == Vector2.zero && !onGround){
            internalCooldown -= Time.deltaTime;
            if(internalCooldown <= 0f) Return();
        }else internalCooldown = 2f;
    }
    
    /// <summary>
    /// Updates animator values.
    /// </summary>
    void AnimatorUpdate(){
        animator.SetBool("Charging", charging);
        animator.SetBool("Release", release);
    }
    
    /// <summary>
    /// Method containing the player's controls
    /// </summary>
    void Controls(){
        if(Input.GetKey(KeyCode.Mouse0) && onGround) DrawLine(mousePos);
        if(Input.GetKeyUp(KeyCode.Mouse0) && onGround){
            Shoot(dist);
            jump.Play();
            jumpSound.Play();
            DestroyLine();
        }
        //Controls for Easy Mode
        if(easyMode){
            if(!onGround) MoveOnAir();
            if(Input.GetKeyDown(KeyCode.R)) Return();
            if(rb.velocity.y < 0f) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.01f);
        }
    }
    /// <summary>
    /// Makes player able to move while on air.
    /// </summary>
    void MoveOnAir(){
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(-onAirMovement * Time.deltaTime, 0f, 0f, Space.Self);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.Translate(onAirMovement * Time.deltaTime, 0f, 0f, Space.Self);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    /// <summary>
    /// Updates the mouse position converting from screen to world point.
    /// </summary>
    void UpdateMousePosition(){
        mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        float distX = Mathf.Clamp(mousePos.x - transform.position.x, minValue, maxValue);
        float distY = Mathf.Clamp(mousePos.y - transform.position.y, minValue, maxValue);

        dist = new Vector2(-distX, -distY);
    }
    /// <summary>
    /// Draws a line between the mouse position and the player.
    /// </summary>
    /// <param name="endPoint"> The coordinates where the line will end.
    void DrawLine(Vector2 endPoint){
        charging = true;
        release = false;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    void DestroyLine(){
        charging = false;
        release = true;
        lineRenderer.enabled = false;
    }

    void GroundCheck(){
        onGround = Physics2D.OverlapCircle(groundCheckPos.position, radius, layerMask);
    }
    /// <summary>
    /// Shoots player at the direction the mouse is.
    /// </summary>
    /// <param name="shootPos"> The coordinates in Vector2 where the player will jump at.
    void Shoot(Vector2 shootPos){
        gameManager.AddJump();
        lastPos = transform.position;
        rb.AddForce(shootPos * force, ForceMode2D.Impulse);
    }
    /// <summary>
    /// Makes player return to the last position.
    /// </summary>
    void Return(){
        rb.velocity = Vector2.zero;
        transform.position = lastPos;
    }

    void OnCollisionEnter2D(Collision2D collision){
        //Plays the jump particles.
        if(onGround) jump.Play();
        //Plays the bump particles
        else {
            bump.startSpeed = rb.velocity.x;
            bump.Play();
            gameManager.AddBump();
        }
        bumpSound.Play();
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.layer == LayerMask.NameToLayer(boosterLayer)){
            switch(collider.gameObject.tag){
                //If player goes through the boost object, shoots them.
                case "Force":
                    if(rb.velocity.y > 0f) Shoot(Vector2.up * boosterForce);
                    break;
            }
        }
        //If player goes through the end gate, ends the game.
        if(collider.gameObject.tag == "End") gameManager.EndGame();
    }

    public void ToggleEasyMode(){
        easyMode = !easyMode;
    }
}
