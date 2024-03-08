using RawDeal.Cards;

namespace RawDeal.Status;

public class State
{
    public PlayerStatus NextPlayDamage = PlayerStatus.Normal;
    public PlayerStatus NextPlayFortitude = PlayerStatus.Normal;
    public PlayerStatus LastCardPlayed = PlayerStatus.Normal;
    public List<PlayerStatus> Turn = new ();
    public PlayerStatus LastDamage = PlayerStatus.Normal;
    public List<PlayerStatus> Game = new ();

    public void CheckPreviousInformation(State playerState, Card card)
    {
        CheckNextPlayDamage(playerState, card);
        CheckNextPlayFortitude(playerState, card);
        CheckTurnDamageBonus(playerState, card);
        LastCardPlayed = playerState.LastCardPlayed;
        LastDamage = playerState.LastDamage;
        Game = playerState.Game;
    }
    // //NextPlayDamage
    // NextGrappleIsPlus4D, NextStrikeIsPlus5D, NextManeuverIsPlus2D, NextStrikeIsPlus2D, NextManeuverIsPlus4D, NextManeuverIsPlus6D,
    // OpponentReversalIsPlus2D, OpponentReversalIsPlus6D,
    // //NextPlayFortitude
    // NextGrapplesReversalIsPlus8F, NextManeuverReversalIsPlus12F, NextManeuverReversalIsPlus20F,
    // DiversionWasPlayed, StaggerWasPlayed,
    // //Turn Bonus
    // ManeuversInTheTurnArePlus3D, StrikesInTheTurnArePlus1D, ManeuversInTheTurnArePlus5D, ManeuversInTheTurnArePlus2D,
    // ReversalsInTheTurnArePlus20F, ReversalsInTheTurnArePlus15F, ReversalsInTheTurnArePlus25F, AyatollahWasPlayed,
    // //LastDamage
    // LastDamageGreaterThan4, LastDamageGreaterThan5,
    // //LastCardPlayed
    // IrishWhipWasPlayed, KickWasPlayed, KanesChokeSlamWasPlayed, StrikeWasPlayed, ReversalWasPlayed, ManeuverWasPlayed,
    // //Game
    // MrSockoInRingArea, Dead, Normal

    private void CheckNextPlayDamage(State playerState, Card card)
    {
        NextPlayDamage = playerState.NextPlayDamage switch
        {
            { } when playerState.NextPlayDamage == PlayerStatus.NextManeuverIsPlus2D && card.Types.Contains("Maneuver") => PlayerStatus.NextManeuverIsPlus2D,
            { } when playerState.NextPlayDamage == PlayerStatus.NextStrikeIsPlus2D && card.Subtypes.Contains("Strike") => PlayerStatus.NextStrikeIsPlus2D,
            { } when playerState.NextPlayDamage == PlayerStatus.NextGrappleIsPlus4D && card.Subtypes.Contains("Grapple") => PlayerStatus.NextGrappleIsPlus4D,
            { } when playerState.NextPlayDamage == PlayerStatus.NextStrikeIsPlus5D && card.Subtypes.Contains("Strike") => PlayerStatus.NextStrikeIsPlus5D,
            { } when playerState.NextPlayDamage == PlayerStatus.NextManeuverIsPlus4D && card.Types.Contains("Maneuver") => PlayerStatus.NextManeuverIsPlus4D,
            { } when playerState.NextPlayDamage == PlayerStatus.NextManeuverIsPlus6D && card.Types.Contains("Maneuver") => PlayerStatus.NextManeuverIsPlus6D,
            { } when playerState.NextPlayDamage == PlayerStatus.OpponentReversalIsPlus2D && card.Types.Contains("Reversal") => PlayerStatus.OpponentReversalIsPlus2D,
            { } when playerState.NextPlayDamage == PlayerStatus.OpponentReversalIsPlus6D && card.Types.Contains("Reversal") => PlayerStatus.OpponentReversalIsPlus6D,
            _ => PlayerStatus.Normal
        };
    }
    
    private void CheckNextPlayFortitude(State playerState, Card card)
    {
        NextPlayFortitude = playerState.NextPlayFortitude switch
        {
            { } when playerState.NextPlayFortitude == PlayerStatus.NextGrapplesReversalIsPlus8F && card.Subtypes.Contains("Grapple") => PlayerStatus.NextGrapplesReversalIsPlus8F,
            { } when playerState.NextPlayFortitude == PlayerStatus.NextManeuverReversalIsPlus12F && card.Types.Contains("Maneuver") => PlayerStatus.NextManeuverReversalIsPlus12F,
            { } when playerState.NextPlayFortitude == PlayerStatus.NextManeuverReversalIsPlus20F && card.Types.Contains("Maneuver") => PlayerStatus.NextManeuverReversalIsPlus20F,
            { } when playerState.NextPlayFortitude == PlayerStatus.DiversionWasPlayed && card.Types.Contains("Maneuver") => PlayerStatus.DiversionWasPlayed,
            { } when playerState.NextPlayFortitude == PlayerStatus.StaggerWasPlayed && card.Types.Contains("Maneuver") => PlayerStatus.StaggerWasPlayed,
            _ => PlayerStatus.Normal
        };
    }

    private void CheckTurnDamageBonus(State playerState, Card card)
    {
        for (int i = 0; i < playerState.Turn.Count; i++)
        {
            Turn.Add(playerState.Turn[i] switch
            {
                { } when playerState.Turn[i] == PlayerStatus.ManeuversInTheTurnArePlus3D && card.Types.Contains("Maneuver") => PlayerStatus.ManeuversInTheTurnArePlus3D,
                { } when playerState.Turn[i] == PlayerStatus.StrikesInTheTurnArePlus1D && card.Subtypes.Contains("Strike") => PlayerStatus.StrikesInTheTurnArePlus1D,
                { } when playerState.Turn[i] == PlayerStatus.ManeuversInTheTurnArePlus5D && card.Types.Contains("Maneuver") => PlayerStatus.ManeuversInTheTurnArePlus5D,
                { } when playerState.Turn[i] == PlayerStatus.ManeuversInTheTurnArePlus2D && card.Types.Contains("Maneuver") => PlayerStatus.ManeuversInTheTurnArePlus2D,
                { } when playerState.Turn[i] == PlayerStatus.ReversalsInTheTurnArePlus20F && card.Types.Contains("Maneuver") => PlayerStatus.ReversalsInTheTurnArePlus20F,
                { } when playerState.Turn[i] == PlayerStatus.ReversalsInTheTurnArePlus15F && card.Types.Contains("Maneuver") => PlayerStatus.ReversalsInTheTurnArePlus15F,
                { } when playerState.Turn[i] == PlayerStatus.ReversalsInTheTurnArePlus25F && card.Types.Contains("Maneuver") => PlayerStatus.ReversalsInTheTurnArePlus25F,
                { } when playerState.Turn[i] == PlayerStatus.AyatollahWasPlayed => PlayerStatus.AyatollahWasPlayed,
                _ => PlayerStatus.Normal
            });
        }
    }

    public int GetExtraDamage()
    {
        int extraDamage = 0;
        extraDamage += NextPlayDamage switch
        {
            { } when NextPlayDamage == PlayerStatus.NextManeuverIsPlus2D => 2,
            { } when NextPlayDamage == PlayerStatus.NextStrikeIsPlus2D => 2,
            { } when NextPlayDamage == PlayerStatus.NextGrappleIsPlus4D => 4,
            { } when NextPlayDamage == PlayerStatus.NextStrikeIsPlus5D => 5,
            { } when NextPlayDamage == PlayerStatus.NextManeuverIsPlus4D => 4,
            { } when NextPlayDamage == PlayerStatus.NextManeuverIsPlus6D => 6,
            _ => 0
        };
        extraDamage += Turn.Count(p => p == PlayerStatus.StrikesInTheTurnArePlus1D);
        extraDamage += 2 * Turn.Count(p => p == PlayerStatus.ManeuversInTheTurnArePlus2D);
        extraDamage += 3 * Turn.Count(p => p == PlayerStatus.ManeuversInTheTurnArePlus3D);
        extraDamage += 5 * Turn.Count(p => p == PlayerStatus.ManeuversInTheTurnArePlus5D);
        extraDamage += Game.Count(p => p == PlayerStatus.MrSockoInRingArea);

        return extraDamage;
    }

    public void CheckMrSockoInRingArea(Player player)
    {
        byte MrSockoInRingArea = 0;
        byte MrSockoInState = 0;
        foreach (Card card in player.RingArea.Cards)
            if (card.Title == "Mr. Socko")
                MrSockoInRingArea++;
        foreach (PlayerStatus status in Game)
            if (status == PlayerStatus.MrSockoInRingArea)
                MrSockoInState++;
        if (MrSockoInRingArea < MrSockoInState)
            Game.Remove(PlayerStatus.MrSockoInRingArea);
    }

    public void SetLastDamagePlayerState(byte PlayDamage)
    {
        if (PlayDamage >= 5)
            LastDamage = PlayerStatus.LastDamageGreaterThan5;
        else if (PlayDamage >= 4)
            LastDamage = PlayerStatus.LastDamageGreaterThan4;
        else
            LastDamage = PlayerStatus.Normal;
    }

    public void ClearStateAfterPlay()
    {
        NextPlayDamage = PlayerStatus.Normal;
        NextPlayFortitude = PlayerStatus.Normal;
        LastCardPlayed = PlayerStatus.Normal;
    }
    
    public void ClearStateAfterTurn()
    {
        NextPlayDamage = PlayerStatus.Normal;
        NextPlayFortitude = PlayerStatus.Normal;
        LastCardPlayed = PlayerStatus.Normal;
        LastDamage = PlayerStatus.Normal;
        Turn.Clear();
    }

    public bool IsPlayerDead()
        => Game.Contains(PlayerStatus.Dead);
}