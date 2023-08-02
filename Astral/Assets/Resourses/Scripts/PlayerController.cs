using UnityEngine;

public class PlayerController : Character
{
    #region Variables

    [Header ("Física do jogador")]
    [HideInInspector] private bool Alive = true;
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private int totalJump = 1;
    //[SerializeField] private float jumpSpeedDecrement = 10;
    private int JumpCount;

    [Header ("Dados do jogador")]
    private Animator animator;
    [SerializeField] private int life = 1;
    private int InitialLife;
    public static PlayerController PlayerInstance;
    [SerializeField] private int currentStage = 1;

    #endregion

    #region Base Methods

    protected override void Awake(){
        base.Awake();
        InitialLife = life;
        JumpCount = totalJump;
        InitialLife = life;
        PlayerInstance = this;
        this.animator = GetComponent<Animator> ();
    }

    protected override void Update()
    {
        base.Update();
        RestoreJump();
        Flip();
    }

    #endregion

    #region Inputs Controller

    
    public void InputActions(float MoveX, float MoveY, bool FPress, bool WPress, bool JumpPress){
        if(!Alive) return;
        if((MoveX!=0 || MoveY!=0)){
            PlayerMove(MoveX, MoveY);
            animator.SetBool ("Running", true);
        }else{
            animator.SetBool ("Running", false);
        }
        if(FPress){
            this.GravityOn = !GravityOn;
            GravityControll(GravityOn);
            ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
            RotatePlayer(0);
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

    public void GravityControll(bool isOn){
        if(isOn){
            Body.gravityScale = this.gravity;
        }else{
            Body.velocity = new Vector2(0,0);
            Body.gravityScale = 0;
            AudioController.AudioControllerInstance.PlayAudio("playerGravity");
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

    public void Dead(){
        Alive = false;
        RotatePlayer(0);
        SpawnBody();
        life = InitialLife;
        AudioController.AudioControllerInstance.PlayAudio("playerDead");
        ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
    }

    protected override void SpawnBody(){
        if(!Alive)Alive = true;
        base.SpawnBody();
    }

    #endregion

    #region Trigger Controller

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Obstacle")){
            TakeDamage();
        }else if(other.CompareTag("Goal")){
            int _nextStage = currentStage+1;
            GameController.ControllerInstance.LoadScennes("Menu");
        }
    }

    #endregion
}
