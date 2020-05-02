using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopUp : MonoBehaviour
{
    public void PlayToGame() {
        if (GameMaster.gameState == GAME_STATE.WAIT) {
            GameMaster.gameState = GAME_STATE.PLAY;
            Time.timeScale = 1.0f;
        }
    }

    public void StoptoGame() {
        if (GameMaster.gameState == GAME_STATE.PLAY) {
            GameMaster.gameState = GAME_STATE.WAIT;
            Time.timeScale = 0.0f;
        }
    }
}
