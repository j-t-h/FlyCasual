﻿using Ship;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ2AWing
    {
        public class LuloLampar : RZ2AWing
        {
            public LuloLampar() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "L'ulo L'ampar",
                    5,
                    42,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.LuloLamparAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );

                ModelInfo.SkinName = "Red";

                ImageUrl = "https://sb-cdn.fantasyflightgames.com/card_images/en/e15d3e2a2fc082b95a64a83df0c96f7f.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class LuloLamparAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += LuloLamparAbilityAtkPilotAbility;
            HostShip.AfterGotNumberOfDefenceDice += LuloLamparAbilityDefPilotAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= LuloLamparAbilityAtkPilotAbility;
            HostShip.AfterGotNumberOfDefenceDice -= LuloLamparAbilityDefPilotAbility;
        }

        private void LuloLamparAbilityAtkPilotAbility(ref int result)
        {
            if (HostShip.IsStressed && Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " is stressed and gains +1 attack die");
                result++;
            }
        }

        private void LuloLamparAbilityDefPilotAbility(ref int result)
        {
            if (HostShip.IsStressed)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " is stressed and gains -1 defense die");
                result--;
            }
        }
    }
}