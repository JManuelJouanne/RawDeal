using RawDealView.Options;
using RawDeal.Cards;

namespace RawDeal.SuperStars;

public class Jericho : SuperStar
{
    private Effects _effects = new();
    public Jericho() : base(
        "CHRIS JERICHO",
        "Jericho",
        7,
        3,
        "Once during your turn, you may discard a card from your hand to force your opponent to discard a card from his hand.",
        false){}

    public override void UseSuperStarAbility(Player player, Player opponent)
    {
        base.UseSuperStarAbility(player, opponent);
        _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
        _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
    }

    public override NextPlay ShowOptionsOfASpecificSuperStar(Player player)
    {
        if (CheckConditionsToUseAbility(player))
            return player.View.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        return player.View.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private bool CheckConditionsToUseAbility(Player player)
        => player.GetNumberOfCardsInTheHand() >= 1 && !_useAbilityInTurn;
}