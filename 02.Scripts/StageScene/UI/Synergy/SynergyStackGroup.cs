using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyStackGroup : BaseBehaviour
{
    [SerializeField] private GameObject _synergyStackPrefab;
    [SerializeField] private SynergySo[] _synergySos;
    private Dictionary<ESynergyType, SynergyStackUI> _synergyStackDic = new Dictionary<ESynergyType, SynergyStackUI>();
    protected override void Initialize()
    {
        base.Initialize();
        CreateSynergyStackDic();
    }

    private void CreateSynergyStackDic()
    {
        foreach (var synergySo in _synergySos)
        {
            var synergyStack = Instantiate(_synergyStackPrefab, transform).GetComponent<SynergyStackUI>();
            synergyStack.InitializeSynergyStackUi(synergySo);
            synergyStack.gameObject.SetActive(false);
            _synergyStackDic[synergySo.SynergyType] = synergyStack;
        }
    }

    public void UpdateSynergyStack(ESynergyType type)
    {
        if (_synergyStackDic.ContainsKey(type))
        {
            _synergyStackDic[type].RefreshSynergy();
        }
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _synergyStackPrefab = FindObjectInAsset<SynergyStackUI>().gameObject;

    }
#endif
}