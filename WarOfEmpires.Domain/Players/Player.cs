﻿using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;
        public const int BaseGoldPerTurn = 500;
        public const int BaseResourceProduction = 20;
        public static readonly long[] BuildingRecruitingLevels = { 50000, 100000, 200000, 300000, 500000, 800000, 1200000, 2000000, 3000000, 5000000, 8000000, 12000000, 20000000, 30000000, 40000000, 50000000, 60000000, 70000000, 80000000, 90000000, 100000000, 110000000, 120000000, 130000000, 140000000, 150000000 };
        public const int AttackDamageCasualtiesModifier = 200;
        public const int AttackStaminaDrainModifier = 2;
        public const int UpkeepWarningTurns = 48;
        public const int NewPlayerTruceHours = 24;
        public const int BlackMarketSellPrice = 1;
        public const int BlackMarketBuyPrice = 20;
        public const double MinimumTransferTax = 0.25;

        public static Resources MercenaryTrainingCost => new(gold: 5000);
        public static Resources PeasantUpkeep => new(food: 2);
        public static Resources SoldierUpkeep => new(food: 2);
        public static Resources MercenaryUpkeep => new(gold: 250, food: 2);
        public static Resources HealCostPerTroopPerTurn => new(food: 2);

        public virtual string DisplayName { get; set; }
        public virtual Security.User User { get; protected set; }
        public virtual Profile Profile { get; protected set; } = new Profile();
        public virtual int CurrentRecruitingEffort { get; protected set; } = 0;
        public virtual int Peasants { get; protected set; } = 10;
        public virtual Resources Resources { get; protected set; } = new Resources(10000, 2000, 2000, 2000, 2000);
        public virtual Resources BankedResources { get; protected set; } = new Resources();
        public virtual int Tax { get; set; } = 50;
        public virtual int AttackTurns { get; protected set; } = 50;
        public virtual int BankTurns { get; protected set; } = 6;
        public virtual int Stamina { get; protected set; } = 100;
        public virtual bool HasUpkeepRunOut { get; protected set; } = false;
        public virtual bool HasNewMarketSales { get; set; } = false;
        public virtual bool HasNewChatMessages { get; set; } = false;
        public virtual int Rank { get; protected set; } = int.MaxValue;
        public virtual TimeSpan GrandOverlordTime { get; protected set; } = TimeSpan.Zero;
        public virtual TitleType Title { get; protected set; } = TitleType.PeasantLeader;
        public virtual Race Race { get; protected set; }
        public virtual Alliance Alliance { get; protected set; }
        public virtual Role AllianceRole { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();
        public virtual ICollection<Workers> Workers { get; protected set; } = new List<Workers>();
        public virtual ICollection<Troops> Troops { get; protected set; } = new List<Troops>();
        public virtual ICollection<Building> Buildings { get; protected set; } = new List<Building>();
        public virtual ICollection<SiegeWeapon> SiegeWeapons { get; protected set; } = new List<SiegeWeapon>();
        public virtual ICollection<Message> SentMessages { get; protected set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; protected set; } = new List<Message>();
        public virtual ICollection<Attack> ExecutedAttacks { get; protected set; } = new List<Attack>();
        public virtual ICollection<Attack> ReceivedAttacks { get; protected set; } = new List<Attack>();
        public virtual ICollection<Caravan> Caravans { get; protected set; } = new List<Caravan>();
        public virtual ICollection<Transaction> BuyTransactions { get; protected set; } = new List<Transaction>();
        public virtual ICollection<Transaction> SellTransactions { get; protected set; } = new List<Transaction>();
        public virtual ICollection<PlayerBlock> PlayerBlocks { get; protected set; } = new List<PlayerBlock>();
        public virtual ICollection<QueuedResearch> QueuedResearch { get; protected set; } = new List<QueuedResearch>();
        public virtual ICollection<Research> Research { get; protected set; } = new List<Research>();

        protected Player() {
        }

        public Player(int id, string displayName, Race race) {
            Id = id;
            DisplayName = displayName;
            Race = race;
            CreationDate = DateTime.UtcNow;

            foreach (var building in GetStartingBuildings()) {
                Buildings.Add(building);
            }
        }

        public static IEnumerable<Building> GetStartingBuildings() {
            yield return new Building(BuildingType.Barracks, 2);
            yield return new Building(BuildingType.Huts, 2);
        }

        public decimal GetTaxRate() {
            return (decimal)Tax / 100;
        }

        public int GetGoldPerWorkerPerTurn() {
            return (int)(GetTaxRate() * BaseGoldPerTurn * GetResearchBonusMultiplier(ResearchType.Commerce));
        }

        public int GetGoldPerTurn() {
            return GetGoldPerWorkerPerTurn() * Workers.Where(w => WorkerDefinitionFactory.Get(w.Type).IsProducer).Sum(w => w.Count);
        }

        public int GetWorkerCount(WorkerType type) {
            return Workers.SingleOrDefault(w => w.Type == type)?.Count ?? 0;
        }

        public ProductionInfo GetProduction(WorkerType workerType) {
            var buildingType = WorkerDefinitionFactory.Get(workerType).BuildingType;
            var raceModifier = RaceDefinitionFactory.Get(Race).GetWorkerModifier(workerType);

            return new ProductionInfo(GetWorkerCount(workerType), GetBuildingBonusMultiplier(buildingType) * GetResearchBonusMultiplier(ResearchType.Efficiency) * raceModifier, GetTaxRate());
        }

        public virtual Resources GetUpkeepPerTurn() {
            var upkeep = new Resources();

            upkeep += (Peasants + Workers.Sum(w => w.Count)) * PeasantUpkeep;
            upkeep += Troops.Sum(t => t.Soldiers) * SoldierUpkeep;
            upkeep += Troops.Sum(t => t.Mercenaries) * MercenaryUpkeep;

            return upkeep;
        }

        public virtual bool WillUpkeepRunOut() {
            var expectedResources = GetTotalResources() + GetResourcesPerTurn() * UpkeepWarningTurns;
            var expectedUpkeep = GetUpkeepPerTurn() * UpkeepWarningTurns;

            return !expectedResources.CanAfford(expectedUpkeep);
        }

        public virtual Troops GetTroops(TroopType type) {
            return Troops.SingleOrDefault(t => t.Type == type) ?? new Troops(type, 0, 0);
        }

        public virtual TroopInfo GetTroopInfo(TroopType type) {
            var definition = TroopDefinitionFactory.Get(type);
            var raceModifier = RaceDefinitionFactory.Get(Race).GetTroopModifier(type);

            return new TroopInfo(
                GetTroops(type),
                definition.BaseAttack,
                definition.BaseDefence,
                GetBuildingBonusMultiplier(definition.BuildingType) * raceModifier,
                GetBuildingBonusMultiplier(BuildingType.Forge),
                GetBuildingBonusMultiplier(BuildingType.Armoury),
                GetSiegeWeaponTroopCount(type)
            );
        }

        public virtual ICollection<Casualties> ProcessAttackDamage(long damage, decimal casualtiesModifier) {
            var troops = Troops.Where(t => t.GetTotals() > 0).Select(t => new { Troops = t, Info = GetTroopInfo(t.Type) });

            if (!troops.Any()) {
                return new List<Casualties>();
            }

            var totalDefence = troops.Sum(t => t.Info.GetTotalDefense());
            var casualties = troops.Select(t => t.Troops.ProcessCasualties((int)(t.Info.GetTotalDefense() * damage * casualtiesModifier / totalDefence / AttackDamageCasualtiesModifier / t.Info.GetDefensePerSoldier() * GetResearchBonusMultiplier(ResearchType.CombatMedicine)))).ToList();

            Stamina = Math.Max(0, (int)(Stamina - damage * AttackStaminaDrainModifier / totalDefence));

            var regencyBonus = GetResearchBonus(ResearchType.Regency);

            if (regencyBonus > 0) {
                foreach (var casualty in casualties) {
                    var regencyMercenaries = (int)Math.Min(casualty.Mercenaries * regencyBonus + 0.5M, GetTotalResources().GetCapacity(MercenaryTrainingCost));

                    TrainTroops(casualty.TroopType, 0, regencyMercenaries);
                }
            }

            return casualties;
        }

        public virtual Resources GetBankCapacity() {
            return new Resources(
                GetBuildingBonus(BuildingType.GoldBank),
                GetBuildingBonus(BuildingType.FoodBank),
                GetBuildingBonus(BuildingType.WoodBank),
                GetBuildingBonus(BuildingType.StoneBank),
                GetBuildingBonus(BuildingType.OreBank)
            );
        }

        public virtual Resources GetAvailableBankCapacity() {
            return GetBankCapacity() - BankedResources;
        }

        public virtual Resources GetBankableResources() {
            var availableCapacity = GetAvailableBankCapacity();

            return new Resources(
                Math.Min(availableCapacity.Gold, Resources.Gold),
                Math.Min(availableCapacity.Food, Resources.Food),
                Math.Min(availableCapacity.Wood, Resources.Wood),
                Math.Min(availableCapacity.Stone, Resources.Stone),
                Math.Min(availableCapacity.Ore, Resources.Ore)
            );
        }

        public virtual void Deposit() {
            var toBank = GetBankableResources();

            BankTurns--;
            Resources -= toBank;
            BankedResources += toBank;
        }

        public virtual Resources GetResourcesPerTurn() {
            return new Resources(
                GetGoldPerTurn(),
                GetProduction(WorkerType.Farmers).GetTotalProduction(),
                GetProduction(WorkerType.WoodWorkers).GetTotalProduction(),
                GetProduction(WorkerType.StoneMasons).GetTotalProduction(),
                GetProduction(WorkerType.OreMiners).GetTotalProduction()
            );
        }

        public virtual void AddBankTurn() {
            BankTurns++;
        }

        public virtual void Recruit() {
            CurrentRecruitingEffort += GetRecruitsPerDay();

            if (CurrentRecruitingEffort >= RecruitingEffortStep) {
                var newRecruits = CurrentRecruitingEffort / RecruitingEffortStep;

                CurrentRecruitingEffort -= newRecruits * RecruitingEffortStep;
                Peasants += newRecruits;
            }
        }

        public virtual void ProcessTurn() {
            var upkeep = GetUpkeepPerTurn();

            AttackTurns++;

            if (Stamina < 100) {
                Stamina++;
            }

            if (CanAfford(upkeep)) {
                var resourcesPerTurn = GetResourcesPerTurn();
                var bankedPercentage = GetResearchBonus(ResearchType.SafeStorage);

                Resources += resourcesPerTurn * (1 - bankedPercentage);
                BankedResources += resourcesPerTurn * bankedPercentage;

                SpendResources(upkeep);

                if (QueuedResearch.Any()) {
                    QueuedResearch.OrderBy(r => r.Priority).First().ProcessTurn(GetWorkerCount(WorkerType.Scientists) * GetBuildingBonus(BuildingType.University));
                }
            }
            else {
                Resources = Resources.SubtractSafe(upkeep, out Resources remainder);
                BankedResources = BankedResources.SubtractSafe(remainder);

                foreach (var troops in Troops) {
                    troops.Untrain(0, troops.Mercenaries);
                }

                HasUpkeepRunOut = true;
            }
        }

        public virtual void TrainWorkers(WorkerType type, int count) {
            var definition = WorkerDefinitionFactory.Get(type);
            var workers = Workers.SingleOrDefault(w => w.Type == type);

            if (workers == null) {
                Workers.Add(new Workers(type, count));
            }
            else {
                workers.Count += count;
            }

            Peasants -= count;
            SpendResources(count * definition.Cost);
        }

        public virtual void UntrainWorkers(WorkerType type, int count) {
            Workers.Single(w => w.Type == type).Count -= count;

            Peasants += count;
        }

        public virtual void UpgradeBuilding(BuildingType type) {
            var definition = BuildingDefinitionFactory.Get(type);
            var building = Buildings.SingleOrDefault(b => b.Type == type);

            SpendResources(definition.GetNextLevelCost(building?.Level ?? 0));

            if (building == null) {
                building = new Building(type, 1);
                Buildings.Add(building);
            }
            else {
                building.Level++;
            }
        }

        public virtual int GetBuildingBonus(BuildingType type) {
            var definition = BuildingDefinitionFactory.Get(type);

            return definition.GetBonus(Buildings.SingleOrDefault(b => b.Type == type)?.Level ?? 0);
        }

        public decimal GetBuildingBonusMultiplier(BuildingType type) {
            return (100m + GetBuildingBonus(type)) / 100m;
        }

        public decimal GetResearchBonus(ResearchType type) {
            var definition = ResearchDefinitionFactory.Get(type);

            return definition.LevelBonus * (Research.SingleOrDefault(r => r.Type == type)?.Level ?? 0);
        }

        public decimal GetResearchBonusMultiplier(ResearchType type) {
            return 1 + GetResearchBonus(type);
        }

        public int GetSiegeWeaponCount(SiegeWeaponType type) {
            return SiegeWeapons.SingleOrDefault(w => w.Type == type)?.Count ?? 0;
        }

        public int GetSiegeWeaponTroopCount(TroopType type) {
            var definition = SiegeWeaponDefinitionFactory.Get(type);

            return Math.Min(GetTroops(type).GetTotals(), GetSiegeWeaponCount(definition.Type) * definition.TroopCount);
        }

        public virtual int GetAvailableBarracksCapacity() {
            return GetBuildingBonus(BuildingType.Barracks) - Troops.Sum(t => t.GetTotals());
        }

        public virtual int GetAvailableHutCapacity() {
            return GetBuildingBonus(BuildingType.Huts) - Workers.Sum(w => w.Count);
        }

        public virtual int GetAvailableHousingCapacity() {
            var housingCapacity = GetAvailableBarracksCapacity() + GetAvailableHutCapacity() - Peasants;

            if (housingCapacity < 0) {
                return 0;
            }
            else {
                return housingCapacity;
            }
        }

        public virtual int GetTheoreticalRecruitsPerDay() {
            var recruiting = 0;
            var totalBuildingGold = GetTotalGoldSpentOnBuildings();

            // Get recruiting for total gold spent
            recruiting += BuildingRecruitingLevels.Where(g => g <= totalBuildingGold).Count();

            // Get recruiting for defences
            recruiting += GetBuildingBonus(BuildingType.Defences);

            // Adjust for not having enough soldiers
            recruiting -= GetSoldierRecruitsPenalty();

            if (recruiting > 25) {
                return 25;
            }
            else if (recruiting < 1) {
                return 1;
            }
            else {
                return recruiting;
            }
        }

        public virtual int GetSoldierRecruitsPenalty() {
            var soldiers = Troops.Sum(t => t.Soldiers);
            var peasants = Peasants + Workers.Sum(w => w.Count);
            var ratio = 1.0m * soldiers / (soldiers + peasants);

            if (ratio >= 0.5m) {
                return 0;
            }
            else if (ratio >= 0.45m) {
                return 1;
            }
            else if (ratio >= 0.4m) {
                return 2;
            }
            else {
                return 3;
            }
        }

        public virtual int GetRecruitsPerDay() {
            var housingCapacity = GetAvailableHousingCapacity();
            var recruiting = GetTheoreticalRecruitsPerDay();

            return Math.Min(housingCapacity, recruiting);
        }

        public long GetTotalGoldSpentOnBuildings() {
            var startingBuildings = GetStartingBuildings().ToDictionary(b => b.Type, b => b.Level);
            var buildingTotals = Buildings
                .Where(b => b.Type != BuildingType.Defences)
                .Select(b => new {
                    Definition = BuildingDefinitionFactory.Get(b.Type),
                    StartingLevel = startingBuildings.ContainsKey(b.Type) ? startingBuildings[b.Type] : 0,
                    b.Level
                })
                .Sum(b => Enumerable.Range(b.StartingLevel, b.Level - b.StartingLevel).Sum(l => b.Definition.GetNextLevelCost(l).Gold));

            return buildingTotals;
        }

        public virtual int GetStaminaToHeal() {
            var healCostPerTurn = Troops.Sum(t => t.GetTotals()) * HealCostPerTroopPerTurn;
            var totalResources = GetTotalResources();

            return (int)Math.Min(100 - Stamina, totalResources.GetCapacity(healCostPerTurn));
        }

        public virtual void TrainTroops(TroopType type, int soldiers, int mercenaries) {
            var definition = TroopDefinitionFactory.Get(type);
            var troops = Troops.SingleOrDefault(t => t.Type == type);

            if (troops == null) {
                Troops.Add(new Troops(type, soldiers, mercenaries));
            }
            else {
                troops.Train(soldiers, mercenaries);
            }

            Peasants -= soldiers;
            SpendResources(soldiers * definition.Cost + mercenaries * MercenaryTrainingCost);
        }

        public virtual void UntrainTroops(TroopType type, int soldiers, int mercenaries) {
            Troops.Single(t => t.Type == type).Untrain(soldiers, mercenaries);

            Peasants += soldiers;
        }

        public virtual void HealTroops(int staminaToHeal) {
            var troops = Troops.Sum(t => t.GetTotals());

            Stamina += staminaToHeal;
            SpendResources(troops * HealCostPerTroopPerTurn * staminaToHeal);
        }

        public virtual void ExecuteAttack(AttackType type, Player defender, int attackTurns) {
            var attack = AttackFactory.Get(type, this, defender, attackTurns);

            attack.Execute();
            ExecutedAttacks.Add(attack);
            defender.ReceivedAttacks.Add(attack);
            AddResources(attack.Resources);
            defender.Resources -= attack.Resources;
            AttackTurns -= attackTurns;
        }

        public virtual void AddResources(Resources resources) {
            Resources += resources;

            if (HasUpkeepRunOut && CanAfford(GetUpkeepPerTurn())) {
                HasUpkeepRunOut = false;
            }
        }

        public virtual Resources GetTotalResources() {
            return Resources + BankedResources;
        }

        public virtual bool CanAfford(Resources resources) {
            return GetTotalResources().CanAfford(resources);
        }

        public virtual void SpendResources(Resources resources) {
            Resources = Resources.SubtractSafe(resources, out Resources remainder);
            BankedResources -= remainder;
        }

        public virtual void BuildSiege(SiegeWeaponType type, int count) {
            var definition = SiegeWeaponDefinitionFactory.Get(type);
            var weapon = SiegeWeapons.SingleOrDefault(b => b.Type == type);

            SpendResources(definition.Cost * count);

            if (weapon == null) {
                weapon = new SiegeWeapon(type, count);
                SiegeWeapons.Add(weapon);
            }
            else {
                weapon.Count += count;
            }
        }

        public virtual void DiscardSiege(SiegeWeaponType type, int count) {
            var weapon = SiegeWeapons.Single(b => b.Type == type);

            weapon.Count -= count;
        }

        public virtual void SellResources(IEnumerable<MerchandiseTotals> merchandiseTotals) {
            var maximumCapacity = GetBuildingBonus(BuildingType.Market);
            var caravan = new Caravan(this);
            Caravans.Add(caravan);

            foreach (var totals in merchandiseTotals) {
                var quantity = totals.Quantity;
                SpendResources(totals.ToResources());

                while (quantity > caravan.GetRemainingCapacity(maximumCapacity)) {
                    quantity -= caravan.GetRemainingCapacity(maximumCapacity);
                    caravan.Merchandise.Add(new Merchandise(totals.Type, caravan.GetRemainingCapacity(maximumCapacity), totals.Price));

                    caravan = new Caravan(this);
                    Caravans.Add(caravan);
                }

                caravan.Merchandise.Add(new Merchandise(totals.Type, quantity, totals.Price));
            }
        }

        public virtual void UpdateRank(int rank, TitleType title) {
            Rank = rank;
            Title = title;

            if (title == TitleType.GrandOverlord) {
                GrandOverlordTime += TimeSpan.FromMinutes(1);
            }
        }

        public virtual void SendMessage(Player recipient, string subject, string body) {
            var message = new Message(this, recipient, subject, body);

            SentMessages.Add(message);
            recipient.ReceivedMessages.Add(message);
        }

        public virtual void Block(Player player) {
            if (!PlayerBlocks.Any(b => b.BlockedPlayer == player)) {
                PlayerBlocks.Add(new PlayerBlock(player));
            }
        }

        public virtual void Unblock(Player player) {
            foreach (var playerBlock in PlayerBlocks.Where(b => b.BlockedPlayer == player).ToList()) {
                PlayerBlocks.Remove(playerBlock);
            }
        }

        public virtual void Update(string displayName) {
            DisplayName = displayName;
        }

        public virtual void Reset() {
            Buildings.Clear();
            Troops.Clear();
            Workers.Clear();
            Caravans.Clear();
            BuyTransactions.Clear();
            ReceivedAttacks.Clear();
            QueuedResearch.Clear();
            Research.Clear();

            Resources = new Resources(10000, 2000, 2000, 2000, 2000);
            BankedResources = new Resources();
            foreach (var building in GetStartingBuildings()) {
                Buildings.Add(building);
            }
            Peasants = 10;
            AttackTurns = 50;
            BankTurns = 6;
            Tax = 50;
            Stamina = 100;
            HasUpkeepRunOut = false;
            HasNewMarketSales = false;
            CreationDate = DateTime.UtcNow;
            GrandOverlordTime = TimeSpan.Zero;
        }

        public virtual void BuyResourcesFromBlackMarket(BlackMarketMerchandiseTotals merchandiseTotals) {
            SpendResources(new Resources(merchandiseTotals.Quantity * BlackMarketBuyPrice));
            AddResources(merchandiseTotals.ToResources());
        }

        public virtual void SellResourcesToBlackMarket(BlackMarketMerchandiseTotals merchandiseTotals) {
            SpendResources(merchandiseTotals.ToResources());
            AddResources(new Resources(merchandiseTotals.Quantity * BlackMarketSellPrice));
        }

        public virtual void QueueResearch(ResearchType type) {
            var priority = 1;

            if (QueuedResearch.Any()) {
                priority = QueuedResearch.Max(r => r.Priority) + 1;
            }

            QueuedResearch.Add(new QueuedResearch(this, priority, type));
        }

        public virtual void PrioritizeResearch(ResearchType type) {
            var priority = 1;

            if (QueuedResearch.Any()) {
                priority = QueuedResearch.Min(r => r.Priority) - 1;
            }

            QueuedResearch.Add(new QueuedResearch(this, priority, type));
        }

        public virtual void RemoveQueuedResearch(QueuedResearch queuedResearch) {
            QueuedResearch.Remove(queuedResearch);
        }

        public virtual void TransferResources(double ratio, Player recipient, Resources resources) {
            decimal tax = (decimal)Math.Max(1 - ratio, MinimumTransferTax);

            SpendResources(resources);
            recipient.AddResources(resources * (1 - tax));
        }
    }
}
