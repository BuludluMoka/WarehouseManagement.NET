using Warehouse.Data.Models;

namespace Warehouse.Core.Services
{
    static public class CategoryTree
    {
        
        public static List<Category> GetCategoryTree(List<Category> categories)
        {
            foreach (var item in categories)
            {
                item.categoryChildren = GetCategoryChildren(categories, item);
            }

            return categories.Where(b => b.ParentId == null).ToList();
        }

        private static List<Category> GetCategoryChildren(List<Category> allCategories, Category Category)
        {
            //base case
            if (allCategories.All(b => b.ParentId != Category.Id)) return null;

            //recursive case
            Category.categoryChildren = allCategories
                .Where(b => b.ParentId == Category.Id)
                .ToList();

            foreach (var item in Category.categoryChildren)
            {
                item.categoryChildren = GetCategoryChildren(allCategories, item);
            }

            return Category.categoryChildren;
        }

    }
}
