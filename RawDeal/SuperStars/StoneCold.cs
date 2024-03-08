using RawDealView.Options;
using RawDeal.Cards;

namespace RawDeal.SuperStars;

public class StoneCold : SuperStar
{
    private Effects _effects = new();
    public StoneCold() : base(
        "STONE COLD STEVE AUSTIN",
        "StoneCold",
        7,
        5,
        "Once during your turn, you may draw a card, but you must then take a card from your hand and place it on the bottom of your Arsenal.",
        false){}

    public override void UseSuperStarAbility(Player player, Player opponent)
    {
        base.UseSuperStarAbility(player, opponent);
        _effects.MustDrawSpecifiedNumberOfCards(player, 1);
        _effects.ReturnCardFromPlayersHandToArsenal(player);
    }

    public override NextPlay ShowOptionsOfASpecificSuperStar(Player player)
    {
        if (CheckConditionsToUseAbility(player))
            return player.View.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        return player.View.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private bool CheckConditionsToUseAbility(Player player)
        => player.GetNumberOfCardsInArsenal() > 0 && !_useAbilityInTurn;
}