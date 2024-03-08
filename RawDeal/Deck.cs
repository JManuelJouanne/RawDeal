using RawDealView.Formatters;
using RawDeal.Cards;

namespace RawDeal;

public class Deck
{
    public List<Card> Cards = new();

    public int Length()
        => Cards.Count;

    public void AddCardToTheDeck(Card card)
        => Cards.Add(card);

    private void InsertCardOnTheBottomOfTheDeck(Card card)
        => Cards.Insert(0, card);

    public void RemoveCardFromTheDeck(Card card)
    {
        if (Cards.Contains(card))
            Cards.Remove(card);
    }

    public void MoveCardFromThisDeckToTheTopOfAnotherDeck(Deck anotherDeck)
    {
        anotherDeck.AddCardToTheDeck(Cards[Cards.Count - 1]);
        Cards.RemoveAt(Cards.Count - 1);
    }

    public void MoveSpecificCardToTheTopOfAnotherDeck(int cardToMove, Deck anotherDeck)
    {
        anotherDeck.AddCardToTheDeck(Cards[cardToMove]);
        Cards.RemoveAt(cardToMove);
    }

    public void MoveSpecificCardToTheBottomOfAnotherDeck(int cardToMove, Deck anotherDeck)
    {
        anotherDeck.InsertCardOnTheBottomOfTheDeck(Cards[cardToMove]);
        Cards.RemoveAt(cardToMove);
    }

    public bool CardExistsInThisDeck(string cardTitle)
    {
        foreach (Card card in Cards)
            if (card.Title == cardTitle)
                return true;
        return false;
    }

    public Card GetCardFromThisDeck(string cardTitle)
    {
        foreach (Card card in Cards)
            if (card.Title == cardTitle)
            {
                Cards.Remove(card);
                return card;
            }
        return Cards[0];
    }

    public List<string> GetStringDeck()   // Returns a string list to show to the user
    {
        List<string> strDeck = new();
        foreach (Card card in Cards)
            strDeck.Add(Formatter.CardToString(card));
        return strDeck;
    }
}