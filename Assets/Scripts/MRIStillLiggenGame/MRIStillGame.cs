using TMPro;
using UnityEngine;

public class MRIStillGame : MonoBehaviour
{
    private bool _Started = false;
    public TMP_Text debugGT;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.gyro.enabled = true;
    }

    public void SetStarted(bool started)
    {
        _Started = started;
    }

    // Update is called once per frame
    void Update()
    {
        
        debugGT.text = $"{Input.gyro.attitude.x}, {Input.gyro.attitude.y}, {Input.gyro.attitude.z}";
        while (_Started)
        {
            
        }
    }
}
