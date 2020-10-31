using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
  
    public override void OnServerAddPlayer(NetworkConnection conn)//ovveriding virtual method from NetworkManager
    {
        base.OnServerAddPlayer(conn);//calling base functionality to execute

        MyNetworkPlayer player= conn.identity.GetComponent<MyNetworkPlayer>();//grabbin networkPlayer comp from conn=>identity
        
        player.SetDisplayName($"Player{numPlayers}");
        
        Color displayColor = new Color(Random.Range(0,5),Random.Range(0,5),Random.Range(0,5));
        
        player.SetPlayerColor(displayColor);


       
    }
}

