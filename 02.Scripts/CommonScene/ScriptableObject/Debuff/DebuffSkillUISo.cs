using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffSkillUI_", menuName = "Scriptable Objects/Debuffs/UI/DebuffSkill")]
public class DebuffSkillUISo : ScriptableObject
{
    [SerializeField] public string _debuffSkillName; // ex) 우울
    public string DebuffSkillName { get { return _debuffSkillName; } }

    [SerializeField] public Sprite _debuffSkillSprite;
    public Sprite DebuffSkillSprite { get { return _debuffSkillSprite; } }

    [SerializeField] public string _debuffSkillDescription; // ex) 우울하면 메모리 감소
    public string DebuffSkillDescripiton { get { return _debuffSkillDescription; } }
}
