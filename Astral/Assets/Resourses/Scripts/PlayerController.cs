using UnityEngine;
using System;

public class PlayerController : Character
{
    #region Variables

    [Header("Física do jogador")]
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private int totalJump = 1;
    [SerializeField] private int gravitySpeedIncrement = 10;
    private int JumpCount;
    //[SerializeField] private float jumpSpeedDecrement = 10;

    [Header("Dados do jogador")]
    [HideInInspector] private bool Alive = true;
    [SerializeField] private Collider2D Collider;
    [SerializeField] private int life = 1;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform footPosition;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float floorDistance = 0.2f;
    [SerializeField] private float wallDistance = 0.2f;
    private int gravityOfMoveX = 0;
    private int gravityOfMoveY = 0;
    private int InitialLife;
    public static PlayerController PlayerInstance;


    [Header("Informações adicionais")]
    [SerializeField] private int currentStage = 1;
    public Transform spawnPointPosition;
    public float ghostSpeed;

    #endregion

    #region Base Methods

    protected override void Awake()
    {
        base.Awake();
        PlayerInstance = this;
        InitialLife = life;
        JumpCount = totalJump;
        InitialLife = life;
        Animator = GetComponent<Animator>();
        SpawnBody();
    }

    protected override void Update()
    {
        base.Update();
        RestoreJump();
        Flip();
        if (!Alive)
        {
            GhostMode();
        }
    }

    #endregion

    #region Inputs Controller


    public void InputActions(float moveX, float moveY)
    {
        if (!Alive) return;
        if (moveX != 0 || moveY != 0)
        {
            PlayerMove(moveX, moveY);
            Animator.SetBool("Running", true);
        }
        else
        {
            Animator.SetBool("Running", false);
        }
        if (moveY > 0)
        {
            Jump(jumpHeight);
        }
    }

    #endregion

    #region Movimentation Controller

    void PlayerMove(float MoveX, float MoveY)
    {
        if (!GravityOn)
        {
            GravityOffMove();
            if (MoveX == 0 && MoveY == 0 || !InWall()) return;
            SetGravityOffDirection(MoveX, MoveY);
        }
        else
        {
            Move(MoveX, MoveY);
        }
    }

    void RotatePlayer(int _rotationValue, int RotateIndex = 0)
    {
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = _rotationValue * RotateIndex;
        transform.rotation = Quaternion.Euler(rotationVector);
    }

    protected override void Flip()
    {
        if (Alive == false) return;
        if ((transform.localScale.x > 0 && Body.velocity.x < 0) || (transform.localScale.x < 0 && Body.velocity.x > 0))
        {
            base.Flip();
        }
    }

    #endregion

    #region Gravity Controller

    void SetGravityOffDirection(float Movex, float MoveY)
    {
        gravityOfMoveX = (int)Movex;
        gravityOfMoveY = (int)MoveY;
    }

    void GravityOffMove()
    {
        if (Body.velocity.x != 0 && Body.velocity.y != 0)
        {
            Body.velocity = new Vector2(Body.velocity.x, 0);
        }


        Body.velocity = new Vector2(gravityOfMoveX, gravityOfMoveY) * moveSpeed * gravitySpeedIncrement;
        if (gravityOfMoveX != 0 && gravityOfMoveY != 0)
        {
            gravityOfMoveY = 0;
        }
        if (gravityOfMoveX != 0)
        {
            RotatePlayer(90, (int)gravityOfMoveX);
        }
        else if (gravityOfMoveY != 0)
        {
            int _index = gravityOfMoveY > 0 ? (int)gravityOfMoveY : 0;
            RotatePlayer(180, _index);
        }

    }

    public void SetGravityStatus(bool status, bool playAudio = true)
    {
        if (GravityOn && !InFloor() && Alive) return;
        GravityOn = status;
        GravityControll(GravityOn, playAudio);
        ItensController.itenInstance.SetItemStatus("Gravity", !GravityOn);
        RotatePlayer(0);
    }

    public void GravityControll(bool isOn, bool playAudio)
    {
        if (isOn)
        {
            Body.gravityScale = this.gravity;
        }
        else
        {
            Body.velocity = new Vector2(0, 0);
            Body.gravityScale = 0;
            if (playAudio) AudioController.AudioControllerInstance.PlayAudio("playerGravity");
        }
    }

    public bool GetGravityStatus()
    {
        return GravityOn;
    }

    #endregion

    #region Jump

    public void Jump(float _height)
    {
        if (this.GravityOn && JumpCount > 0 && InFloor())
        {
            Body.velocity = new Vector2(Body.velocity.x, 0);
            Body.velocity = new Vector2(Body.velocity.x, _height);
            JumpCount--;
            AudioController.AudioControllerInstance.PlayAudio("playerJump");
        }
    }

    public void RestoreJump()
    {
        if (InFloor() && JumpCount < totalJump)
        {
            JumpCount = totalJump;
        }
    }

    #endregion

    #region life controller

    public void TakeDamage()
    {
        life--;
        if (life <= 0)
        {
            Dead();
        }
    }

    private bool InFloor()
    {
        return Physics2D.OverlapCircle(footPosition.position, floorDistance, floorLayer);
    }

    private bool InWall()
    {
        return Physics2D.OverlapCircle(footPosition.position, wallDistance, wallLayer);
    }

    public void Dead()
    {
        Alive = false;
        Animator.SetBool("Dead", true);
        SetGravityStatus(false, false);
        RotatePlayer(0);
        AudioController.AudioControllerInstance.PlayAudio("playerDead");
        ItensController.itenInstance.SetItemStatus("Gravity", !GravityOn);
        Collider.enabled = false;
    }

    public bool GetIsAlive()
    {
        return Alive;
    }

    public void GhostMode()
    {

        transform.position = Vector2.MoveTowards(transform.position, spawnPointPosition.position, ghostSpeed * setGhostVelocityIncrement() * Time.deltaTime);
        SetGhostDirection();
        if (transform.position == spawnPointPosition.position) SpawnBody();
    }

    public float setGhostVelocityIncrement()
    {
        float incremetController = 10;
        int minIncrement = 1;
        //int maxIncrement = 4;

        float _distance = Math.Abs(transform.position.x - spawnPoint.transform.position.x);

        //float _incrementedVelocity = _distance/incremetController > maxIncrement ? maxIncrement : _distance/incremetController > minIncrement ? _distance/incremetController : minIncrement;

        float _incrementedVelocity = Math.Max(_distance / incremetController, Math.Max(_distance / incremetController, minIncrement));



        return _incrementedVelocity;
    }

    public void SetGhostDirection()
    {
        float scale = Math.Abs(transform.localScale.x);
        if (transform.position.x > spawnPoint.position.x)
        {
            base.SetFlip(scale);
        }
        else if (transform.position.x < spawnPoint.position.x)
        {
            base.SetFlip(-scale);
        }
    }

    protected void SpawnBody()
    {
        Collider.enabled = true;
        SetGravityStatus(true, false);
        Animator.SetBool("Dead", false);
        Body.gravityScale = this.gravity;
        life = InitialLife;
        if (!Alive) Alive = true;
        Body.velocity = new Vector2(0, 0);
        Body.position = spawnPoint.position;
    }

    #endregion

    #region Trigger Controller

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
        else if (other.CompareTag("Goal"))
        {   
            
            PhasesCache.PhasesCacheInstance.SetPhaseCompleted(currentStage);
            int _nextStage = currentStage + 1;
            GameController.ControllerInstance.LoadScennes("Phase_" + _nextStage);
        }
    }

    #endregion
}


// using UnityEngine;
// using System;

// public class PlayerController : Character
// {
//     #region Variables

//     [Header ("Física do jogador")]
//     [SerializeField] private float jumpHeight = 15;
//     [SerializeField] private int totalJump = 1;
//     [SerializeField] private int gravitySpeedIncrement = 10;
//     private int JumpCount;
//     //[SerializeField] private float jumpSpeedDecrement = 10;

//     [Header ("Dados do jogador")]
//     [HideInInspector] private bool Alive = true;
//     [SerializeField] private Collider2D Collider;
//     [SerializeField] private int life = 1;
//     [SerializeField] private Transform spawnPoint;
//     [SerializeField] private Transform footPosition;
//     [SerializeField] private LayerMask floorLayer;
//     [SerializeField] private LayerMask wallLayer;
//     [SerializeField] private float floorDistance = 0.2f;
//     [SerializeField] private float wallDistance = 0.2f;
//     private int InitialLife;
//     public static PlayerController PlayerInstance;


//     [Header ("Informações adicionais")]
//     [SerializeField] private int currentStage = 1;
//     public Transform spawnPointPosition;
//     public float ghostSpeed;

//     #endregion

//     #region Base Methods

//     protected override void Awake(){
//         base.Awake();
//         PlayerInstance = this;
//         InitialLife = life;
//         JumpCount = totalJump;
//         InitialLife = life;
//         Animator = GetComponent<Animator> ();
//         SpawnBody();
//     }

//     protected override void Update()
//     {
//         base.Update();
//         RestoreJump();
//         Flip();
//         if(!Alive){
//             GhostMode();
//         }
//     }

//     #endregion

//     #region Inputs Controller


//     public void InputActions(float moveX, float moveY){
//         if(!Alive) return;
//         if(moveX!=0 || moveY!=0){
//             PlayerMove(moveX, moveY);
//             Animator.SetBool ("Running", true);
//         }else{
//             Animator.SetBool ("Running", false);
//         }
//         if(moveY>0){
//             Jump(jumpHeight);
//         }
//     }

//     #endregion

//     #region Movimentation Controller

//     void PlayerMove(float MoveX, float MoveY){
//         if(!GravityOn){
//             GravityOffMove(MoveX, MoveY);
//         }else{
//             Move(MoveX, MoveY);
//         }
//     }

//     void RotatePlayer(int _rotationValue, int RotateIndex = 0){
//         var rotationVector = transform.rotation.eulerAngles;
//         rotationVector.z = _rotationValue * RotateIndex;
//         transform.rotation = Quaternion.Euler(rotationVector);
//     }

//     protected override void Flip(){
//         if(Alive == false) return;
//         if((transform.localScale.x>0 && Body.velocity.x<0) || (transform.localScale.x<0 && Body.velocity.x>0)){
//             base.Flip();
//         }
//     }

//     #endregion

//     #region Gravity Controller

//     void GravityOffMove(float MoveX, float MoveY){
//         if(Body.velocity.x != 0 && Body.velocity.y != 0){
//             Body.velocity = new Vector2 (Body.velocity.x,0);
//        }

//        if(!InWall() || MoveX == 0 && MoveY ==0) return;




//             Body.velocity = new Vector2 (MoveX, MoveY) * moveSpeed * gravitySpeedIncrement;
//             if(MoveX!=0 && MoveY!=0){
//                 MoveY =0;
//             }
//              if(MoveX != 0){
//                  RotatePlayer(90, (int)MoveX);
//              }else if(MoveY != 0){
//                  int _index = MoveY > 0 ? (int) MoveY : 0;
//                  RotatePlayer(180, _index);
//              }

//     }

//     public void SetGravityStatus(bool status, bool playAudio = true){
//         if(GravityOn && !InFloor() && Alive) return;
//         GravityOn = status;
//         GravityControll(GravityOn, playAudio);
//         ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
//         RotatePlayer(0);
//     }

//     public void GravityControll(bool isOn, bool playAudio){
//         if(isOn){
//             Body.gravityScale = this.gravity;
//         }else{
//             Body.velocity = new Vector2(0,0);
//             Body.gravityScale = 0;
//             if(playAudio) AudioController.AudioControllerInstance.PlayAudio("playerGravity");
//         }
//     }

//     public bool GetGravityStatus(){
//         return GravityOn;
//     }

//     #endregion

//     #region Jump

//     public void Jump(float _height){
//         if(this.GravityOn && JumpCount>0 && InFloor()){
//             Body.velocity = new Vector2(Body.velocity.x, 0);
//             Body.velocity = new Vector2(Body.velocity.x, _height);
//             JumpCount--;
//             AudioController.AudioControllerInstance.PlayAudio("playerJump");
//         }
//     }

//     public void RestoreJump(){
//         if(InFloor() && JumpCount < totalJump){
//             JumpCount = totalJump;
//         }
//     }

//     #endregion

//     #region life controller

//     public void TakeDamage(){
//         life--;
//         if(life <= 0){
//             Dead();
//         }
//     }

//     private bool InFloor(){
//         return Physics2D.OverlapCircle(footPosition.position, floorDistance, floorLayer);
//     }

//     private bool InWall(){
//         return Physics2D.OverlapCircle(footPosition.position, wallDistance, wallLayer);
//     }

//     public void Dead(){
//         Alive = false;
//         Animator.SetBool ("Dead", true);
//         SetGravityStatus(false, false);
//         RotatePlayer(0);
//         AudioController.AudioControllerInstance.PlayAudio("playerDead");
//         ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
//         Collider.enabled = false;
//     }

//     public bool GetIsAlive(){
//         return Alive;
//     }

//     public void GhostMode(){

//         transform.position = Vector2.MoveTowards(transform.position, spawnPointPosition.position, ghostSpeed*setGhostVelocityIncrement()*Time.deltaTime);
//         SetGhostDirection();
//         if(transform.position == spawnPointPosition.position) SpawnBody();
//     }

//     public float setGhostVelocityIncrement(){
//         float incremetController = 10;
//         int minIncrement = 1;
//         //int maxIncrement = 4;

//         float _distance = Math.Abs(transform.position.x - spawnPoint.transform.position.x);

//         //float _incrementedVelocity = _distance/incremetController > maxIncrement ? maxIncrement : _distance/incremetController > minIncrement ? _distance/incremetController : minIncrement;

//         float _incrementedVelocity= Math.Max(_distance/incremetController,Math.Max(_distance/incremetController,minIncrement));



//         return _incrementedVelocity;
//     }

//     public void SetGhostDirection(){
//         float scale = Math.Abs(transform.localScale.x);
//         if(transform.position.x > spawnPoint.position.x){
//             base.SetFlip(scale);
//         }else if (transform.position.x < spawnPoint.position.x){
//             base.SetFlip(-scale);
//         }
//     }

//     protected void SpawnBody(){
//         Collider.enabled = true;
//         SetGravityStatus(true, false);
//         Animator.SetBool ("Dead", false);
//         Body.gravityScale = this.gravity;
//         life = InitialLife;
//         if(!Alive)Alive = true;
//         Body.velocity = new Vector2(0,0);
//         Body.position = spawnPoint.position;
//     }

//     #endregion

//     #region Trigger Controller

//     void OnTriggerEnter2D(Collider2D other){
//         if(other.CompareTag("Obstacle") || other.CompareTag("Enemy")){
//             TakeDamage();
//         }else if(other.CompareTag("Goal")){
//             PhasesCache.PhasesCacheInstance.SetPhaseCompleted(currentStage);
//             int _nextStage = currentStage+1;
//             GameController.ControllerInstance.LoadScennes("Phase_"+_nextStage);
//         }
//     }

//     #endregion
// }