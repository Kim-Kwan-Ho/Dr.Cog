using UnityEngine;

public class DebuffSkillGroup : BaseBehaviour
{
    [SerializeField] private DebuffSkillInfo[] _debuffSkillInfos;

    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff += Event_ActiveDebuff;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff -= Event_ActiveDebuff;
    }


    private void Event_ActiveDebuff(StageSceneDebuffEventArgs eventArgs)
    {
        var debuffSkillSo = eventArgs.DebuffSo.DebuffUiSo;
        if (debuffSkillSo.DebuffSkillUiSos == null)
            return;
        debuffSkillSo.LevelChanged += LevelChanged;

        for (int i = 0; i < debuffSkillSo.DebuffSkillUiSos.Length; i++)
        {
            _debuffSkillInfos[i].SetDebuffSkillInfo(debuffSkillSo.DebuffSkillUiSos[i]);
            _debuffSkillInfos[i].DeactiveSkill();
        }
    }
    private void LevelChanged(int level)
    {
        for (int i = 0; i < _debuffSkillInfos.Length; i++)
        {
            if (i < level)
            {
                _debuffSkillInfos[i].ActiveSkill();
            }
            else
            {
                _debuffSkillInfos[i].DeactiveSkill();
            }
        }
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _debuffSkillInfos = GetComponentsInChildren<DebuffSkillInfo>();
    }
#endif
}
