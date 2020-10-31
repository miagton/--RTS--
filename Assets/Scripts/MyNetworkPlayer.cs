using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour 
{
    [SerializeField] private Renderer diplayColorRenderer=null;
    [SerializeField] private TMP_Text  displayNameText=null;

    [SerializeField] private List<string> forbiddenNamesList= new List<string>{"Jopa", "popa","game" };
    
    
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))] 
    [SerializeField] private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdate))]// hook makes so method with hoooked Name will be called when variable is sync
    [SerializeField] private Color playerColor = Color.black;

#region Server
    [Server]//called on a server
    public void SetDisplayName(string newDisplayName)
    {
        displayName=newDisplayName;
    }

    [Server]
    public void SetPlayerColor(Color newColor)
    {
        
        playerColor=newColor;
       
    }
    [Command]//called from a client
    private void CmdSetDisplayName(string newDisplayName)
    {
        if(forbiddenNamesList.Contains(newDisplayName)){return;}// validating the name (any type of check) before assigning it
        
        
        //calling Remote Procedural command  => calling method on client => all client in the game will Log their name
        RpcLogNewName(newDisplayName);

        // before this we can make some logic for server validation for example => if name is valid or not taken etc.
        SetDisplayName(newDisplayName);
 
    }
    
#endregion

#region  Client
//THose 2 methods are called when SyncVar's are updated
    private void HandleDisplayColorUpdate(Color oldColor,Color newColor)// setting color of player
    {
        diplayColorRenderer.material.SetColor("_BaseColor",newColor);

    }

    private void HandleDisplayNameUpdate(string oldName,string newName)//setting name of player on UI
    {
        displayNameText.text=newName;
    }

    [ContextMenu("Set My Name")]//attribute enables the posibillity to call this method in Unity editor via context menu
    private void SetMyName()
    {
      CmdSetDisplayName("New Player");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
       Debug.Log(newDisplayName);
        
    }
#endregion
    
}
