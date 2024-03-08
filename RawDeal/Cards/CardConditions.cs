using System.Reflection.Metadata;
using RawDeal.Status;

namespace RawDeal.Cards;

public class CardConditions
{
    public bool HasMinimumNumberOfCardsInHand(Player player, byte minCardsInHand)
        => player.GetNumberOfCardsInTheHand() >= minCardsInHand;

    public bool LastManeuverHadAMinimalDamage(Player player, byte minDamage)
    {
        if (player.State.LastDamage == PlayerStatus.LastDamageGreaterThan4 && minDamage <= 4)
            return true;
        if (player.State.LastDamage == PlayerStatus.LastDamageGreaterThan5 && minDamage <= 5)
            return true;
        return false;
    }

    public bool OpponentsPlayHadAMaximumDamage(Play opponentsPlay, byte maxDamage)
        => opponentsPlay.PlayDamage <= maxDamage;

    public bool OpponentsPlayIsASpecificCard(Play opponentsPlay, string cardName)
        => opponentsPlay.Card.Title == cardName;
    
    public bool OpponentsPlayHasASpecificSubtype(Play opponentsPlay, string subtype)
        => opponentsPlay.Card.Subtypes.Contains(subtype) && opponentsPlay.PlayedAs == "MANEUVER";

    public bool OpponentsPlayHasASpecificType(Play opponentsPlay, string type)
        => opponentsPlay.PlayedAs == type;

    public bool OpponentsCardIsIrreversible(Play opponentsPlay, string fromWhere)
    {
        List<string> irreversibles = new List<string> { "Tree of Woe", "Austin Elbow Smash", "Leaping Knee to the Face"};
        bool irreversibleByDiversion = opponentsPlay.PrevInfo.NextPlayFortitude == PlayerStatus.DiversionWasPlayed && opponentsPlay.PlayedAs == "MANEUVER";
        bool irreversibleByStagger = opponentsPlay.PrevInfo.NextPlayFortitude == PlayerStatus.StaggerWasPlayed && opponentsPlay.PlayDamage <= 7 && opponentsPlay.PlayedAs == "MANEUVER";
        bool irreversibleByAyatollah = opponentsPlay.PrevInfo.Turn.Contains(PlayerStatus.AyatollahWasPlayed) && fromWhere == "fromDeck";
        return irreversibles.Contains(opponentsPlay.Card.Title) || irreversibleByDiversion || irreversibleByStagger || irreversibleByAyatollah;
    }

    public bool LastPlayWasAIrishWhip(Player player)
        => player.State.LastCardPlayed == PlayerStatus.IrishWhipWasPlayed;

    public bool OpponentsPlayWasAfterAnIrishWhip(Play opponentsPlay)
        => opponentsPlay.PrevInfo.LastCardPlayed == PlayerStatus.IrishWhipWasPlayed && opponentsPlay.PlayedAs == "MANEUVER";

    public bool ItIsReversingFromTheHand(string fromWhere)
        => fromWhere == "fromHand";

    public bool PlayerHasAMinumumNumberOfCardsInTheHand(Player player, byte minCardsInHand)
        => player.GetNumberOfCardsInTheHand() >= minCardsInHand;

    public bool ThereIsASpecificCardInTheRing(Player player, string cardName)
    {
        foreach (Card card in player.RingArea.Cards)
            if (card.Title == cardName)
                return true;
        return false;
    }

    public bool LastPlayWasASpecificCardToDecreaseFortitude(Player player, PlayerStatus playerStatus, Card card, string cardName)
        => player.State.LastCardPlayed == playerStatus && card.Title == cardName;

    public byte GetFortitudeRequiredToPlayReversal(Play opponentsPlay, Card card)
    {
        byte fortitudeRequired = byte.Parse(card.Fortitude);
        if (opponentsPlay.PrevInfo.NextPlayFortitude == PlayerStatus.NextGrapplesReversalIsPlus8F)
            fortitudeRequired += 8;
        else if (opponentsPlay.PrevInfo.NextPlayFortitude == PlayerStatus.NextManeuverReversalIsPlus12F)
            fortitudeRequired += 12;
        else if (opponentsPlay.PrevInfo.NextPlayFortitude == PlayerStatus.NextManeuverReversalIsPlus20F)
            fortitudeRequired += 20;
        if (opponentsPlay.PrevInfo.Turn.Contains(PlayerStatus.ReversalsInTheTurnArePlus15F))
            fortitudeRequired += 15;
        if (opponentsPlay.PrevInfo.Turn.Contains(PlayerStatus.ReversalsInTheTurnArePlus20F))
            fortitudeRequired += 20;
        if (opponentsPlay.PrevInfo.Turn.Contains(PlayerStatus.ReversalsInTheTurnArePlus25F))
            fortitudeRequired += 25;
        return fortitudeRequired;
    }

    public byte GetFortitudeRequiredToPlayCard(Player player, Card card, string typeOfPlay)
    {
        if (card.Title == "Undertaker's Tombstone Piledriver" && typeOfPlay == "Action")
            return 0;
        byte fortitudeRequired = byte.Parse(card.Fortitude);
        if (LastPlayWasASpecificCardToDecreaseFortitude(player, PlayerStatus.KickWasPlayed, card, "Stone Cold Stunner"))
            fortitudeRequired -= 6;
        if (LastPlayWasASpecificCardToDecreaseFortitude(player, PlayerStatus.KanesChokeSlamWasPlayed, card, "Kane's Tombstone Piledriver"))
            fortitudeRequired -= 6;
        return fortitudeRequired;
    }

    public bool OpponentsFortitudRatingIsHigherThanThePlayers(Player player, Player opponent)
        => opponent.FortitudeRating() > player.FortitudeRating();

    public bool LastPlayWasOfASpecificType(Player player, string type)
    {
        PlayerStatus lastCard = player.State.LastCardPlayed;
        if (type == "Maneuver")
            return lastCard == PlayerStatus.ManeuverWasPlayed || lastCard == PlayerStatus.StrikeWasPlayed || lastCard == PlayerStatus.KickWasPlayed;
        else if (type == "Strike")
            return lastCard == PlayerStatus.StrikeWasPlayed;
        return false;
    }
}