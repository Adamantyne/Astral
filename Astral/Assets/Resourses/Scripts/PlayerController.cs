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
    [SerializeField] private int life = 1;
    private int InitialLife;
    public static PlayerController PlayerInstance;
    public int test = 1;
    [SerializeField] private int currentStage = 1;

    #endregion

    #region Base Methods

    protected override void Awake(){
        base.Awake();
        InitialLife = life;
        JumpCount = totalJump;
        InitialLife = life;
        PlayerInstance = this;
    }

    protected override void Update()
    {
        base.Update();
        RestoreJump();
        Inputs();
        Flip();
    }

    #endregion

    #region Inputs Controller

    public void Inputs ()
    {
        float MoveX = Input.GetAxisRaw ("Horizontal");         
        float MoveY = Input.GetAxisRaw ("Vertical");
        bool WPress = Input.GetKeyDown ("w");
        bool FPress = Input.GetKeyDown ("f");
        bool JumpPress = Input.GetButtonDown("Jump");
        bool LeftMouseDown = Input.GetMouseButtonDown (0);
        bool pause = Input.GetKeyDown(KeyCode.Escape);
        if(Alive){
            InputActions(MoveX, MoveY, FPress, WPress, JumpPress);
        }
        if(pause){
            GameController.ControllerInstance.PauseGame();
        }
    }

    public void InputActions(float MoveX, float MoveY, bool FPress, bool WPress, bool JumpPress){
        if((MoveX!=0 || MoveY!=0)){
            PlayerMove(MoveX, MoveY);
        }
        if(FPress){
            this.GravityOn = !GravityOn;
            GravityControll(GravityOn);
            ItensController.itenInstance.SetItemStatus("Gravity",!GravityOn);
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

    #endregion

    #region Gravity Controller

    void GravityOffMove(float MoveX, float MoveY){
        if(Mathf.Approximately(Body.velocity.x,0) && Mathf.Approximately(Body.velocity.y,0)){
            if(MoveX!=0 && MoveY!=0){
                    MoveY =0;
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
        }
    }

    #endregion

    #region pulo

    public void Jump(float _height){
        if(this.GravityOn && JumpCount>0 && InFloor()){
            Body.velocity = new Vector2(Body.velocity.x, 0);
            Body.velocity = new Vector2(Body.velocity.x, _height);
            JumpCount--;
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
        SpawnBody();
        life = InitialLife;
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
            GameController.ControllerInstance.LoadScennes("Stage_"+_nextStage);
        }
    }

    #endregion
}
