using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gui.Menus
{
    public class StartMenu: MonoBehaviour
    {
        public void LoadGameScene() {
            SceneManager.LoadScene("Main Scene");
        }
    }
}