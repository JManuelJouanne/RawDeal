using RawDeal.Cards;

namespace RawDeal.SuperStars;

public class TheRock : SuperStar
{
    private Effects _effects = new();
    public TheRock() : base(
        "THE ROCK",
        "TheRock",
        5,
        5,
        "At the start of your turn, before your draw segment, you may take 1 card from your Ringside pile and place it on the bottom of your Arsenal.",
        false){}
    
    public override void UseSuperStarAbility(Player player, Player opponent)
    {
        base.UseSuperStarAbility(player, opponent);
        _effects.RecoverDamage(player, 1);
    }

    public override byte UseAbilityBeforeTakingACard(Player player, Player opponent)
    {
        if (player.RingSide.Length() > 0)
            if (player.View.DoesPlayerWantToUseHisAbility(Name))
                UseSuperStarAbility(player, opponent);
        return 1;
    }
}