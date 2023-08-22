using UnityEngine;
using System;

public class PlayerController : Character
{
    #region Variables

    [Header ("Física do jogador")]
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private int totalJump = 1;
    [SerializeField] private int gravitySpeedIncrement = 10;
    private int JumpCount;
    //[SerializeField] private float jumpSpeedDecrement = 10;

    [Header ("Dados do jogador")]
    [HideInInspector] private bool Alive = true;
    [SerializeField] private Collider2D collider;
    [SerializeField] private int life = 1;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform footPosition;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private float floorDistance = 0.2f;
    private int InitialLife;
    public static PlayerController PlayerInstance;
    

    [Header ("Informações adicionais")]
    [SerializeField] private int currentStage = 1;
    public Transform spawnPointPosition;
    public float ghostSpeed;

    #endregion

    #region Base Methods

    protected override void Awake(){
        base.Awake();
        PlayerInstance = this;
        InitialLife = life;
        JumpCount = totalJump;
        InitialLife = life;
        this.Animator = GetComponent<Animator> ();
        SpawnBody();
    }

    protected override void Update()
    {
        base.Update();
        RestoreJump();
        Flip();
        if(!Alive){
            GhostMode();
        }
    }

    #endregion

    #region Inputs Controller

    
    public void InputActions(float MoveX, float MoveY, bool FPress, bool WPress, bool JumpPress){
        if(!Alive) return;
        if((MoveX!=0 || MoveY!=0)){
            PlayerMove(MoveX, MoveY);
            Animator.SetBool ("Running", true);
        }else{
            Animator.SetBool ("Running", false);
        }
        if(FPress){
            SetGravityStatus(!GravityOn);
        }
        if((WPress || JumpPress)){
            Jump(jumpHeight);
        }
    }

    #endregion

    #region Movimentation Controller

    void PlayerMove(float MoveX, float MoveY){
        if(!GravityOn){
            GravityOffMove(MoveX, MoveY);
        }else{
            Move(MoveX, MoveY);
        }
    }

    void RotatePlayer(int _rotationValue, int RotateIndex = 0){
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = _rotationValue * RotateIndex;
        transform.rotation = Quaternion.Euler(rotationVector);
    }

    protected override void Flip(){
        if(Alive == false) return;
        if((transform.localScale.x>0 && Body.velocity.x<0) || (transform.localScale.x<0 && Body.velocity.x>0)){
            base.Flip();
        }
    }

    #endregion

    #region Gravity Controller

    void GravityOffMove(float MoveX, float MoveY){
        if(Mathf.Approximately(Body.velocity.x,0) && Mathf.Approximately(Body.velocity.y,0)){
            if(MoveX!=0 && MoveY!=0){
                MoveY =0;
            }
            if(MoveX != 0){
                RotatePlayer(90, (int)MoveX);
            }else if(MoveY != 0){
                int _index = MoveY > 0 ? (int) MoveY : 0;
                RotatePlayer(180, _index);
            }
            Body.velocity = new Vector2 (MoveX, MoveY) * moveSpeed * gravitySpeedIncrement;
        }
    }

    public void SetGravityStatus(bool _status , bool playAudio = true){
        if(GravityOn && !InFloor() && Alive) return;
        GravityOn = _status;
        GravityControll(GravityOn, playAudio);
        ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
        RotatePlayer(0);
    }

    public void GravityControll(bool isOn, bool playAudio){
        if(isOn){
            Body.gravityScale = this.gravity;
        }else{
            Body.velocity = new Vector2(0,0);
            Body.gravityScale = 0;
            if(playAudio) AudioController.AudioControllerInstance.PlayAudio("playerGravity");
        }
    }

    #endregion

    #region Jump

    public void Jump(float _height){
        if(this.GravityOn && JumpCount>0 && InFloor()){
            Body.velocity = new Vector2(Body.velocity.x, 0);
            Body.velocity = new Vector2(Body.velocity.x, _height);
            JumpCount--;
            AudioController.AudioControllerInstance.PlayAudio("playerJump");
        }
    }

    public void RestoreJump(){
        if(InFloor() && JumpCount < totalJump){
            JumpCount = totalJump;
        }
    }

    #endregion

    #region life controller

    public void TakeDamage(){
        life--;
        if(life <= 0){
            Dead();
        }
    }

    private bool InFloor(){
        return Physics2D.OverlapCircle(footPosition.position, floorDistance, floorLayer);
    }

    public void Dead(){
        Alive = false;
        Animator.SetBool ("Dead", true);
        SetGravityStatus(false, false);
        RotatePlayer(0);
        AudioController.AudioControllerInstance.PlayAudio("playerDead");
        ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
        collider.enabled = false;
    }

    public void GhostMode(){

        transform.position = Vector2.MoveTowards(transform.position, spawnPointPosition.position, ghostSpeed*setGhostVelocityIncrement()*Time.deltaTime);
        SetGhostDirection();
        if(transform.position == spawnPointPosition.position) SpawnBody();
    }

    public float setGhostVelocityIncrement(){
        float incremetController = 10;
        int minIncrement = 1;
        int maxIncrement = 4;

        float _distance = Math.Abs(transform.position.x - spawnPoint.transform.position.x);

        float _incrementedVelocity = _distance/incremetController > maxIncrement ? maxIncrement : _distance/incremetController > minIncrement ? _distance/incremetController : minIncrement;

        return _incrementedVelocity;
    }

    public void SetGhostDirection(){
        float scale = Math.Abs(transform.localScale.x);
        if(transform.position.x > spawnPoint.position.x){
            base.SetFlip(scale);
        }else if (transform.position.x < spawnPoint.position.x){
            base.SetFlip(-scale);
        }
    }

    protected void SpawnBody(){
        collider.enabled = true;
        SetGravityStatus(true);
        Animator.SetBool ("Dead", false);
        Body.gravityScale = this.gravity;
        life = InitialLife;
        if(!Alive)Alive = true;
        Body.velocity = new Vector2(0,0);
        Body.position = spawnPoint.position;
    }

    #endregion

    #region Trigger Controller

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Obstacle") || other.CompareTag("Enemy")){
            TakeDamage();
        }else if(other.CompareTag("Goal")){
            int _nextStage = currentStage+1;
            GameController.ControllerInstance.LoadScennes("Menu");
        }
    }

    #endregion
}
