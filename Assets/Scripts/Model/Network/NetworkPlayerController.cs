﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class NetworkPlayerController : NetworkBehaviour {

    private void Start()
    {
        if (isLocalPlayer) Network.CurrentPlayer = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public bool IsServer
    {
        get { return isServer; }
    }

    // TESTS

    [Command]
    public void CmdTest()
    {
        RpcTest();
    }

    [ClientRpc]
    private void RpcTest()
    {
        Messages.ShowInfo("Network test\nLocal: " + isLocalPlayer + "; Client: " + isClient + "; Server: " + isServer);
    }

    [Command]
    public void CmdCallBacksTest()
    {
        new NetworkExecuteWithCallback(CmdRosterTest, CmdShowVariable);
    }

    [Command]
    public void CmdRosterTest()
    {
        Network.AllShipNames = "";
        RpcRosterTest();
    }

    [ClientRpc]
    private void RpcRosterTest()
    {
        string text = (isServer) ? "Hello from server" : "Hello from client";
        text += "\nMy first ship is " + RosterBuilder.TestGetNameOfFirstShipInRoster();
        Network.UpdateAllShipNames(RosterBuilder.TestGetNameOfFirstShipInRoster() + "\n");
        Network.ShowMessage(text);

        Network.FinishTask();
    }

    [Command]
    public void CmdUpdateAllShipNames(string text)
    {
        Network.AllShipNames += text;
    }

    [Command]
    public void CmdShowVariable()
    {
        RpcShowVariable(Network.AllShipNames);
    }

    [ClientRpc]
    private void RpcShowVariable(string text)
    {
        Messages.ShowInfo(text);
    }

    // START OF BATTLE

    [Command]
    public void CmdStartNetworkGame()
    {
        new NetworkExecuteWithCallback(CmdLoadBattleScene, CmdStartBattle);
    }

    [Command]
    public void CmdLoadBattleScene()
    {
        RpcLoadBattleScene();
    }

    [ClientRpc]
    public void RpcLoadBattleScene()
    {
        RosterBuilder.HideNetworkManagerHUD();
        RosterBuilder.ShowOpponentSquad();
        RosterBuilder.LoadBattleScene();
    }

    [Command]
    public void CmdStartBattle()
    {
        RpcStartBattle();
    }

    [ClientRpc]
    public void RpcStartBattle()
    {
        Global.StartBattle();
    }

    // DECISIONS

    [Command]
    public void CmdTakeDecision(string decisionName)
    {
        RpcTakeDecision(decisionName);
    }

    [ClientRpc]
    private void RpcTakeDecision(string decisionName)
    {
        (Phases.CurrentSubPhase as SubPhases.DecisionSubPhase).ExecuteDecision(decisionName);
    }

    // SETUP

    [Command]
    public void CmdConfirmShipSetup(int shipId, Vector3 position, Vector3 angles)
    {
        RpcConfirmShipSetup(shipId, position, angles);
    }

    [ClientRpc]
    private void RpcConfirmShipSetup(int shipId, Vector3 position, Vector3 angles)
    {
        (Phases.CurrentSubPhase as SubPhases.SetupSubPhase).ConfirmShipSetup(shipId, position, angles);
    }

    // ASSIGN MANEUVER

    [Command]
    public void CmdAssignManeuver(int shipId, string maneuverCode)
    {
        RpcAssignManeuver(shipId, maneuverCode);
    }

    [ClientRpc]
    private void RpcAssignManeuver(int shipId, string maneuverCode)
    {
        // Temporary
        GameManagerScript Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        Game.Movement.AssignManeuver(shipId, maneuverCode);
    }

    // NEXT BUTTON
    
    [Command]
    public void CmdNextButtonEffect()
    {
        RpcNextButtonEffect();
    }

    [ClientRpc]
    private void RpcNextButtonEffect()
    {
        UI.NextButtonEffect();
    }

    // SKIP BUTTON

    [Command]
    public void CmdSkipButtonEffect()
    {
        RpcSkipButtonEffect();
    }

    [ClientRpc]
    private void RpcSkipButtonEffect()
    {
        UI.SkipButtonEffect();
    }

    // SHIP MOVEMENT

    [Command]
    public void CmdPerformStoredManeuver(int shipId)
    {
        new NetworkExecuteWithCallback(
            delegate { CmdLaunchStoredManeuver(shipId); },
            delegate { CmdFinishManeuver(shipId); }
        );
    }

    [Command]
    public void CmdLaunchStoredManeuver(int shipId)
    {
        RpcLaunchStoredManeuver(shipId);
    }

    [ClientRpc]
    private void RpcLaunchStoredManeuver(int shipId)
    {
        // Temporary
        GameManagerScript Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        Game.Movement.PerformStoredManeuver(shipId);
    }

    [Command]
    public void CmdFinishManeuver(int shipId)
    {
        RpcFinishManeuver(shipId);
    }

    [ClientRpc]
    private void RpcFinishManeuver(int shipId)
    {
        Phases.FinishSubPhase(typeof(SubPhases.MovementExecutionSubPhase));
    }

    // DECLARE ATTACK TARGET

    [Command]
    public void CmdDeclareTarget(int attackerId, int defenderId)
    {
        RpcDeclareTarget(attackerId, defenderId);
    }

    [ClientRpc]
    private void RpcDeclareTarget(int attackerId, int defenderId)
    {
        Combat.DeclareTarget(attackerId, defenderId);
    }

    // CONFIRM DICE RESULTS MODIFICATIONS

    [Command]
    public void CmdConfirmDiceResults()
    {
        RpcConfirmDiceResults();
    }

    [ClientRpc]
    private void RpcConfirmDiceResults()
    {
        Combat.ConfirmDiceResultsClient();
    }

    // CONFIRM DICE ROLL CHECK

    [Command]
    public void CmdConfirmDiceRollCheckResults()
    {
        new NetworkExecuteWithCallback(CmdShowDiceRollCheckConfirmButton, CmdConfirmDiceRerollCheckResults);
    }

    [Command]
    private void CmdShowDiceRollCheckConfirmButton()
    {
        RpcShowDiceRollCheckConfirmButton();
    }

    [ClientRpc]
    private void RpcShowDiceRollCheckConfirmButton()
    {
        (Phases.CurrentSubPhase as SubPhases.DiceRollCheckSubPhase).ShowDiceRollCheckConfirmButton();
    }

    [Command]
    private void CmdConfirmDiceRerollCheckResults()
    {
        RpcConfirmDiceRerollCheckResults();
    }

    [ClientRpc]
    private void RpcConfirmDiceRerollCheckResults()
    {
        (Phases.CurrentSubPhase as SubPhases.DiceRollCheckSubPhase).Confirm();
    }

    // CONFIRM INFORM CRIT WINDOW

    [Command]
    public void CmdCallInformCritWindow()
    {
        new NetworkExecuteWithCallback(CmdShowInformCritWindow, CmdHideInformCritWindow);
    }

    [Command]
    private void CmdShowInformCritWindow()
    {
        RpcShowInformCritWindow();
    }

    [ClientRpc]
    private void RpcShowInformCritWindow()
    {
        InformCrit.ShowPanelVisible();
    }

    [Command]
    private void CmdHideInformCritWindow()
    {
        RpcHideInformCritWindow();
    }

    [ClientRpc]
    private void RpcHideInformCritWindow()
    {
        Triggers.FinishTrigger();
    }

    // DICE ROLL SYNC

    [Command]
    public void CmdSyncDiceResults()
    {
        new NetworkExecuteWithCallback(
            CmdSendDiceRollResultsToClients,
            CmdCalculateDiceRoll
        );
    }

    [Command]
    public void CmdSendDiceRollResultsToClients()
    {
        RpcSendDiceRollResultsToClients(DiceRoll.CurrentDiceRoll.ResultsArray);
    }

    [ClientRpc]
    private void RpcSendDiceRollResultsToClients(DieSide[] dieSideResults)
    {
        Network.CompareDiceSidesAgainstServer(dieSideResults);
    }

    [Command]
    public void CmdCalculateDiceRoll()
    {
        RpcCalculateDiceRoll();
    }

    [ClientRpc]
    private void RpcCalculateDiceRoll()
    {
        if (DiceRoll.CurrentDiceRoll.CheckType == DiceRollCheckType.Combat)
        {
            (Phases.CurrentSubPhase as SubPhases.DiceRollCombatSubPhase).CalculateDice();
        }
        else if (DiceRoll.CurrentDiceRoll.CheckType == DiceRollCheckType.Check)
        {
            (Phases.CurrentSubPhase as SubPhases.DiceRollCheckSubPhase).CalculateDice();
        }

    }

    // MESSAGES

    [Command]
    public void CmdShowMessage(string text)
    {
        RpcShowMessage(text);
    }

    [ClientRpc]
    private void RpcShowMessage(string text)
    {
        Messages.ShowInfo(text);
    }

    // CALLBACKS

    [Command]
    public void CmdFinishTask()
    {
        Network.ServerFinishTask();
    }
}
