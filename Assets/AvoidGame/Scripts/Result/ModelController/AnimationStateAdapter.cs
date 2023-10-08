using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimationStateAdapter : MonoBehaviour
{
    private Animator animator;
    [Inject] ResultSceneManager _sceneManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        _sceneManager.OnRecordGot += ChangeAnimation;
    }

    private void ChangeAnimation(List<long> timeList, long time)
    {
        animator?.SetInteger("Ranking", timeList.IndexOf(time) + 1);
    }

}
