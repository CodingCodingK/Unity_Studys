using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Play;
        AddTransition(Transition.PauseButtonClick,StateID.Menu);
    }

    public override void DoBeforeEntering()
    {
        ctrl.view.ShowGameUI();
        ctrl.cameraManager.ZoomIn();
        
        ctrl.gameManager.StartGame();
    }
    
    public override void DoBeforeLeaving()
    {
        ctrl.view.ShowRestartButton();
        ctrl.view.HideGameUI();
        
        ctrl.gameManager.PauseGame();
    }

    public void OnPauseButtonClick()
    {
        ctrl.audioManager.PlayAudio("cursor");
        fsm.PerformTransition(Transition.PauseButtonClick);
    }
}
