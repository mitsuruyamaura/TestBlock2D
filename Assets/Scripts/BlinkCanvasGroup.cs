using UnityEngine;
using DG.Tweening;

public class BlinkCanvasGroup : MonoBehaviour
{
    public float duration;
    public Ease easeType;
    public CanvasGroup canvasGroup;

    void Start(){
        canvasGroup.DOFade(0.0f, duration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
}
