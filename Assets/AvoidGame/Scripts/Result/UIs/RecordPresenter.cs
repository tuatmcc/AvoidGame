using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RecordPresenter : MonoBehaviour
{
    private Image image;
    [SerializeField] private int targetRank;
    [Inject] ResultSceneManager _sceneManager;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    void Start()
    {
        _sceneManager.OnRecordGot += SetNewRecordImage;
    }

    private void SetNewRecordImage(List<long> timeList, long time)
    {
        if (targetRank == 0) return;
        if(timeList.IndexOf(time) + 1 == targetRank)
        {
            image.enabled = true;
        }
    }
}