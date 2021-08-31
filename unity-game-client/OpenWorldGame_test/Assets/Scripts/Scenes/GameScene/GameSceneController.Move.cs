using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneController.Move 
public partial class GameSceneController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveIntervalSeconds=0.2f;
    [SerializeField] private Joystick joystick;
    [SerializeField] PlayerController myPlayerController;

    private void StartMoveInterval()
    {
        StartCoroutine(StartMoveIntervalCoroutine());
    }

    private IEnumerator StartMoveIntervalCoroutine()
    {
        while (true)
        {
            if (joystick.Horizontal!=0 && joystick.Vertical!=0)
            {
                SendMove(myPlayerController.gameObject.transform.position, myPlayerController.NextPositionFromVector2(new Vector2(joystick.Horizontal,joystick.Vertical) ));
            }
            yield return new WaitForSeconds(moveIntervalSeconds);
        }
    }


    private void SendMove(Vector3 nowPos, Vector3 newPos)
    {
        this.gameMethodHandler.SendMove(nowPos, newPos);
        chatInputField.text = "";
    }

    private void OnReceiveMove(uint user_id, Gamemodel.MoveMethod moveMethod)
    {
        Debug.Log($"Received Move Method: UserID: {user_id}, position: {moveMethod.to.position}, rotation: {moveMethod.to.rotation}");
        //dispatcher.Invoke(() => { this.ingameManager.IngameClientsInstances[user_id].playerController.MoveTo(moveMethod.to); });
        this.ingameManager.IngameClientsInstances[user_id].playerController.MoveTo(moveMethod.to);
    }

}
