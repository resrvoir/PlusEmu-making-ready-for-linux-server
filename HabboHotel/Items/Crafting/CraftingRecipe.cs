﻿using System;
using System.Collections.Generic;

namespace Quasar.HabboHotel.Items.Crafting
{
    class CraftingRecipe
    {
        internal string Id;
        internal Dictionary<string, int> ItemsNeeded;
        internal string Result;
        internal int type;
        public CraftingRecipe(string id, string itemsNeeded, string result, int type)
        {
            Id = id;
            ItemsNeeded = new Dictionary<string, int>();
            var splitted = itemsNeeded.Split(';');
            foreach (var split in splitted)
            {
                var item = split.Split(':');
                if (item.Length != 2)
                {
                    continue;
                }

                ItemsNeeded.Add(item[0], Convert.ToInt32(item[1]));
            }
            Result = result;
        }
    }
}