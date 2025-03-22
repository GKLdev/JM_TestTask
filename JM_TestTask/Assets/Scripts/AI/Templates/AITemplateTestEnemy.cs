using GDTUtils;
using Modules.AISensors.Player;
using Modules.AISensors_Public;
using Modules.AITemplate_Public;
using Modules.ReferenceDb_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "AITemplateTestEnemy", menuName = "AI/Template/AITemplateTestEnemy")]
public class AITemplateTestEnemy : AITemplateBase
{
    // x - min, y - max
    public Vector2  sideStepDistance;
    public Vector3  testStrafeAxis          = Vector3.forward;
    public float    maxStrafeDist           = 5f;
    public float    changeDirectionDelay    = 1f;

    public TimeLayerType timeLayer = TimeLayerType.World;

    private TemplateState   currentState        = TemplateState.None;
    private Vector3         distanceToTarget    = Vector3.zero;
    private Vector3         myPosition          = Vector3.zero;
    private Vector3         initialPos          = Vector3.zero;

    private int     cooldownId;

    private IAISensorPlayer sensorPlayer;
    private ITimeManager    timeMgr;

    private bool cooldownStarted = false;

    // *****************************
    // OnInit 
    // *****************************
    protected override void OnInit()
    {
        base.OnInit();

        bool error = !brain.GetSensor(AISensorsTable.Player, out sensorPlayer);
        if (error)
        {
            Debug.Assert(false, $"Template={this} failed to get Player sensor!");
        }

        timeMgr     = diContainer.Resolve<ITimeManager>();
        cooldownId  = timeMgr.AddCooldown(changeDirectionDelay, timeLayer);
    }

    // *****************************
    // OnTemplateActivated 
    // *****************************
    protected override void OnTemplateActivated()
    {
        base.OnTemplateActivated();
        initialPos = character.P_Controller.P_Position;
    }

    // *****************************
    // OnUpdate 
    // *****************************
    protected override void OnUpdate(float _delta)
    {
        base.OnUpdate(_delta);

        HandleState();
        CalculateValues();

        switch (currentState)
        {
            case TemplateState.None:
                break;
            case TemplateState.Moving:
                MoveBehaviour();
                break;
            case TemplateState.Dead:
                Deadbehaviour();
                break;
            default:
                break;
        }
    }

    // *****************************
    // HandleState 
    // *****************************
    void HandleState()
    {
        bool iAmDead = character.IsDead();

        currentState = TemplateState.None;

        if (iAmDead)
        {
            currentState = TemplateState.Dead;
        }
        else
        {
            currentState = TemplateState.Moving;
        }

    }

    // *****************************
    // CalculateValues 
    // *****************************
    void CalculateValues()
    {
        myPosition = character.P_Controller.P_Position;
        distanceToTarget = GetDistanceToTarget();
    }

    // *****************************
    // MoveBehaviour 
    // *****************************
    void MoveBehaviour()
    {
        bool switchDirection = !cooldownStarted || timeMgr.CheckCooldownPassed(cooldownId);
        if (switchDirection)
        {
            int     randomNumber    = GDTRandom.generalRng.Next(1, 100);
            float   factor          = (50f - (float)randomNumber) / 100f;

            factor = Mathf.Sign(factor) * (sideStepDistance.x + (sideStepDistance.y - sideStepDistance.x) * Mathf.Abs(factor));

            Vector3 targetPos = initialPos + testStrafeAxis.normalized * factor;

            character.P_Controller.MoveToTarget(targetPos);
            timeMgr.ResetCooldown(cooldownId, changeDirectionDelay);
            cooldownStarted = true;
        }

        character.P_GameObjectAccess.transform.forward = distanceToTarget.normalized;
    }

    // *****************************
    // Deadbehaviour 
    // *****************************
    void Deadbehaviour()
    {

    }

    // *****************************
    //  GetDistanceToTarget
    // *****************************
    Vector3 GetDistanceToTarget()
    {
        Vector3 result = result = sensorPlayer.GetPosition() - myPosition;

        return result;
    }

    // *****************************
    // OnResetTemplate 
    // *****************************
    protected override void OnResetTemplate()
    {
        base.OnResetTemplate();

        currentState        = TemplateState.None;
        myPosition          = Vector3.zero;
        distanceToTarget    = Vector3.zero;
        cooldownStarted     = false;
    }

    // *****************************
    // TemplateState 
    // *****************************
    private enum TemplateState
    {
        None = 0,
        Moving,
        Dead
    }
}
