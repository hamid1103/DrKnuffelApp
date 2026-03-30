using UnityEngine;
using UnityEngine.SceneManagement;

public class switchscenes : MonoBehaviour
{
    public GameObject PathScreen;
    public GameObject StilLiggenGameScherm;
    public void switchsceneToMRIStilLiggen()
    {
        StilLiggenGameScherm.SetActive(true);
        PathScreen.SetActive(false);
    }
}
