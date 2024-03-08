using RawDeal.Cards;

namespace RawDeal;

public class ArsenalValidation
{
    private Deck _arsenal;
    private string _superstarLogo;

    public ArsenalValidation(Deck arsenal, string superstarLogo)
    {
        _arsenal = arsenal;
        _superstarLogo = superstarLogo;
    }
    
    public bool ArsenalIsValid()
    {
        if (ArsenalHasAInvalidSize())
            return false;
        if (ArsenalContainsRepeatedCards())
            return false;
        if (ArsenalContainsHeelAndFaceCards())
            return false;
        if (ArsenalContainsOtherSuperstarsMove())
            return false;
        return true;
    }

    private bool ArsenalHasAInvalidSize()
        => _arsenal.Length() != 60;

    private bool ArsenalContainsRepeatedCards()
    {
        for (byte i = 0; i < _arsenal.Length(); i++)
        {
            if (_arsenal.Cards[i].Subtypes.Contains("SetUp"))   // SetUp cards can be repeated
                continue;
            if (CardIsRepeated(_arsenal.Cards[i], i, 1))
                return true;
        }
        return false;
    }

    private bool CardIsRepeated(Card card, byte index, byte repetition)
    {
        for (byte i = (byte)(index + 1); i < _arsenal.Length(); i++)
            if (card.Title == _arsenal.Cards[i].Title)
                return ThisCardCantBeRepeated(card, i, repetition);
        return false;
    }

    private bool ThisCardCantBeRepeated(Card card, byte index, byte repetition)
    {
        if (card.Subtypes.Contains("Unique"))   // Unique cards can't be repeated
            return true;
        else if (repetition < 3)                // Cards can be repeated up to 3 times
            return CardIsRepeated(card, index, (byte)(repetition + 1));
        return true;
    }

    private bool ArsenalContainsHeelAndFaceCards()  // Arsenal mustn't contain Heel and Face cards. Just one of them.
    {
        bool deckContainsHeel = DoesDeckContainsSubtype(_arsenal.Cards, "Heel");
        bool deckContainsFace = DoesDeckContainsSubtype(_arsenal.Cards, "Face");
        return deckContainsHeel && deckContainsFace;
    }

    private bool DoesDeckContainsSubtype(List<Card> deck, string subtype)
    {
        foreach (Card card in deck)
            if (card.Subtypes.Contains(subtype))
                return true;
        return false;
    }

    private bool ArsenalContainsOtherSuperstarsMove()   // Arsenal mustn't contain other Superstar's moves.
    {
        List<string> logos = new List<string> { "HHH", "StoneCold", "Undertaker", "Kane", "Mankind", "Jericho", "TheRock" };
        foreach (Card card in _arsenal.Cards)
            if (ThisCardBelongsToSomeSuperstar(card, logos))
                return true;
        return false;
    }

    private bool ThisCardBelongsToSomeSuperstar(Card card, List<string> logos)
    {
        foreach (string superstar in logos)
                if (ThisCardBelongsToThisSuperstar(card, superstar))
                    return true;
        return false;
    }

    private bool ThisCardBelongsToThisSuperstar(Card card, string logo)
        => card.Subtypes.Contains(logo) && logo != _superstarLogo;
}