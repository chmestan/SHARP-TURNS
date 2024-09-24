using UnityEngine;
using System.Collections;
using TMPro;

public class SawSpawner : MonoBehaviour
{
    [Header("Spawning Behavior")]
    [SerializeField] GameObject[] saws;
    [SerializeField] float spawnInterval = 5;    
    [SerializeField] bool spawningSaws = true;

    [Header("Spawn Area")]
    [SerializeField] Vector2 spawnAreaCenter;    
    [SerializeField] Vector2 spawnAreaSize;   

    [Header("Saw Behavior")]
    [SerializeField] float sawTransparency = 0.5f;  
    [SerializeField] float delayBeforeActivation = 0.5f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Elapsed time")]
    [SerializeField] float elapsedTime; //for debugging

    private void OnEnable() 
    {
        StartCoroutine(SpawnSawsCoroutine());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimeText();

    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"Time survived: {minutes:D2} : {seconds:D2}"; 
    }

    private IEnumerator SpawnSawsCoroutine() 
    {
        while (spawningSaws)
        {
            SpawnSaw();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSaw()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2)
        );

        GameObject sawInstance = Instantiate(ChooseSawBasedOnTime(), (randomPosition), Quaternion.identity);
        sawInstance.SetActive(false);

        SawMovement sawMovement = sawInstance.GetComponent<SawMovement>();
        SawLifetime sawLifetime = sawInstance.GetComponent<SawLifetime>();

        float calculatedDisableTime = Mathf.Max(10f, elapsedTime / 2); 
        float calculatedDestroyTime = calculatedDisableTime + 10f; 
        sawLifetime._DisableTime = calculatedDisableTime;
        sawLifetime._DestroyTime = calculatedDestroyTime;

        sawInstance.SetActive(true);
        sawMovement.ActivateSaw(delayBeforeActivation, sawTransparency);
    }

    private GameObject ChooseSawBasedOnTime()
    {
        if (elapsedTime < 6f) return saws[0];  
        else if (elapsedTime < 15f) return saws[1];
        else if (elapsedTime < 20f) return saws[2];
        else
        {
            int randomIndex = Random.Range(0, saws.Length);
            return saws[randomIndex];
        }
    }
}
