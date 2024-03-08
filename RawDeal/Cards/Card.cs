using RawDealView.Formatters;
using RawDeal.Status;

namespace RawDeal.Cards;

public class Card : IViewableCardInfo
{
    public string Title { get; }
    public List<string> Types { get; }
    public List<string> Subtypes { get; }
    public string Fortitude { get; }
    public string Damage { get; }
    public string StunValue { get; }
    public string CardEffect { get; }

    public Card(string title, List<string> types, List<string> subtypes, string fortitude, string damage, string stunValue, string cardEffect)
    {
        Title = title;
        Types = types;
        Subtypes = subtypes;
        Fortitude = fortitude;
        Damage = damage;
        StunValue = stunValue;
        CardEffect = cardEffect;
    }


    // PLAY METHODS
    public virtual PlayResult PlayThisCardAsAManeuver(Player player, Player opponent, Play play)
    {
        player.PutCardInRingArea(play.IdInHand);
        player.View.SayThatPlayerSuccessfullyPlayedACard();
        player.CardEffects.ApplyCardEffect(player, opponent, play);
        if (player.State.IsPlayerDead())
            return PlayResult.PlayerIsDead;
        return DamageTheOpponent(opponent, play);
    }

    public virtual PlayResult PlayThisCardAsAnAction(Player player, Player opponent, Play play)
    {
        player.View.SayThatPlayerSuccessfullyPlayedACard();
        player.CardEffects.ApplyCardEffect(player, opponent, play);
        return PlayResult.ActionPlayed;
    }

    public virtual PlayResult PlayThisCardAsAReversal(Player player, Player opponent, Play play)
    {
        player.PutCardInRingArea(play.IdInHand);
        player.CardEffects.ApplyCardEffect(player, opponent, play);
        PlayResult result = DamageTheOpponent(opponent, play);

        if (result == PlayResult.OpponentIsDead)   // The opponent in this case, is the player who played the card that was reversed
            return PlayResult.PlayerIsDead;
        return result;
    }
    
    // DAMAGE METHODS
    private PlayResult DamageTheOpponent(Player opponent, Play play)
    {
        if (play.PlayDamage == 0)
            return PlayResult.UnharmedOpponent;
        opponent.View.SayThatSuperstarWillTakeSomeDamage(opponent.SuperStar.Name, play.PlayDamage);
        return TakeTheOpponentsCardsFromHisArsenalToHisRingSide(opponent, play);
    }

    private PlayResult TakeTheOpponentsCardsFromHisArsenalToHisRingSide(Player opponent, Play play)
    {
        for (int i = 0; i < play.PlayDamage; i++)
        {
            if (opponent.GetNumberOfCardsInArsenal() == 0)
                return PlayResult.OpponentIsDead;
            Card card = opponent.TakeACardFromPlayersArsenalToHisRingSide();
            opponent.View.ShowCardOverturnByTakingDamage(Formatter.CardToString(card), i+1, play.PlayDamage);
            
            if (CanThisCardRevertThePlayByDeck(opponent, play, card, i+1))
                return PlayResult.RevertedByDeck;
        }
        return PlayResult.OpponentIsAlive;
    }

    private bool CanThisCardRevertThePlayByDeck(Player opponent, Play play, Card card, int cardTaken)
    {
        if (play.Player.CardConditions.CanThisCardBePlayedAsReversal(opponent, play, card, "fromDeck"))
        {
            opponent.View.SayThatCardWasReversedByDeck(opponent.SuperStar.Name);
            if (cardTaken < play.PlayDamage && int.Parse(StunValue) > 0)
                ApplyStunValueOfTheCard(play.Player);
            return true;
        }
        return false;
    }

    private void ApplyStunValueOfTheCard(Player player)
    {
        int numOfCardsToDraw = player.View.AskHowManyCardsToDrawBecauseOfStunValue(player.SuperStar.Name, int.Parse(StunValue));
        player.View.SayThatPlayerDrawCards(player.SuperStar.Name, numOfCardsToDraw);
        player.TakeCardsFromArsenalToHand((byte)numOfCardsToDraw);
    }
}