using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    #region Variables

    [Header ("Física do personagem")]
    [SerializeField] protected float moveSpeed = 5;
    [SerializeField] protected float gravity = 5;
    [SerializeField] protected int gravitySpeedIncrement = 10;
    [SerializeField] protected bool GravityOn = true;

    [Header ("Dados do personagem")]
    [SerializeField] protected Transform footPosition;

    [Header ("Objetos e Componentes")]
    protected Animator Animator;
    protected Rigidbody2D Body;
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected LayerMask floorLayer;
    [SerializeField] protected float floorDistance = 0.2f;

    #endregion

    #region Base Methods

    protected virtual void Awake ()
    {
        Body = GetComponent<Rigidbody2D> ();
        Animator = GetComponent<Animator> ();
        SpawnBody();
    }

    protected virtual void Update ()
    {
        Flip();
    }

    #endregion

    #region Movimentation Controller

    protected virtual void Move(float MoveX, float MoveY)
    {
        Body.velocity = new Vector2 (MoveX * moveSpeed, Body.velocity.y);
    }

    protected void Flip(){
        if((transform.localScale.x>0 && Body.velocity.x<0) || (transform.localScale.x<0 && Body.velocity.x>0)){
            Vector2 _localScale = transform.localScale;
            _localScale.x *=-1;
            transform.localScale = _localScale;
        }
    }

    #endregion

    #region Functional Methods

    protected virtual void SpawnBody(){
        Body.velocity = new Vector2(0,0);
        Body.position = spawnPoint.position;
    }

    protected bool InFloor(){
        return Physics2D.OverlapCircle(footPosition.position, floorDistance, floorLayer);
    }

    #endregion
}
