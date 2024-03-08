namespace RawDeal.Status;

public enum PlayerStatus
{
    //NextPlayDamage
    NextGrappleIsPlus4D, NextStrikeIsPlus5D, NextManeuverIsPlus2D, NextStrikeIsPlus2D, NextManeuverIsPlus4D, NextManeuverIsPlus6D,
    OpponentReversalIsPlus2D, OpponentReversalIsPlus6D,
    //NextPlayFortitude
    NextGrapplesReversalIsPlus8F, NextManeuverReversalIsPlus12F, NextManeuverReversalIsPlus20F,
    DiversionWasPlayed, StaggerWasPlayed,
    //Turn Bonus
    ManeuversInTheTurnArePlus3D, StrikesInTheTurnArePlus1D, ManeuversInTheTurnArePlus5D, ManeuversInTheTurnArePlus2D,
    ReversalsInTheTurnArePlus20F, ReversalsInTheTurnArePlus15F, ReversalsInTheTurnArePlus25F, AyatollahWasPlayed,
    //LastDamage
    LastDamageGreaterThan4, LastDamageGreaterThan5,
    //LastCardPlayed
    IrishWhipWasPlayed, KickWasPlayed, KanesChokeSlamWasPlayed, StrikeWasPlayed, ReversalWasPlayed, ManeuverWasPlayed,
    //Game
    MrSockoInRingArea, Dead, Normal
}