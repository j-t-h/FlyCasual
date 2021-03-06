﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actions;
using ActionsList;
using Arcs;
using Movement;
using Ship;
using Upgrade;

namespace Ship.SecondEdition.EscapeCraft
{
    public class EscapeCraft : GenericShip
    {
        public EscapeCraft() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "Escape Craft",
                BaseSize.Small,
                Faction.Scum,
                new ShipArcsInfo(ArcType.Front, 2), 2, 2, 2,
                new ShipActionsInfo(
                    new ActionInfo(typeof(FocusAction)),
                    new ActionInfo(typeof(BarrelRollAction)),
                    new ActionInfo(typeof(CoordinateAction), ActionColor.Red)
                ),
                new ShipUpgradesInfo(
                    UpgradeType.Modification,
                    UpgradeType.Crew
                )
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Scum, typeof(AutopilotDrone) }
            };

            ModelInfo = new ShipModelInfo(
                "Escape Craft",
                "Default"
            );

            DialInfo = new ShipDialInfo(
                new ManeuverInfo(ManeuverSpeed.Speed0, ManeuverDirection.Stationary, ManeuverBearing.Stationary, MovementComplexity.Complex),

                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Easy),

                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Complex),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Complex),

                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.KoiogranTurn, MovementComplexity.Complex)
            );

            SoundInfo = new ShipSoundInfo(
                new List<string>()
                {
                    "XWing-Fly1",
                    "XWing-Fly2",
                    "XWing-Fly3"
                },
                "XWing-Laser", 2
            );

            HotacManeuverTable = new AI.EscapeCraftTable();

            ShipIconLetter = 'X';
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CoPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnDocked += CopyPilotAbilityToHost;
            HostShip.OnUndocked += RemovePilotAbilityFromHost;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDocked -= CopyPilotAbilityToHost;
            HostShip.OnUndocked -= RemovePilotAbilityFromHost;
        }

        private void CopyPilotAbilityToHost(GenericShip ship)
        {
            GenericAbility abilityCopy = (GenericAbility) Activator.CreateInstance(HostShip.PilotInfo.AbilityType);
            HostShip.DockingHost.PilotAbilities.Add(abilityCopy);
            abilityCopy.Initialize(HostShip.DockingHost);
        }

        private void RemovePilotAbilityFromHost(GenericShip ship)
        {
            GenericAbility copiedAbility = HostShip.DockingHost.PilotAbilities.First(n => n.GetType() == HostShip.PilotInfo.AbilityType);
            copiedAbility.DeactivateAbility();
            HostShip.DockingHost.PilotAbilities.Remove(copiedAbility);
        }
    }
}
