using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button m_ServerButton;
    [SerializeField] private Button m_HostButton;
    [SerializeField] private Button m_ClientButton;


    void Awake()
    {
        m_ServerButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartServer();
       });

        m_HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        m_ClientButton.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartClient();
       });
    }


}
