using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField]
    Transform m_PolySpatialCameraTransform;

    [SerializeField]
    GameObject spawnedPrefab;

    List<NetworkObject> spawnedList = new List<NetworkObject>();

    private void Start()
    {

    }

    void Update()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.SpawnManager != null)
        {
            var object_list = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList;
            infoText.text = "";
            foreach (var obj in object_list)
            {
                infoText.text += "\n" + obj.name;
            }
        }

        //objectListText.text = "";

        //objectListText.text = "Camera Offset:" + cameraOffset.position.ToString()
        //    + "\n" + "Volume Camera:" + volumeCamera.position.ToString()
        //    + "\n" + "Hand 1:" + hand1.position.ToString()
        //    + "\n" + "Hand 2:" + hand2.position.ToString()
        //    + "\n" + "Cylinder:" + cylinder.position.ToString();
    }

    public void SpawnGameObject(Vector3 pos)
    {
        if(m_PolySpatialCameraTransform != null)
            pos = m_PolySpatialCameraTransform.InverseTransformPoint(pos);
        SpawnGameObjectServerRpc(pos);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnGameObjectServerRpc(Vector3 pos, ServerRpcParams serverRpcParams = default)
    {
        var gameobject = Instantiate(spawnedPrefab, pos, Quaternion.identity);
        gameobject.GetComponent<NetworkObject>().Spawn();

        spawnedList.Add(gameobject.GetComponent<NetworkObject>());
    }

    public void ClearGameObject()
    {
        ClearGameObjectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClearGameObjectServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = spawnedList.Count - 1; i >= 0; i--)
        {
            spawnedList[i].Despawn();
            spawnedList.RemoveAt(i);
        }
    }



    public TextMeshProUGUI logText;
    public TextMeshProUGUI infoText;
    int count = 0;
    public void OnPinch(Vector3 pos)
    {
        SpawnGameObject(pos);


        Debug.Log("Spawn at:" + pos.ToString());
        LogToUI("Spawn at:" + pos.ToString());
    }

    public void OnPerformGesture()
    {
        ClearGameObject();

        Debug.Log("OnPerformGesture!");
        LogToUI("OnPerformGesture!");
    }

    void LogToUI(string str)
    {
        if (count > 10)
        {
            logText.text = "";
            count = 0;
        }

        logText.text = count == 0 ? str : logText.text + "\n" + str;
        count++;        
    }
}
