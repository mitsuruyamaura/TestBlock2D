using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public TransitionManager_2 transition;
    public float waitTime;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(MoveMenuScene());
        }
    }

    private IEnumerator MoveMenuScene() {
        transition.TransFadeOut(waitTime);
        yield return new WaitForSeconds(waitTime);
        SelectStage.stageNo = Random.Range(1,4);
        Debug.Log(SelectStage.stageNo);
        SceneManager.LoadScene("Main");
    }
}
