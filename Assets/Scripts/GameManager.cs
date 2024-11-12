using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] Text remainDistanceText;
    [SerializeField] List<Text> nameHorse;
    [SerializeField] List<Horse> horses;
    [SerializeField] Vector3 cameraFinished;
    public Camera mainCamera;
    int remainDistance;
    private static GameManager _instance;
    private float totalRace;
    public static GameManager Instance => _instance;
    [SerializeField] AudioSource audioRace;
    public int RemainDistance { get => remainDistance; private set => remainDistance = value; }
    private Horse firstHorse;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        audioRace = GetComponent<AudioSource>();
        totalRace = endPosition.position.z - startPosition.position.z;
        audioRace.Play();
    }

    // Update is called once per frame
    void Update()
    {
        sortingRanking();
        showRemainDistance();
        slowDownCamera();
    }


    private void sortingRanking()
    {

        var listHorse = horses.OrderByDescending(o => o.transform.position.z).ToList();
        Vector3 currentPositionCamera = mainCamera.transform.position;
        firstHorse = listHorse[0];
        mainCamera.transform.position = new Vector3(currentPositionCamera.x, currentPositionCamera.y, firstHorse.transform.position.z);
        displayRanking(listHorse);
    }

    private void showRemainDistance()
    {
        remainDistance = (int)Math.Round(totalRace - firstHorse.transform.position.z);

        if (remainDistance >= 0f)
        {
            remainDistanceText.text = firstHorse.Name + ":" + remainDistance + Common.Meter;
            audioRace.Stop();
        }
        else
        {
            remainDistanceText.text = Common.Zero + "" + Common.Meter;
        }

    }

    private void slowDownCamera()
    {
        if (remainDistance <= 20)
        {
            Time.timeScale = 0.2f;
            cameraFinished = new Vector3(cameraFinished.x, cameraFinished.y, firstHorse.transform.position.z);
            Vector3 mainCamera = Camera.main.transform.position;
            Camera.main.transform.position = Vector3.Lerp(mainCamera, cameraFinished, 1);
        }
    }

    public void displayRanking(List<Horse> horses)
    {
        for (int i = horses.Count - 1; i >= 0; i--)
        {
            nameHorse[i].text = horses[i].Name;
        }

    }
}
