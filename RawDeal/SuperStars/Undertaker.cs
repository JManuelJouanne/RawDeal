using RawDealView.Options;
using RawDeal.Cards;

namespace RawDeal.SuperStars;

public class Undertaker : SuperStar
{
    private Effects _effects = new();
    public Undertaker() : base(
        "THE UNDERTAKER",
        "Undertaker",
        6,
        4,
        "Once during your turn, you may discard 2 cards to the Ringside pile and take 1 card from the Ringside pile and place it into your hand.",
        false){}

    public override void UseSuperStarAbility(Player player, Player opponent)
    {
        base.UseSuperStarAbility(player, opponent);
        _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 2);
        _effects.RecoverCardsToTheHand(player, 1);
    }

    public override NextPlay ShowOptionsOfASpecificSuperStar(Player player)
    {
        if (CheckConditionsToUseAbility(player))
            return player.View.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        return player.View.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private bool CheckConditionsToUseAbility(Player player)
        => player.GetNumberOfCardsInTheHand() >= 2 && !_useAbilityInTurn;
}