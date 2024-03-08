using System.Data.Common;
using RawDeal.Status;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal.Cards;

public class Effects
{
    public void PlayerDiscardSpecifiedNumberOfHisCards(Player player, byte numberOfCardsToDiscard)
    {
        for (int i = numberOfCardsToDiscard; i > 0; i--)
        {
            if (player.GetNumberOfCardsInTheHand() == 0)
                return;
            int cardToDiscard = player.View.AskPlayerToSelectACardToDiscard(player.GetStringHand(), player.SuperStar.Name, player.SuperStar.Name, i);
            player.DiscardCardsFromHandToRingSide(cardToDiscard);
        }
    }

    public void DiscardThisCard(Player player, byte idInHand)
    {
        player.View.SayThatPlayerMustDiscardThisCard(player.SuperStar.Name, player.GetHand().Cards[idInHand].Title);
        player.DiscardCardsFromHandToRingSide(idInHand);
    }

    public void PlayerDiscardSpecifiedNumberOfOpponentsCards(Player player, Player opponent, byte numberOfCardsToDiscard)
    {
        for (int i = numberOfCardsToDiscard; i > 0; i--)
        {
            if (opponent.GetNumberOfCardsInTheHand() == 0)
                return;
            int cardToDiscard = player.View.AskPlayerToSelectACardToDiscard(opponent.GetStringHand(), opponent.SuperStar.Name, player.SuperStar.Name, i);
            opponent.DiscardCardsFromHandToRingSide(cardToDiscard);
        }
    }

    public void MoveThisCardToRingArea(Player player, byte idInHand)
        => player.PutCardInRingArea(idInHand);

    public void CollateralDamage(Player player, byte damage)     // player damages himself
    {
        player.View.SayThatPlayerDamagedHimself(player.SuperStar.Name, damage);
        player.View.SayThatSuperstarWillTakeSomeDamage(player.SuperStar.Name, damage);

        for (int i = 0; i < damage; i++)
        {
            if (player.GetNumberOfCardsInArsenal() == 0)
            {
                player.View.SayThatPlayerLostDueToSelfDamage(player.SuperStar.Name);
                player.State.Game.Add(PlayerStatus.Dead);
                return;
            }
            Card card = player.TakeACardFromPlayersArsenalToHisRingSide();
            player.View.ShowCardOverturnByTakingDamage(Formatter.CardToString(card), i+1, damage);
        }
    }

    public void MayDrawSpecifiedNumberOfCards(Player player, byte maxCardsToDraw)
    {
        byte numberOfCardsToDraw = (byte) player.View.AskHowManyCardsToDrawBecauseOfACardEffect(player.SuperStar.Name, maxCardsToDraw);
        MustDrawSpecifiedNumberOfCards(player, numberOfCardsToDraw);
    }

    public void MustDrawSpecifiedNumberOfCards(Player player, byte numberOfCardsToDraw)
    {
        byte maxCardsToDraw = AdjustMaximumNumberOfCards(player.GetNumberOfCardsInArsenal(), numberOfCardsToDraw);
        player.View.SayThatPlayerDrawCards(player.SuperStar.Name, maxCardsToDraw);
        player.TakeCardsFromArsenalToHand(maxCardsToDraw);
    }

    public void RecoverDamage(Player player, byte numberOfCardsToRecover)
    {
        byte cardsToRecover = AdjustMaximumNumberOfCards((byte) player.RingSide.Length(), numberOfCardsToRecover);
        for (int i = cardsToRecover; i > 0; i--)
        {
            int cardToRecover = player.View.AskPlayerToSelectCardsToRecover(player.SuperStar.Name, i, player.RingSide.GetStringDeck());
            player.RecoverCardsFromRingSideToArsenal(cardToRecover);
        }
    }

    public void JockeyingForPositionEffect(Player player)
    {
        SelectedEffect effect = player.View.AskUserToSelectAnEffectForJockeyForPosition(player.SuperStar.Name);
        if (effect == SelectedEffect.NextGrapplesReversalIsPlus8F)
            player.State.NextPlayFortitude = PlayerStatus.NextGrapplesReversalIsPlus8F;
        else if (effect == SelectedEffect.NextGrappleIsPlus4D)
            player.State.NextPlayDamage = PlayerStatus.NextGrappleIsPlus4D;
    }

    public void DrawCardsOrForceOpponentToDiscardCards(Player player, Player opponent, Play play, byte numberOfCards)
    {
        SelectedEffect effect = player.View.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(player.SuperStar.Name);
        if (effect == SelectedEffect.DrawCards)
        {
            if (play.Card.Title == "Y2J")
                MayDrawSpecifiedNumberOfCards(player, numberOfCards);
            else
                MustDrawSpecifiedNumberOfCards(player, numberOfCards);
        }
        else if (effect == SelectedEffect.ForceOpponentToDiscard)
            PlayerDiscardSpecifiedNumberOfHisCards(opponent, numberOfCards);
    }

    public void DiscardCardsToObtainCardsFromTheRingside(Player player, byte maxCardsToDiscard)
    {
        byte newMaxCardsToDiscard = AdjustMaximumNumberOfCards(player.GetNumberOfCardsInTheHand(), maxCardsToDiscard);
        if (newMaxCardsToDiscard == 0)
            return;
        int numberOfCardsToDiscard = player.View.AskHowManyCardsToDiscard(player.SuperStar.Name, newMaxCardsToDiscard);
        PlayerDiscardSpecifiedNumberOfHisCards(player, (byte) numberOfCardsToDiscard);
        RecoverCardsToTheHand(player, (byte) numberOfCardsToDiscard);
    }

    public void AddDamageAfterPlayingAMinimumDamageManeuver(Play play, byte minDamage)
    {
        if (play.PrevInfo.LastDamage == PlayerStatus.LastDamageGreaterThan5 && minDamage == 5)
            play.PlayDamage += 5;
    }

    public byte AdjustMaximumNumberOfCards(byte DeckLength, byte maxNumberOfCards)
    {
        if (DeckLength < maxNumberOfCards)
            return DeckLength;
        return maxNumberOfCards;
    }

    public void RecoverCardsToTheHand(Player player, byte numberOfCardsToRecover)
    {
        for (int i = numberOfCardsToRecover; i > 0; i--)
        {
            if (player.RingSide.Length() == 0)
                return;
            int cardToRecover = player.View.AskPlayerToSelectCardsToPutInHisHand(player.SuperStar.Name, i, player.RingSide.GetStringDeck());
            player.RecoverCardFromRingSideToHand(cardToRecover);
        }
    }

    public void DamageOpponent(Player opponent, byte damage)
    {
        opponent.View.SayThatSuperstarWillTakeSomeDamage(opponent.SuperStar.Name, damage);
        for (int i = 0; i < damage; i++)
        {
            if (opponent.GetNumberOfCardsInArsenal() == 0)
            {
                opponent.State.Game.Add(PlayerStatus.Dead);
                return;
            }
            Card card = opponent.TakeACardFromPlayersArsenalToHisRingSide();
            opponent.View.ShowCardOverturnByTakingDamage(Formatter.CardToString(card), i+1, damage);
        }
    }

    public void ReturnCardFromPlayersHandToArsenal(Player player)
    {
        int card = player.View.AskPlayerToReturnOneCardFromHisHandToHisArsenal(player.SuperStar.Name, player.GetStringHand());
        player.ReturnCardFromHandToArsenal(card);
    }

    public void RecoverCardFromRingSideOrArsenal(Player player, string cardName)
    {
        player.View.SayThatPlayerSearchesForTheTargetCardInHisRingside(player.SuperStar.Name, cardName);
        Card cardToRecover;
        if (player.RingSide.CardExistsInThisDeck(cardName))
            cardToRecover = player.RingSide.GetCardFromThisDeck(cardName);
        else
        {
            player.View.SayThatPlayerDidntFindTheCard(player.SuperStar.Name);
            player.View.SayThatPlayerSearchesForTheTargetCardInHisArsenal(player.SuperStar.Name, cardName);
            if (player.GetArsenal().CardExistsInThisDeck(cardName))
                cardToRecover = player.GetArsenal().GetCardFromThisDeck(cardName);
            else
            {
                player.View.SayThatPlayerDidntFindTheCard(player.SuperStar.Name);
                return;
            }
        }
        player.GetHand().AddCardToTheDeck(cardToRecover);
        player.View.SayThatPlayerFoundTheCardAndPutItIntoHisHand(player.SuperStar.Name);
    }

    public void PutCardInTheBottomOfPlayersArsenal(Player player, Play play)
    {
        player.GetHand().MoveSpecificCardToTheBottomOfAnotherDeck(play.IdInHand, player.GetArsenal());
        player.View.SayThatPlayerPutsThisCardAtTheBottomOfHisArsenal(player.SuperStar.Name, play.Card.Title);
    }

    public void DoubleEdgedCard(Player player, byte addedDamage)
    {
        if (addedDamage == 2)
            player.State.NextPlayDamage = PlayerStatus.OpponentReversalIsPlus2D;
        else if (addedDamage == 6)
            player.State.NextPlayDamage = PlayerStatus.OpponentReversalIsPlus6D;
    }

    public void CkeckOpponentsDoubleEdgedCard(Player opponent, Play play)
    {
        if (opponent.State.NextPlayDamage == PlayerStatus.OpponentReversalIsPlus2D)
            play.PlayDamage += 2;
        else if (opponent.State.NextPlayDamage == PlayerStatus.OpponentReversalIsPlus6D)
            play.PlayDamage += 6;
        opponent.State.NextPlayDamage = PlayerStatus.Normal;
    }

    public void RecieveADamageBonusAfterPlayingAStrike(Play play)
    {
        if (play.PrevInfo.LastCardPlayed == PlayerStatus.StrikeWasPlayed || play.PrevInfo.LastCardPlayed == PlayerStatus.KickWasPlayed)
            play.PlayDamage += 2;
    }

    public void MarkLastCardPlayed(Player player, Play play)
    {
        if (play.Card.Subtypes.Contains("Strike") && play.PlayedAs == "MANEUVER")
            player.State.LastCardPlayed = PlayerStatus.StrikeWasPlayed;
        else if (play.PlayedAs == "MANEUVER")
            player.State.LastCardPlayed = PlayerStatus.ManeuverWasPlayed;
        else if (play.PlayedAs == "REVERSAL")
            player.State.LastCardPlayed = PlayerStatus.ReversalWasPlayed;
    }

    public void IncreaseDamageByOneForEverySlamCardInTheRingArea(Player player, Play play)
    {
        foreach (Card card in player.RingArea.Cards)
            if (card.Title.Contains(" Slam"))
                play.PlayDamage += 1;
    }

    public void MoveCardFromOpponentsRingAreaToHisRingSide(Player player, Player opponent)
    {
        Deck cardsThatMayBeDiscarded = new Deck();
        foreach (Card card in opponent.RingArea.Cards)
            if (byte.Parse(card.Damage) <= player.FortitudeRating())
                cardsThatMayBeDiscarded.AddCardToTheDeck(card);
        if (cardsThatMayBeDiscarded.Length() == 0)
        {
            player.View.SayThatNoCardMeetsTheConditionsToBeRemoved();
            return;
        }
        int cardDiscarted = player.View.AskPlayerToSelectACardToDiscardFromRingArea(player.SuperStar.Name, opponent.SuperStar.Name, cardsThatMayBeDiscarded.GetStringDeck());
        opponent.RingSide.AddCardToTheDeck(cardsThatMayBeDiscarded.Cards[cardDiscarted]);
        opponent.RingArea.RemoveCardFromTheDeck(cardsThatMayBeDiscarded.Cards[cardDiscarted]);
    }

    public void MoveCardFromArsenalOrRingSideToTheHand(Player player)
    {
        SelectedEffect deckSelected = player.View.AskUserToChooseBetweenTakingACardFromYourArsenalOrRingside(player.SuperStar.Name);
        if (deckSelected == SelectedEffect.TakeCardFromArsenal)
        {
            int card = player.View.AskPlayerToSelectCardsToPutInHisHand(player.SuperStar.Name, 1, player.GetArsenal().GetStringDeck());
            player.GetHand().AddCardToTheDeck(player.GetArsenal().Cards[card]);
            player.GetArsenal().Cards.RemoveAt(card);
        }
        else if (deckSelected == SelectedEffect.TakeCardFromRingside)
            RecoverCardsToTheHand(player, 1);
    }

    public void DiscardAllThaHand(Player player)
    {
        int numberOfCardsInHand = player.GetNumberOfCardsInTheHand();
        for (int i = 0; i < numberOfCardsInHand; i++)
            player.DiscardCardsFromHandToRingSide(0);
        player.View.SayThatPlayerDiscardsHisHand(player.SuperStar.Name);
    }

    public void SeeOpponentsHand(Player player, Player opponent)
    {
        player.View.SayThatPlayerLooksAtHisOpponentsHand(player.SuperStar.Name, opponent.SuperStar.Name);
        player.View.ShowCards(opponent.GetStringHand());
    }
}
