using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Tetris
{

    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }


    public class ShopItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public Rarity ItemRarity { get; set; }
    }
    
    public partial class Shop : Window
    {
        

        private List<ShopItem> _masterItemPool = new List<ShopItem>()
        {

            new ShopItem { Name = "MaxComboExtender", Description = "Extends maximum combo by 1", Price = 50, ItemRarity = Rarity.Common, },
            new ShopItem { Name = "", Description = "", Price = 50, ItemRarity = Rarity.Common },

            new ShopItem { Name = "", Description = "", Price = 220, ItemRarity = Rarity.Rare },
            new ShopItem { Name = "", Description = "", Price = 200, ItemRarity = Rarity.Rare },
 
            new ShopItem { Name = "", Description = "", Price = 400, ItemRarity = Rarity.Epic },
            new ShopItem { Name = "", Description = "", Price = 400, ItemRarity = Rarity.Epic },

            new ShopItem { Name = "", Description = "", Price = 800, ItemRarity = Rarity.Legendary }
        };


        private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>()
        {
            { Rarity.Common, 60 },
            { Rarity.Rare, 25 },
            { Rarity.Epic, 12 },
            { Rarity.Legendary, 3 }
        };

        private Random _rand = new Random();

        public Shop()
        {
            InitializeComponent();
            LoadRandomShopItems();
        }

        private void LoadRandomShopItems()
        {
            List<ShopItem> generatedShopItems = new List<ShopItem>();

            for (int i = 0; i < 4; i++)
            {
                Rarity chosenRarity = GetRandomRarity();

                var itemsOfRarity = _masterItemPool.Where(x => x.ItemRarity == chosenRarity).ToList();

 
                if (itemsOfRarity.Count == 0)
                {
                    itemsOfRarity = _masterItemPool;
                }


                ShopItem randomItem = itemsOfRarity[_rand.Next(itemsOfRarity.Count)];
                generatedShopItems.Add(randomItem);
            }


            ItemButton1.Content = $"{generatedShopItems[0].Name} ({generatedShopItems[0].ItemRarity}) - {generatedShopItems[0].Price}G";
            ItemButton2.Content = $"{generatedShopItems[1].Name} ({generatedShopItems[1].ItemRarity}) - {generatedShopItems[1].Price}G";
            ItemButton3.Content = $"{generatedShopItems[2].Name} ({generatedShopItems[2].ItemRarity}) - {generatedShopItems[2].Price}G";
            ItemButton4.Content = $"{generatedShopItems[3].Name} ({generatedShopItems[3].ItemRarity}) - {generatedShopItems[3].Price}G";
        }



        private Rarity GetRandomRarity()
        {
            int totalWeight = _rarityWeights.Values.Sum();
            int rolledWeight = _rand.Next(totalWeight);

            foreach (var kvp in _rarityWeights)
            {
                if (rolledWeight < kvp.Value)
                {
                    return kvp.Key;
                }
                rolledWeight -= kvp.Value;
            }

            return Rarity.Common;
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void Window_Deactivated(object sender, EventArgs e)
        {

        }
    }
}