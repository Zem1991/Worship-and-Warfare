//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class AbstractFieldPiece : AbstractPiece
//{
//    //[Header("Animator parameters")]
//    //public bool anim_movement;
//    //public float anim_directionX;
//    //public float anim_directionZ = -1;

//    //protected override void AnimatorParameters()
//    //{
//    //    anim_movement = inMovement;
//    //    animator.SetBool("Movement", anim_movement);

//    //    anim_directionX = 0;
//    //    if (direction.x < 0) anim_directionX = -1;
//    //    if (direction.x > 0) anim_directionX = 1;
//    //    animator.SetFloat("Direction X", anim_directionX);

//    //    anim_directionZ = 0;
//    //    if (direction.z < 0) anim_directionZ = -1;
//    //    if (direction.z > 0) anim_directionZ = 1;
//    //    animator.SetFloat("Direction Z", anim_directionZ);
//    //}

//    //public override void StartTurn()
//    //{
//    //    ResetMovementPoints();
//    //}

//    //public override void EndTurn()
//    //{
//    //    throw new System.NotImplementedException();
//    //}

//    //public override void InteractWithTile(AbstractTile target, bool canPathfind)
//    //{
//    //    if (canPathfind)
//    //    {
//    //        if (target)
//    //        {
//    //            if (HasPath(target)) Move();
//    //            else FieldManager.Instance.pieceHandler.Pathfind(this, target as FieldTile);
//    //        }
//    //    }
//    //    else
//    //    {
//    //        if (HasPath()) Move();
//    //    }
//    //}

//    //public override void InteractWithPiece(AbstractPiece target, bool canPathfind)
//    //{
//    //    InteractWithTile(target.targetTile, canPathfind);
//    //}
//}
