using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public enum CombatState { Waiting, Ready, Drawing };

    public CombatState state;

    public static CombatScript combat;

    public BowHandler bowBrain;
    public BowButtonHandler letterBrain;

    private void Awake()
    {
        //singleton
        if (combat != null && combat != this)
        {
            Destroy(this);
        }
        else
        {
            combat = this;

        }
    }

    private void OnEnable()
    {
        state = CombatState.Ready;
    }

    private void Update()
    {
        if(state == CombatState.Ready)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //start drawing bow
                bowBrain.StartDraw();
                state = CombatState.Drawing;
            }
        }
        else if (state == CombatState.Drawing)
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                //release bow
                bowBrain.StartRelease();
                state = CombatState.Waiting;
            }
        }
    }
}
