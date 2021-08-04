using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_Management
{
    public static class SearchHelper
    {
        public static IEnumerable<Inventory> SearchInventories(string searchKey, IEnumerable<Inventory> inventories)
        {
            if (string.IsNullOrWhiteSpace(searchKey)) return inventories;
            var searchables = GenerateSearchKeys(inventories);
            return SearchInventories(searchKey, searchables);
        }

        public static IEnumerable<Inventory> SearchInventories(
            string searchKey,
            List<(string Key, Inventory Inventory, long Weight)> searchables)
        {
            if (string.IsNullOrWhiteSpace(searchKey)) return searchables.Select(s => s.Inventory).ToArray();
            
            var words = searchKey?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var searchIndexes = GetSearchIndex(words, searchables);
            return searchIndexes.Select(p => p.Key).ToList();
        }

        public static List<(string Key, Inventory Inventory, long Weight)> GenerateSearchKeys(IEnumerable<Inventory> inventories)
        {
            var searchables = new List<(string Key, Inventory Inventory, long Weight)>();
            foreach (var inv in inventories)
            {
                searchables.Add((inv.Category, inv, 3));
                searchables.Add((inv.SubCategory, inv, 3));
                searchables.Add((inv.Name, inv, 5));
                searchables.Add((inv.Hsn, inv, 1));

                foreach (var w in inv.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    searchables.Add((w, inv, 1));
            }
            return searchables;
        }

        private static Dictionary<Inventory, long> GetSearchIndex(
            string[] words, 
            List<(string Key, Inventory Inventory, long Weight)> searchables)
        {
            if (searchables == null) return null;
            if (words == null) return null;

            var indexes = new Dictionary<Inventory, long>();
            foreach (var word in words)
            {
                foreach (var searchable in searchables)
                {
                    if (searchable.Key.StartsWith(word, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!indexes.ContainsKey(searchable.Inventory))
                            indexes.Add(searchable.Inventory, searchable.Weight);
                        else
                            indexes[searchable.Inventory] += searchable.Weight;
                    }
                }
            }
            return indexes.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
