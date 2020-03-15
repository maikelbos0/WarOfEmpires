namespace WarOfEmpires.Commands.Markets {
    public sealed class BuyResourcesCommand : ICommand {
        public string Email { get; }
        public string Food { get; set; }
        public string FoodPrice { get; set; }
        public string Wood { get; set; }
        public string WoodPrice { get; set; }
        public string Stone { get; set; }
        public string StonePrice { get; set; }
        public string Ore { get; set; }
        public string OrePrice { get; set; }

        public BuyResourcesCommand(string email, string food, string foodPrice, string wood, string woodPrice, string stone, string stonePrice, string ore, string orePrice) {
            Email = email;
            Food = food;
            FoodPrice = foodPrice;
            Wood = wood;
            WoodPrice = woodPrice;
            Stone = stone;
            StonePrice = stonePrice;
            Ore = ore;
            OrePrice = orePrice;
        }
    }
}