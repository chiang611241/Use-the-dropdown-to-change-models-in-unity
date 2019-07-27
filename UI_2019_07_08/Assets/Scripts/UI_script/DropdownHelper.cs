using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropdownHelper : MonoBehaviour
{
    public Dropdown dropdown;
    public Sprite[] models;

    void Start()
    {
        dropdown.ClearOptions();
        List<Dropdown.OptionData> modelItems = new List<Dropdown.OptionData>();
        foreach(var model in models){
            var modelOption = new Dropdown.OptionData(model.name,model);
            modelItems.Add(modelOption);
        }

        dropdown.AddOptions(modelItems);
    }
}
