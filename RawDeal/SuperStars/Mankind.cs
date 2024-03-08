namespace RawDeal.SuperStars;

public class Mankind : SuperStar
{
    public Mankind() : base(
        "MANKIND",
        "Mankind",
        2,
        4,
        "You must always draw 2 cards, if possible, during your draw segment. All damage from opponent is at -1D.",
        false){}

    public override byte UseAbilityBeforeTakingACard(Player player, Player opponent)
    {
        if (player.GetNumberOfCardsInArsenal() >= 2)
            return 2;
        return 1;
    }

    public override byte UseSuperStarAbilityReceivingDamage(byte damage)
    {
        if (damage > 0)
            return (byte)(damage - 1);
        return damage;
    }
}