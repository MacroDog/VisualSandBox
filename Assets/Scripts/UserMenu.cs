using UnityEngine;
using System.Collections;

public class UserMenu : MonoBehaviour {
    private bool IsOpen=false;
    private bool CanUse=false;
    [SerializeField]
    private GameObject menu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& CanUse == true)
        {
            OpenOrCloseMainMenu();
        }
	}
    public void OpenOrCloseMainMenu()
    {
        if (IsOpen==false)
        {
            Time.timeScale = 0;
            menu.SetActive(true);
            IsOpen = true;
        }
        else
        {
            Time.timeScale = 1;
            menu.SetActive(true);
            IsOpen = false;
        }
    }
    public void ChangeCanUse()
    {
        CanUse = true;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
