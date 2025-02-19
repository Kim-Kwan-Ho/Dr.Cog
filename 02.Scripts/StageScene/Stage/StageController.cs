using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(StageMemory))]
[RequireComponent(typeof(StageTime))]
[RequireComponent(typeof(StageReward))]

public class StageController : BaseBehaviour
{
    [Header("Stage")]
    [SerializeField] private StageMemory _memory;
    [SerializeField] private StageTime _time;
    [SerializeField] private StageReward _reward;

    private StageInfoSo _stageInfoSo;
    [Header("StageAdditiveStat")]
    [SerializeField] private StageAdditiveStatSo _stageAdditiveStatSo;

    private void SetStage()
    {
        _memory.SetMemory(_stageAdditiveStatSo.GetTotalTargetMemory(_stageInfoSo.GoalMemory));
        _time.SetTime(_stageAdditiveStatSo.GetTotalStageTime(_stageInfoSo.StageTime));
        _reward.SetReward(_stageInfoSo.RewardType, _stageInfoSo.MapPositionInfo);
    }

    public StageInfoSo GetStageInfo() { return _stageInfoSo; }

    #region Event

    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize += Event_StageInitialize;
        StageSceneManager.Instance.EventStageScene.OnAnimationEnded += Event_AnimationEnded;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize -= Event_StageInitialize;
        StageSceneManager.Instance.EventStageScene.OnAnimationEnded -= Event_AnimationEnded;
    }


    private void Event_StageInitialize(StageInfoSo stageInfoSo)
    {
        _stageInfoSo = stageInfoSo;
        SetStage();
        GameManager.PlayerGameData.AddVisitedCount(_stageInfoSo.MapPositionInfo);
    }

    private void Event_AnimationEnded()
    {
        if (!DialogueDataManager.Instance.CheckValidEventDialogue(DialogueType.StageEntered, _stageInfoSo.MapPositionInfo))
        {
            StageSceneManager.Instance.EventStageScene.CallStageStart();
        }
        else
        {
            //UIManager.Instance.OpenPopupUI<DialogueManager>()
            // Todo : use this & call StageSceneManager.Instance.EventStageScene.CallTimeResumed();
            //UIManager.Instance.OpenPopupUI<DialogTextBoxPopup>().StartInitialDialogue(DialogueType.StageEntered, _stageInfoSo.MapPositionInfo);
            UIManager.Instance.OpenPopupUI<StartTutorialOptionPopup>().StartTutorialPopup(_stageInfoSo.MapPositionInfo);
        }
    }
    #endregion


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _memory = GetComponent<StageMemory>();
        _time = GetComponent<StageTime>();
        _reward = GetComponent<StageReward>();
    }
#endif
}

#region Legacy
/*
using System;
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;
   
   public struct StageContainer
   {
       public StageStatSo StageSo;
       public int Level;
   
       public StageContainer(StageStatSo gearSo, int level = 1)
       {
           StageSo = gearSo;
           Level = level;
       }
   }
   
   public class StageController : BaseBehaviour
   {
       [SerializeField] private float _stageTime;
   
       [SerializeField] private MemoryController _memoryController;
       [SerializeField] private StageTimeController _stageTimeController;
   
       private Dictionary<int, StageContainer> _stageSos;
   
       [SerializeField] private int _currentStageID = 0;
       private bool _checked;
   
       // Start is called before the first frame update
       void Start()
       {
           _checked = false;
           LoadAllStageSo();
           SetStageUI(_currentStageID);
       }
   
       public void SetStageUI(int stageID)
       {
           _memoryController.SetStageMemory(_stageSos[stageID]);
           _stageTimeController.SetStageTime(_stageSos[stageID]);
       }
   
       //Check Stage Success
       public void CheckGameResult()
       {
           if (_memoryController.CheckMemoryGoal())
           {
               UIManager.Instance.OpenPopupUI<RewardPopup>();
           }
           else
           {
               var result = UIManager.Instance.OpenPopupUI<ResultPopup>();
               result.SetFailed();
           }
           //Debug.Log(_memoryController.CheckMemoryGoal());
       }
   
       #region LoadStage
       private void LoadAllStageSo()
       {
           _stageSos = new Dictionary<int, StageContainer>();
   
           StageStatSo[] allStages;
   
           allStages = Resources.LoadAll<StageStatSo>("ScriptableObjects/StageStatSo");
           int index = 0;
           foreach (StageStatSo stat in allStages)
           {
               _stageSos[index] = new StageContainer(stat, 1);
               index++;
           }
   
           if (_stageSos.Count == 0)
           {
               throw new System.Exception("GearStatSo ScriptableObjects Load FAIL");
           }
   
       }
   
       public StageStatSo GetStageSoWithID(int stageId)
       {
           return _stageSos[stageId].StageSo;
       }
       #endregion
   
       #region Event
   
       private void OnEnable()
       {
           GameSceneManager.Instance.EventGameScene.OnGameEnded += Event_GameEnded;
       }
   
       private void OnDisable()
       {
           GameSceneManager.Instance.EventGameScene.OnGameEnded -= Event_GameEnded;
       }
       private void Event_GameEnded()
       {
           CheckGameResult();
       }
       #endregion
   
   #if UNITY_EDITOR
       protected override void OnBindField()
       {
           base.OnBindField();
           _memoryController = GameObject.FindAnyObjectByType<MemoryController>();
           _stageTimeController = GameObject.FindAnyObjectByType<StageTimeController>();
       }
   #endif
   }
   
*/
#endregion