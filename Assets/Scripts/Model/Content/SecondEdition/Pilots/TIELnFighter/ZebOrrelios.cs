﻿namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class ZebOrrelios : TIELnFighter
        {
            public ZebOrrelios() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Zeb\" Orrelios",
                    2,
                    24,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.ZebOrreliosPilotAbility),
                    factionOverride: Faction.Rebel,
                    seImageNumber: 49
                );

                PilotNameCanonical = "zeborrelios-tielnfighter";

                ModelInfo.ModelName = "TIE Fighter Rebel";
                ModelInfo.SkinName = "Rebel";
            }
        }
    }
}