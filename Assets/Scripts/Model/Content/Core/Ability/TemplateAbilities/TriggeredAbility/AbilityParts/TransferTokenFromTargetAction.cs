﻿using System;

namespace Abilities
{
    public class TransferTokenFromTargetAction : AbilityPart
    {
        private GenericAbility Ability;
        public Type TokenType { get; }
        public Func<string> GetMessage { get; }

        public TransferTokenFromTargetAction(Type tokenType, Func<string> showMessage)
        {
            TokenType = tokenType;
            GetMessage = showMessage;
        }

        public override void DoAction(GenericAbility ability)
        {
            Ability = ability;
            Messages.ShowInfo(GetMessage());
            Ability.TargetShip.Tokens.TransferToken(TokenType, Ability.HostShip, Triggers.FinishTrigger);
        }
    }
}
