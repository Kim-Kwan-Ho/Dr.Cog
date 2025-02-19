using System;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "DebuffUI_", menuName = "Scriptable Objects/Debuffs/UI/Debuff")]
public class DebuffUISo : ScriptableObject
{
    [SerializeField] private string _debuffName;
    public string DebuffName { get { return _debuffName; } }
    public void SetDebuffName(string name)
    {
        _debuffName = name;
    }
    [SerializeField] private Sprite _debuffSprite;
    public Sprite DebuffSprite { get { return _debuffSprite; } }
    [SerializeField] private Sprite _stackSprite;
    public Sprite StackSprite { get { return _stackSprite; } }


    [SerializeField] private string _debuffDescription;
    public string DebuffDescription { get { return _debuffDescription; } }// ex) 이 디버프는 메모리 짱짱많이 벌어야함
    public void SetDebuffDescription(string description)
    {
        _debuffDescription = description;
    }

    [SerializeField] private string[] _stackDescriptions; // ex) 1레벨 : n개 이상 시 어지러움 발생 2레벨: ...
    public string[] StackDescriptions { get { return _stackDescriptions; } }
    public void SetStackDescriptions(string[] descriptions)
    {
        _stackDescriptions = descriptions;
    }

    [SerializeField] private DebuffSkillUISo[] _debuffSkillUiSos;
    public DebuffSkillUISo[] DebuffSkillUiSos { get { return _debuffSkillUiSos; } }

    public Action<int> LevelChanged;
}