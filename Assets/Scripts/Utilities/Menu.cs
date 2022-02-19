using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject createRoomPanel;
    [SerializeField] private GameObject roomMenu;
    [SerializeField] private GameObject connectToRoomMenu;

    public  Text header;
    private static Menu _instance;

    public static Menu Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Menu>();
                if (_instance == null)
                {
                    GameObject singletonGameObject = new GameObject("Menu");
                    _instance = singletonGameObject.AddComponent<Menu>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void OpenLoading()
    {
      loadingPanel.SetActive(true);
    }
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        createRoomPanel.SetActive(false);
        loadingPanel.SetActive(false);
        roomMenu.SetActive(false);
        connectToRoomMenu.SetActive(false);
    }

    public void OpenRoomMenu()
    {
        roomMenu.SetActive(true);
    }
    
    public void OpenConnectToRoomMenu()
    {
        menuPanel.SetActive(false);
        connectToRoomMenu.SetActive(true);
    }

    public void OpenCreateRoomMenu()
    {
        menuPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
}
