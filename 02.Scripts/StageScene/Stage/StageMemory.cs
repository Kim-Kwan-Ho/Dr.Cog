using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageMemory : BaseBehaviour
{
    [Header("UI")]
    [SerializeField] private MemoryUI _memoryUI;

    [Header("Memory")]
    private float _curMemory = 0;
    private float _goalMemory = 0;
    private bool _stageEnded;

    [Header("Cheer")]
    [SerializeField] private StageCheerMemorySo _cheerMemorySo;
    public void SetMemory(float goalMemory)
    {
        _stageEnded = false;
        _goalMemory = goalMemory;
        _memoryUI.SetGoalMemoryText(goalMemory);
        _cheerMemorySo.InitializeStageCheerMemorySo();

    }

    private bool CheckGameSucceed()
    {
        return _curMemory >= _goalMemory;
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _memoryUI.UpdateMemoryText(_curMemory);
    }
    public virtual void AddMemory(float amount)
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_AddMemory);
        if (_stageEnded)
            return;
        _curMemory += amount;
        _memoryUI.UpdateMemoryTextAnimation(_curMemory);
        int cheerIndex = _cheerMemorySo.CheckCheerMemory(_curMemory / _goalMemory);
        if (cheerIndex != -1)
        {
            StageSceneManager.Instance.EventStageScene.CallStartCheer(ECheerType.Memory, cheerIndex);
        }
        if (CheckGameSucceed())
        {
            _stageEnded = true;
            StageSceneManager.Instance.EventStageScene.CallStageSucceed();
        }
    }



    #region Legacy
    /*
      
     private Coroutine _debuffCoroutine; 
     
    [SerializeField] private float _decreaseTime = 1;
    [SerializeField] private float _decreaseAmount = 1;
    [SerializeField] private float _decreaseAmountIncreaseTime = 10;
    [SerializeField] private float _decreaseAmountIncreaseAmount = 5;

    void Update()
    {
        //StartCoroutine(CoDecreaseMemory());
        //StartCoroutine(CoIncreaseDecreaseAmountTimer());
    }

    private IEnumerator CoIncreaseDecreaseAmountTimer()
    {
        yield return new WaitForSeconds(_decreaseAmountIncreaseTime);
        _decreaseAmount += _decreaseAmountIncreaseAmount;
        StartCoroutine(CoIncreaseDecreaseAmountTimer());
    }

    private IEnumerator CoDecreaseMemory()
    {
        yield return new WaitForSeconds(_decreaseTime);
        _memory -= _decreaseAmount;
        _memory = Mathf.Max(_memory, 0);
        StartCoroutine(CoDecreaseMemory());
    }

     */
    #endregion


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _memoryUI = GameObject.FindAnyObjectByType<MemoryUI>();
        _cheerMemorySo = FindObjectInAsset<StageCheerMemorySo>();
    }
#endif
}
