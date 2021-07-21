using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListVideo : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint = null;


    [SerializeField] private GameObject item = null;

    [SerializeField] private RectTransform content = null;
    private int _spawnY = 0;

    public void AddToList(string text)
    {
        //setContent Holder Height;
        content.sizeDelta += new Vector2(0, 35);

        // 60 width of item
        _spawnY += 35;
        //newSpawn Position
        Vector3 pos = new Vector3(spawnPoint.position.x, -_spawnY, spawnPoint.position.z);
        //instantiate item
        GameObject SpawnedItem = Instantiate(item, pos, spawnPoint.rotation);
        //setParent
        SpawnedItem.transform.SetParent(spawnPoint, false);
        //get ItemDetails Component
        ItemVideo itemSeason = SpawnedItem.GetComponent<ItemVideo>();
        itemSeason.text.text = text;
    }
}