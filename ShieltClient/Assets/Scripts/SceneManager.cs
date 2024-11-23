using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
   public static SceneManager Instance;

   private void Awake()
   {
      Instance = this;
   }

   public void SwitchScene(string sceneName)
   {
      UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
   }
}
