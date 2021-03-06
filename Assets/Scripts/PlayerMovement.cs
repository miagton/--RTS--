﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
   [SerializeField] private NavMeshAgent agent=null;
   private Camera mainCamera=null;

   #region Server
   [Command]
   private void CmdMove(Vector3 position)
   {
     if(!NavMesh.SamplePosition(position,out NavMeshHit hit,1f,NavMesh.AllAreas)){return;}//checking if mouse pos is on navmesh
     agent.SetDestination(hit.position);
   }
    #endregion
    #region Client
    public override void OnStartAuthority()
    {
        mainCamera=Camera.main;
    }

    [ClientCallback]//prevents server from running this code
     private void Update() 
    {
        if(!hasAuthority){return;}// if we have authority

        if(!Input.GetMouseButtonDown(1)){return;}

        Ray ray =  mainCamera.ScreenPointToRay(Input.mousePosition);//getting teh cursor position

        if(!Physics.Raycast(ray,out RaycastHit hit,Mathf.Infinity)) {return;}// if we hit smthing in scene

        CmdMove(hit.point);
    }
 
   #endregion
}
