using RawDealView;
using RawDealView.Formatters;
using RawDeal.SuperStars;
using RawDeal.Cards;
using RawDeal.Status;

namespace RawDeal;

public class Player
{
    public SuperStar SuperStar;
    private Deck _arsenal = new ();
    private Deck _hand = new ();
    public Deck RingSide = new ();
    public Deck RingArea = new ();
    public View View;
    public ConditionsCatalog CardConditions = new ();
    public EffectsCatalog CardEffects = new ();
    public State State = new ();

    // INICIATION METHODS
    public Player(View view)
    {
        View = view;
    }

    public void InitiateCards(List<Card> cards, string[] deckInfo)
    {
        CreateSuperStar(deckInfo[0]);
        CreateArsenal(deckInfo[1..], cards);
    }

    private void CreateSuperStar(string superstarLine)
    {
        string superstarName = superstarLine.Substring(0, superstarLine.IndexOf("(") - 1);
        SuperStar = superstarName switch
        {
            "THE ROCK" => new TheRock(),
            "HHH" => new HHH(),
            "STONE COLD STEVE AUSTIN" => new StoneCold(),
            "THE UNDERTAKER" => new Undertaker(),
            "KANE" => new Kane(),
            "MANKIND" => new Mankind(),
            "CHRIS JERICHO" => new Jericho(),
            _ => throw new Exception("Invalid SuperStar")
        };
    }

    private void CreateArsenal(string[] cardNames, List<Card> cards)
    {
        foreach (string cardName in cardNames)
            foreach (Card card in cards)
                CheckIfCardMustBeAddedToArsenal(cardName, card);
    }

    private void CheckIfCardMustBeAddedToArsenal(string cardName, Card card)
    {
        if (card.Title == cardName)
            _arsenal.AddCardToTheDeck(card);
    }

    public bool ArsenalDeckIsValid()
    {
        ArsenalValidation arsenalValidation = new ArsenalValidation(_arsenal, SuperStar.Logo);
        return arsenalValidation.ArsenalIsValid();
    }

    public void InitiatePlayer()    // Draw the number of cards of the HandSize to start.
        => TakeCardsFromArsenalToHand(SuperStar.HandSize);


    // PLAY METHODS
    public Play SelectCardToPlay(Player opponent)
    {
        List<Play> options = CreatePlaysAvailable(opponent);
        List<string> strOptions = FormatPlays(options);
        int optionId = View.AskUserToSelectAPlay(strOptions);
        View.SayThatPlayerIsTryingToPlayThisCard(SuperStar.Name, strOptions[optionId]);
        return options[optionId];
    }

    private List<Play> CreatePlaysAvailable(Player opponent)
    {
        List<Play> plays = new();
        for (byte idInHand = 0; idInHand < _hand.Length(); idInHand++)
        {
            if (CardConditions.CanThisCardBePlayedAsManeuverOrAction(this, opponent, _hand.Cards[idInHand], "Action"))
                AddCardFromATypeToTheListOfPlays(plays, _hand.Cards[idInHand], idInHand, "Action");
            if (CardConditions.CanThisCardBePlayedAsManeuverOrAction(this, opponent, _hand.Cards[idInHand], "Maneuver"))
                AddCardFromATypeToTheListOfPlays(plays, _hand.Cards[idInHand], idInHand, "Maneuver");
        }
        return plays;
    }

    public Play SelectCardToReverse(Play opponentsPlay)
    {
        List<Play> options = CreateReversalsAvailable(opponentsPlay);
        List<string> strOptions = FormatPlays(options);
        int optionId = View.AskUserToSelectAReversal(SuperStar.Name, strOptions);
        View.SayThatPlayerReversedTheCard(SuperStar.Name, strOptions[optionId]);
        return options[optionId];
    }

    private List<Play> CreateReversalsAvailable(Play opponentsPlay)
    {
        List<Play> plays = new();
        for (byte idInHand = 0; idInHand < _hand.Length(); idInHand++)
            if (CardConditions.CanThisCardBePlayedAsReversal(this, opponentsPlay, _hand.Cards[idInHand], "fromHand"))
                AddCardFromATypeToTheListOfPlays(plays, _hand.Cards[idInHand], idInHand, "Reversal");
        return plays;
    }

    private void AddCardFromATypeToTheListOfPlays(List<Play> plays, Card card, byte idInHand, string type)
    {
        Play play = new Play(card, this, type, idInHand);
        plays.Add(play);
    }

    private List<string> FormatPlays(List<Play> plays)  // Get the string list of plays to show to the user
    {
        List<string> strPlays = new();
        foreach(Play play in plays)
            strPlays.Add(Formatter.PlayToString(play));
        return strPlays;
    }

    // TAKE CARDS METHODS
    public void TakeCardsFromArsenalToHand(byte n)
    {
        for (byte i = 0; i < n; i++)
            if (_arsenal.Length() > 0)
                _arsenal.MoveCardFromThisDeckToTheTopOfAnotherDeck(_hand);
    }

    public void ReturnCardFromHandToArsenal(int cardToReturn)   // Just one card
        => _hand.MoveSpecificCardToTheBottomOfAnotherDeck(cardToReturn, _arsenal);

    public Card TakeACardFromPlayersArsenalToHisRingSide()      // Just one card
    {
        _arsenal.MoveCardFromThisDeckToTheTopOfAnotherDeck(RingSide);
        return RingSide.Cards[RingSide.Length() - 1];
    }

    public void RecoverCardsFromRingSideToArsenal(int cardToRecover)
        => RingSide.MoveSpecificCardToTheBottomOfAnotherDeck(cardToRecover, _arsenal);

    public void DiscardCardsFromHandToRingSide(int cardToDiscard)
        => _hand.MoveSpecificCardToTheTopOfAnotherDeck(cardToDiscard, RingSide);

    public void RecoverCardFromRingSideToHand(int cardToReturn)
        => RingSide.MoveSpecificCardToTheTopOfAnotherDeck(cardToReturn, _hand);

    public void PutCardInRingArea(byte idInHand)
        => _hand.MoveSpecificCardToTheTopOfAnotherDeck(idInHand, RingArea);


    // GET METHODS
    public PlayerInfo GetPlayerInfo()
        => new PlayerInfo(SuperStar.Name, FortitudeRating(), _hand.Length(), _arsenal.Length());

    public byte GetNumberOfCardsInTheHand()
        => (byte)_hand.Length();

    public byte GetNumberOfCardsInArsenal()
        => (byte)_arsenal.Length();

    public Deck GetHand()
        => _hand;
    
    public Deck GetArsenal()
        => _arsenal;

    public List<string> GetStringHand()
        => _hand.GetStringDeck();
    
    public byte FortitudeRating()
    {
        byte fortitudeRating = 0;
        foreach (Card card in RingArea.Cards)
            if (card.Damage != "#")
                fortitudeRating += byte.Parse(card.Damage);
        return fortitudeRating;
    }
}