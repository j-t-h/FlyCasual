﻿using Arcs;
using Ship;
using SubPhases;
using UnityEngine;

namespace Abilities
{
    public class RollDiceAction : AbilityPart
    {
        private GenericAbility Ability;

        public DiceKind DiceType { get; }
        public AbilityPart OnCrit { get; }
        public AbilityPart OnHit { get; }

        public RollDiceAction(DiceKind diceType, AbilityPart onCrit, AbilityPart onHit)
        {
            DiceType = diceType;
            OnCrit = onCrit;
            OnHit = onHit;
        }

        public override void DoAction(GenericAbility ability)
        {
            Ability = ability;

            Debug.Log("Dice Roll action");

            Triggers.FinishTrigger();
        }
    }
}
