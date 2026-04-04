using UnityEngine;
using UnityEngine.UI;

public class StepButtonProgressTracking : MonoBehaviour
{
    private GameManager _gameManager;

    public int StepID = -1;
    private Image buttonImage;
    private Button button;
    public GameObject arrowIndicator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        buttonImage = gameObject.GetComponent<Image>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        arrowIndicator = transform.Find("ArrowIndicator").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (StepID != -1)
        {
            if (_gameManager.LocalSaveData.CompletedSteps.Contains(StepID))
            {
                buttonImage.color = Color.green;
                button.interactable = true;
                arrowIndicator.SetActive(false);
            }else if (_gameManager.LocalSaveData.CompletedSteps.Contains(StepID-1) || StepID == 0)
            {
                buttonImage.color = Color.blue;
                button.interactable = true;
                arrowIndicator.SetActive(true);
            }
            else
            {
                buttonImage.color = Color.gray;
                button.interactable = false;
            }
        }
    }
}
