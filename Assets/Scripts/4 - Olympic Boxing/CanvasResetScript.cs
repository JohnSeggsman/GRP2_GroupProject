using System.Collections;
using UnityEngine;

public class CanvasResetScript : MonoBehaviour
{
    public IEnumerator ResetKnockoutCanvas()
    {
        if(BoxingMinigameManager.Instance.isGameEnded == false && BoxingMinigameManager.Instance.isKnockedOut == false)
        {
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("KnockoutAnim", false);
            yield return new WaitForSeconds(0.25f);
            GameObject.Find("Canvas").GetComponent<Animator>().enabled = false;
            yield break;
        }
        else
        {
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("KnockoutAnim", false);
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("GameEnded", true);
            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(BoxingMinigameManager.Instance.MinigameSFX[14]);
        }
    }

    public void PerfectIndicatorDisplay()
    {
        if(BoxingMinigameManager.Instance.PerfectRound)
        {
            GameObject.Find("MinigameManager").GetComponent<AudioSource>().PlayOneShot(BoxingMinigameManager.Instance.MinigameSFX[12]);
            GameObject PerfectIndicator = Instantiate(BoxingMinigameManager.Instance.GameObjects[5], new Vector3(0, 0, 0), Quaternion.identity);
            PerfectIndicator.transform.SetParent(GameObject.Find("Canvas").transform, false);
            Destroy(PerfectIndicator, 5);
        }
    }
}