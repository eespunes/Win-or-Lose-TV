using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListSeason : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint = null;

    [SerializeField] private TMP_InputField numberOfMatches;

    [SerializeField] private GameObject item = null;
    [SerializeField] private GameObject back = null;

    [SerializeField] private RectTransform content = null;


    public void GenerateList()
    {
        var numberOfItems = Int32.Parse(numberOfMatches.text);
        //setContent Holder Height;
        content.sizeDelta = new Vector2(0, numberOfItems * 40f);

        for (int i = 0; i < numberOfItems; i++)
        {
            // 60 width of item
            float spawnY = i * 35;
            //newSpawn Position
            Vector3 pos = new Vector3(spawnPoint.position.x, -spawnY, spawnPoint.position.z);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, spawnPoint.rotation);
            //setParent
            SpawnedItem.transform.SetParent(spawnPoint, false);
            //get ItemDetails Component
            ItemSeason itemSeason = SpawnedItem.GetComponent<ItemSeason>();
            itemSeason.matchDay.text = (i + 1) < 10 ? "J0" + (i + 1) : "J" + (i + 1);
            itemSeason.home.text = PlayerPrefs.GetString(itemSeason.matchDay.text + " Home");
            itemSeason.away.text = PlayerPrefs.GetString(itemSeason.matchDay.text + " Away");
        }

        GameObject SpawnedBack = Instantiate(back,
            new Vector3(spawnPoint.position.x + 100, -numberOfItems * 35, spawnPoint.position.z), spawnPoint.rotation);
        SpawnedBack.transform.SetParent(spawnPoint, false);
        Button listReset = SpawnedBack.GetComponent<Button>();
        listReset.onClick.AddListener(ResetList);

        numberOfMatches.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ResetList()
    {
        foreach (Transform child in spawnPoint)
        {
            Destroy(child.gameObject);
        }

        numberOfMatches.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}