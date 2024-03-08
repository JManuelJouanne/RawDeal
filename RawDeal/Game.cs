using RawDealView;
using RawDealView.Options;
using RawDeal.Cards;
using RawDeal.Status;
using Newtonsoft.Json;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private List<Player> _players = new();
    private List<Card> _cards;
    private int _winner = -1; // winner = index of the winner (-1 = no winner)
    private bool _turnIsOver;
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        ReadCardsInfo();
    }

    private void ReadCardsInfo()
    {
        string cards = Path.Combine("data", "cards.json");
        string infoCards = File.ReadAllText(cards);
        _cards = JsonConvert.DeserializeObject<List<Card>>(infoCards);
    }

    public void Play()
    {
        bool PlayersCreatedSuccessfully = CreatePlayers();
        if (!PlayersCreatedSuccessfully)
            return;
        CheckWhoStarts();
        while (_winner == -1)   // Take turns until there is a winner.
            PlayTurn();
    }

    private bool CreatePlayers()    // Returns true if the players were created successfully and the deck is valid.
    {
        for (byte i = 0; i < 2; i++)
        {
            Player player = new Player(_view);

            SelectDeck(player);
            if (!player.ArsenalDeckIsValid())
            {
                _view.SayThatDeckIsInvalid();
                return false;
            }
            player.InitiatePlayer();
            _players.Add(player);
        }
        return true;
    }

    private void SelectDeck(Player player)
    {
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        string[] deckInfo = File.ReadAllLines(deckPath);
        player.InitiateCards(_cards, deckInfo);
    }

    private void CheckWhoStarts()
    {
        if (_players[0].SuperStar.SuperstarValue < _players[1].SuperStar.SuperstarValue)
            _players.Reverse();
    }

    private void PlayTurn()
    {
        IniciateTurn();
        while(_winner == -1 && !_turnIsOver)
        {
            _view.ShowGameInfo(_players[0].GetPlayerInfo(), _players[1].GetPlayerInfo());
            SelectOptionToPlayTheTurn();
        }
        CheckIfThereAreWinner();
        ChangeTurn();
    }

    private void IniciateTurn()     // The turn is said to begin and the player draws a card.
    {
        _view.SayThatATurnBegins(_players[0].SuperStar.Name);   // ability returns 1 in most cases
        byte cardsToTake = _players[0].SuperStar.UseAbilityBeforeTakingACard(_players[0], _players[1]);
        _players[0].TakeCardsFromArsenalToHand(cardsToTake);
    }

    private void SelectOptionToPlayTheTurn()
    {
        NextPlay move = _players[0].SuperStar.ShowOptionsOfASpecificSuperStar(_players[0]);
        switch (move)
        {
            case NextPlay.UseAbility:
                _players[0].SuperStar.UseSuperStarAbility(_players[0], _players[1]);
                break;
            case NextPlay.ShowCards:
                ShowSomeDeck();
                break;
            case NextPlay.PlayCard:
                PlayACard();
                break;
            case NextPlay.EndTurn:
                _turnIsOver = true;
                break;
            case NextPlay.GiveUp:
                _winner = 1;
                break;
        };
    }

    private void ShowSomeDeck()
    {
        CardSet deck = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<string> strDeck = WhatDeckDidThePlayerSelectToSee(deck).GetStringDeck();
        _view.ShowCards(strDeck);
    }

    private Deck WhatDeckDidThePlayerSelectToSee(CardSet deckSelected)
    {
        Deck deck = deckSelected switch
        {
            CardSet.Hand => _players[0].GetHand(),
            CardSet.RingArea => _players[0].RingArea,
            CardSet.RingsidePile => _players[0].RingSide,
            CardSet.OpponentsRingArea => _players[1].RingArea,
            CardSet.OpponentsRingsidePile => _players[1].RingSide,
            _ => throw new Exception("Invalid deck")
        };
        return deck;
    }

    private void PlayACard()
    {
        try
        {
            Play play = _players[0].SelectCardToPlay(_players[1]);
            _players[0].State.ClearStateAfterPlay();
            play.SetPlayDamage(_players[1]);    // The player's status is set to the damage of the card he is playing.
            try                                 // This will help for some conditions of other cards.
            {
                Play reversal = _players[1].SelectCardToReverse(play);
                PlayResult result = ApplyEffectsOfCards(play, reversal);
                CheckEffectsOfThePlay(result);
            }
            catch (Exception)
            {
                PlayResult result = ApplyEffectsOfCards(play, null);
                CheckEffectsOfThePlay(result);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("(Player did not play a card)");
        }
    }

    private PlayResult ApplyEffectsOfCards(Play play, Play reversal)
    {
        try
        {
            reversal.SetReversalDamage(_players[0], play);
            _players[0].DiscardCardsFromHandToRingSide(play.IdInHand);  // The card reversed is discarded.
            _turnIsOver = true;
            return reversal.ApplyEffectsOfPlayingACard(_players[0]);
        }
        catch (Exception)
        {
            return play.ApplyEffectsOfPlayingACard(_players[1]);
        }
    }

    private void CheckEffectsOfThePlay(PlayResult result)
    {
        if (result == PlayResult.OpponentIsDead)
            _winner = 0;
        else if (result == PlayResult.PlayerIsDead)
            _winner = 1;
        else if (result == PlayResult.RevertedByDeck)
            _turnIsOver = true;
    }

    private void CheckIfThereAreWinner()
    {
        if (_winner != -1)
            _view.CongratulateWinner(_players[_winner].SuperStar.Name);
        else
        {
            CheckIfAPlayerHasNoCardsInHisArsenal(0);
            CheckIfAPlayerHasNoCardsInHisArsenal(1);
        }
    }

    private void CheckIfAPlayerHasNoCardsInHisArsenal(byte player)
    {
        if (_players[player].GetNumberOfCardsInArsenal() == 0)
        {
            _winner = (player + 1) % 2;     // The winner is the other player.
            _view.CongratulateWinner(_players[_winner].SuperStar.Name);
        }
    }

    private void ChangeTurn()   // The player who is playing is always the first in the list (number 0).
    {
        _players[0].State.ClearStateAfterTurn();
        _players.Reverse();
        _turnIsOver = false;
    }
}

