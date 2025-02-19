using UnityEngine;


[CreateAssetMenu(fileName = "UI_", menuName = "Scriptable Objects/UI/Description")]

public class UIDescriptionSo : ScriptableObject
{
    [SerializeField] private string _uiName;
    public string UIName { get { return _uiName; } }


    [SerializeField] private string _uiDescription;
    public string UIDescripiton { get { return _uiDescription; } }

    public void SetUIName(string name)
    {
        _uiName = name;
    }

    public void SetUIDescription(string description)
    {
        _uiDescription = description;
    }
}
