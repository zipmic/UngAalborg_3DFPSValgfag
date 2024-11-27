using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public GameObject[] GOToActivateOnTouch;
    public GameObject EndScreentoActivate;
    public float TimeFromGoalToEndscreenDisplay;

    // Start is called before the first frame update
    void Start()
    {
      foreach(GameObject g in GOToActivateOnTouch)
        {
            g.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (GameObject g in GOToActivateOnTouch)
            {
                g.SetActive(true);
            }
			StartCoroutine(EndScreenDisplay());
		}
  
    }

    IEnumerator EndScreenDisplay()
    {

        yield return new WaitForSeconds(TimeFromGoalToEndscreenDisplay);
        if (EndScreentoActivate != null)
        {
            EndScreentoActivate.SetActive(true);
        }

    }
}
