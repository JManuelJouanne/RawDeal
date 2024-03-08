using RawDealView.Formatters;
using RawDeal.Cards;
using RawDeal.Status;

namespace RawDeal;

public class Play : IViewablePlayInfo
{
    public IViewableCardInfo CardInfo { get; set; }
    public Card Card { get; set; }
    public Player Player { get; set; }  // The player who played the card.
    public string PlayedAs { get; set; }  // MANEUVER, ACTION, REVERSAL
    public byte PlayDamage { get; set; } = 0;
    public byte IdInHand { get; set; }  // This is used to know the position of the card in the hand.
    public State PrevInfo { get; set; } = new ();

    public Play(IViewableCardInfo cardInfo, Player player, string playedAs, byte idInHand)
    {
        CardInfo = cardInfo;
        Player = player;
        Card = (Card)cardInfo;
        PlayedAs = playedAs.ToUpper();
        IdInHand = idInHand;
        PrevInfo.CheckPreviousInformation(Player.State, Card);
    }

    public void SetPlayDamage(Player opponent)  // It is necessary to set the PlayDamage before serching the reversals in the opponent's hand.
    {
        if (PlayedAs == "ACTION")
            return;
        byte damage = byte.Parse(Card.Damage);
        damage = opponent.SuperStar.UseSuperStarAbilityReceivingDamage(damage);
        int extraDamage = PrevInfo.GetExtraDamage();
        PlayDamage = (byte)(damage + extraDamage);
        CheckDoubleEdged(opponent);
    }

    private void CheckDoubleEdged(Player opponent)
    {
        List<string> doubleEdgedCards = new List<string> {"Discus Punch", "Undertaker's Flying Clothesline", "Kane's Flying Clothesline"};
        if (doubleEdgedCards.Contains(Card.Title))
            Player.CardEffects.ApplyCardEffect(Player, opponent, this);
        
    }

    public void SetReversalDamage(Player opponent, Play opponentPlay)
    {
        if (Card.Damage == "#")     // The hashtag means that the card uses the damage of the card it is reversing.
            PlayDamage = opponent.SuperStar.UseSuperStarAbilityReceivingDamage(opponentPlay.PlayDamage);
        else
            PlayDamage = opponent.SuperStar.UseSuperStarAbilityReceivingDamage(byte.Parse(Card.Damage));
    }

    public virtual PlayResult ApplyEffectsOfPlayingACard(Player opponent)
    {
        PlayResult result = PlayedAs switch
        {
            "MANEUVER" => Card.PlayThisCardAsAManeuver(Player, opponent, this),
            "ACTION" => Card.PlayThisCardAsAnAction(Player, opponent, this),
            "REVERSAL" => Card.PlayThisCardAsAReversal(Player, opponent, this),
            _ => throw new Exception("PlayedAs is not valid")
        };
        Player.State.SetLastDamagePlayerState(PlayDamage);
        return result;
    }
}