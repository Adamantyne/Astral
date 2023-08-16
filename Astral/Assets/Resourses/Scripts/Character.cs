using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    #region Variables

    [Header ("Física do personagem")]
    [SerializeField] protected float moveSpeed = 5;
    [SerializeField] protected float gravity = 5;
    [SerializeField] protected bool GravityOn = true;

    [Header ("Objetos e Componentes")]
    protected Animator Animator;
    protected Rigidbody2D Body;

    #endregion

    #region Base Methods

    protected virtual void Awake ()
    {
        Body = GetComponent<Rigidbody2D> ();
        Animator = GetComponent<Animator> ();
    }

    protected virtual void Update ()
    {
        
    }

    #endregion

    #region Movimentation Controller

    protected virtual void Move(float MoveX, float MoveY = 0)
    {
        Body.velocity = new Vector2 (MoveX * moveSpeed, Body.velocity.y);
    }

    protected virtual void Flip(){
        Vector2 _localScale = transform.localScale;
        _localScale.x *=-1;
        transform.localScale = _localScale;
    }

    #endregion
}
