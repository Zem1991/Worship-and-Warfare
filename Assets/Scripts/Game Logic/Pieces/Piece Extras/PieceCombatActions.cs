//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PieceCombatActions : MonoBehaviour
//{
//    [Header("Associated objects")]
//    public Animator animator;

//    [Header("References")]
//    public PieceCombatActions retaliationTarget;

//    [Header("Movement states")]
//    public bool movement_start;
//    public bool movement_going;
//    public bool movement_end;

//    /*
//    *   BEGIN:  Movement
//    */
//    public IEnumerator Movement()
//    {
//        yield return StartCoroutine(MovementStart());
//        yield return StartCoroutine(MovementGoing());
//        yield return StartCoroutine(MovementEnd());
//    }
//    public IEnumerator MovementStart()
//    {
//        movement_start = true;

//        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
//        while (state.IsName("Movement start")) yield return null;

//        movement_start = false;
//    }
//    public IEnumerator MovementGoing()
//    {
//        movement_going = true;

//        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
//        while (pieceMovement.inMovement) yield return null;
//        //while (state.IsName("Movement going")) yield return null;

//        movement_going = false;
//    }
//    public IEnumerator MovementEnd()
//    {
//        movement_end = true;

//        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
//        while (state.IsName("Movement end")) yield return null;

//        movement_end = false;
//    }
//    /*
//    *   END:    Movement
//    */

//    ///*
//    //*   BEGIN:  Hurt or Dead
//    //*/
//    //public IEnumerator HurtOrDead(int damage)
//    //{
//    //    ReceiveDamage(damage);
//    //    if (alive) yield return StartCoroutine(Hurt());
//    //    else yield return StartCoroutine(Dead());
//    //}
//    //public IEnumerator Hurt()
//    //{
//    //    yield return true;
//    //}
//    //public IEnumerator Dead()
//    //{
//    //    yield return true;
//    //}
//    ///*
//    //*   END:    Hurt or Dead
//    //*/

//    ///*
//    //*   BEGIN:  Attack
//    //*/
//    //public IEnumerator Attack()
//    //{
//    //    if (attack.isMelee)
//    //    {
//    //        yield return StartCoroutine(Movement());
//    //        yield return StartCoroutine(EvaluateCounter());
//    //        yield return StartCoroutine(AttackMelee());
//    //        yield return StartCoroutine(EvaluateRetaliation());
//    //    }
//    //    else
//    //    {
//    //        yield return StartCoroutine(AttackRanged());
//    //    }
//    //}


//}
