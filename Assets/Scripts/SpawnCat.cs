using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnCat : MonoBehaviour
{
    public static SpawnCat Instance;

    public GameObject[] catPrefabs;

    public List<GameObject> activeCat;

    public List<DragAndDrop> catStack;

    public GameObject vfx;

    private bool isWon = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (activeCat.Count >= 1) return;
        GameObject catToSpawn = Instantiate(catPrefabs[Random.Range(0, catPrefabs.Length)], transform.position, Quaternion.identity);
        activeCat.Add(catToSpawn);
    }

    public void CheckWin()
    {
        if (catStack.Count >= 5)
        {
            if (!isWon)
            {
                GameObject vfxVictory = Instantiate(vfx, transform.position, Quaternion.identity);
                Destroy(vfxVictory, 1f);
                StartCoroutine(ResetGame());
            }

            isWon = true;
        }
    }

    IEnumerator ResetGame()
    {

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
