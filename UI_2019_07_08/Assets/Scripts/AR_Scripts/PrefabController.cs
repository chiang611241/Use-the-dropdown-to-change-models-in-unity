using UnityEngine;
using UnityEngine.UI;

public class PrefabController : MonoBehaviour
{
    public static int  prefab;
    public void selectmodels(int value){
        switch(value){
            case 0:
                break;
            case 1:
                prefab = 1;
                Debug.Log("prefab = 1");
                break;
            case 2:
                prefab = 2;
                 Debug.Log("prefab = 2");
                break;
            case 3:
                prefab = 3;
                break;
        }
    }
}
