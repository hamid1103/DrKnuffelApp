using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineHandler : MonoBehaviour
{
    public int OrderIndex = 0;
    private PlayableDirector pd;
    private GameManager gm;
    public bool firstEnable = false;
    
    void Start()
    {
        pd = this.gameObject.GetComponent<PlayableDirector>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    public void Restart()
    {
        //Step already completed
        if (gm.LocalSaveData.CompletedSteps.Contains(OrderIndex))
        {
            pd.time = 0;
            pd.Play();
        }
    }
    
}
