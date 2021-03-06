﻿using Ship;
using Upgrade;
using System.Collections.Generic;
using UnityEngine;
using BoardTools;
using Arcs;

namespace UpgradesList.FirstEdition
{
    public class TailGunner : GenericUpgrade
    {
        public TailGunner() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Tail Gunner",
                UpgradeType.Crew,
                cost: 2,
                isLimited: true,
                abilityType: typeof(Abilities.FirstEdition.TailGunnerAbility)
            );
        }        
    }
}

namespace Abilities.FirstEdition
{
    public class TailGunnerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += AddTailGunnerAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker -= AddTailGunnerAbility;
        }

        public void AddTailGunnerAbility()
        {
            if (Selection.ThisShip.ShipId == HostShip.ShipId)
            {
                //Gather shot info to determine if in rear arc
                ShotInfo shotInfo = new ShotInfo(Combat.Attacker, Combat.Defender, Combat.Attacker.PrimaryWeapons);
                //make sure card requirements are met.
                //can't reduce defender agility past 0 and must be aux arc
                if (Combat.Defender.State.Agility != 0 && shotInfo.InArcByType(ArcType.Rear))
                {
                    Messages.ShowInfo("Tail Gunner: " + Combat.Defender.PilotInfo.PilotName + "'s Agility is decreased by 1");
                    Combat.Defender.Tokens.AssignCondition(typeof(Conditions.TailGunnerCondition));
                    Combat.Defender.ChangeAgilityBy(-1);
                    Combat.Defender.OnAttackFinish += RemoveTailGunnerAbility;
                }
            }
        }

        public void RemoveTailGunnerAbility(GenericShip ship)
        {
            Messages.ShowInfo("Tail Gunner: " + Combat.Defender.PilotInfo.PilotName + "'s Agility has been restored");
            Combat.Defender.Tokens.RemoveCondition(typeof(Conditions.TailGunnerCondition));
            ship.ChangeAgilityBy(+1);
            ship.OnAttackFinish -= RemoveTailGunnerAbility;
        }
    }
}

namespace Conditions
{
    public class TailGunnerCondition : Tokens.GenericToken
    {
        public TailGunnerCondition(GenericShip host) : base(host)
        {
            Name = ImageName = "Debuff Token";
            Temporary = false;
            Tooltip = new UpgradesList.FirstEdition.TailGunner().ImageUrl;
        }
    }
}