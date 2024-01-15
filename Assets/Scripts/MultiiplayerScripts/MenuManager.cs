using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
   public Menu[] menus;
    public static MenuManager instance;

    private void Awake()
    {
        instance = this; 
    }

    public void OpenMenu(string menuName)
    {
        foreach (var menu in menus)
        {
            if (menu.MenuName == menuName)
            {
                OpenMenu(menu);
            }else if(menu.IsOpen)
            {
                CloseMenu(menu);
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        foreach(var _menu in  menus)
        {
            if(_menu.IsOpen)
            {
                CloseMenu(_menu);
            }
        }
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
