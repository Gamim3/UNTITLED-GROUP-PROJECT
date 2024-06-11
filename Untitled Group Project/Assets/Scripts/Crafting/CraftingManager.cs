using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [SerializeField] Recipe[] allRecipes;
    public Recipe selectedRecipeToCraft;

    public Transform recipeListTransform;
    public GameObject itemPrefab;

    [Header("Recipe Selection")]
    [SerializeField] GameObject imagePrefab;
    [SerializeField] Transform imageListTransform;
    public CraftButton selectedButtonObject;
    [SerializeField] List<Recipe> _currentRecipes = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < recipeListTransform.childCount; i++)
        {
            _currentRecipes.Add(recipeListTransform.GetChild(i).GetComponent<CraftButton>().recipe);
        }
    }

    public void SelectCraftingRecipe(Recipe recipe, CraftButton button)
    {
        for (int i = imageListTransform.childCount; i > 0; i--)
        {
            Destroy(imageListTransform.GetChild(i - 1).gameObject);
        }
        if (recipe == null)
        {
            Destroy(button.gameObject);
            _currentRecipes.Remove(recipe);
            Debug.Log("No Recipe Selected");
            imageListTransform.gameObject.SetActive(false);
            selectedButtonObject = null;
            return;
        }

        selectedRecipeToCraft = recipe;
        selectedButtonObject = button;

        Debug.Log($"Selected Recipe {recipe}");
        imageListTransform.gameObject.SetActive(true);

        for (int i = 0; i < recipe.itemsNeeded.Length; i++)
        {
            GameObject spawnedImage = Instantiate(imagePrefab, imageListTransform);
            spawnedImage.GetComponent<Image>().sprite = recipe.itemsNeeded[i].item.image;
            spawnedImage.transform.GetChild(0).GetComponent<TMP_Text>().text = recipe.itemsNeeded[i].amount.ToString();
        }
    }

    public void CraftItem()
    {
        if (selectedRecipeToCraft != null)
        {
            if (!InventoryManager.Instance.HasSpace(selectedRecipeToCraft.itemToCraft, selectedRecipeToCraft.amountToCraft))
            {
                Debug.Log("Had No Place To Place Output From Crafting");
            }
            else
            {
                for (int i = 0; i < selectedRecipeToCraft.itemsNeeded.Length; i++)
                {
                    for (int y = 0; y < InventoryManager.Instance.itemsInInventory.Count; y++)
                    {
                        if (InventoryManager.Instance.itemsInInventory[y].item == selectedRecipeToCraft.itemsNeeded[i].item)
                        {
                            if (InventoryManager.Instance.itemsInInventory[y].amount < selectedRecipeToCraft.itemsNeeded[i].amount)
                            {
                                Debug.Log($"You don't have enough {selectedRecipeToCraft.itemsNeeded[i].item.name} to craft this item!");
                                return;
                            }
                        }
                    }
                }
                for (int i = 0; i < selectedRecipeToCraft.itemsNeeded.Length; i++)
                {
                    InventoryManager.Instance.UseItem(selectedRecipeToCraft.itemsNeeded[i].item.itemID, selectedRecipeToCraft.itemsNeeded[i].amount);
                }
                InventoryManager.Instance.AddItem(selectedRecipeToCraft.itemToCraft.itemID, selectedRecipeToCraft.amountToCraft);
                InventoryManager.Instance.UpdateItemsInfoList();
            }
        }
        else
        {
            Debug.Log("No Recipe Selected");
        }
    }

    public void AddRecipeButton(int itemID)
    {
        for (int r = 0; r < allRecipes.Length; r++)
        {
            if (allRecipes[r].itemToCraft.itemID == itemID)
            {
                Debug.Log("Found Recipe For " + allRecipes[r].itemToCraft);
                for (int i = 0; i < _currentRecipes.Count; i++)
                {
                    if (_currentRecipes[i] == null)
                    {
                        _currentRecipes.RemoveAt(i);
                    }
                    else if (_currentRecipes.Contains(allRecipes[r]))
                    {
                        Debug.LogError($"There is already a recipe for {allRecipes[r]}");
                        return;
                    }
                    else
                    {
                        AddRecipe(allRecipes[r]);
                        return;
                    }
                }
                AddRecipe(allRecipes[r]);
            }
        }

        Debug.LogError("Item ID Does Not Exist");
    }

    public void AddRecipe(Recipe recipe)
    {
        if (_currentRecipes.Contains(recipe))
        {
            return;
        }

        GameObject spawnedRecipe = Instantiate(itemPrefab, recipeListTransform);
        spawnedRecipe.GetComponent<CraftButton>().recipe = recipe;
        spawnedRecipe.GetComponent<CraftButton>().UpdateRecipeUI();
        _currentRecipes.Add(recipe);

        for (int i = 0; i < _currentRecipes.Count; i++)
        {
            if (_currentRecipes[i] == null)
            {
                _currentRecipes.RemoveAt(i);
            }
        }
    }
}
