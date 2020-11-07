using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public FixedButton nextLevelButton;
    public GameObject nextLevelButtonObj;

    void Update()
    {
        if (nextLevelButton.pressed)
        { }

        if ((Input.GetKey(KeyCode.O) || nextLevelButton.pressed) && SceneManager.GetActiveScene().name == "MyScene_0")
        {
            SceneManager.LoadScene("MyScene_1");
        }
        if ((Input.GetKey(KeyCode.O) || nextLevelButton.pressed) && SceneManager.GetActiveScene().name == "MyScene_1")
        {
            SceneManager.LoadScene("MyScene_0");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextLevelButtonObj.SetActive(true);
            StartCoroutine(EndTime());
        }
    }

    IEnumerator EndTime()
    {
        yield return new WaitForSeconds(0.1f);
        nextLevelButtonObj.SetActive(false);
    }
}
