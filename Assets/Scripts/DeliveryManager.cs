using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance {get; private set;}
    [SerializeField] private RecipeListSO recipeListSO;



    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private void Awake() {
        
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax) {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
            Debug.Log(waitingRecipeSO.recipeName);
            waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i=0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                //Has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSO.kitchenObjectSOList) {
                    //Cycling through all ingredients in recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList()) {
                        //Cycling through all the ingredients on the plate
                        if (plateKitchenObjectSo == recipeKitchenObjectSo) {
                            //Ingredient matches
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        //This Recipe ingredient was not found on plate
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe){
                    //Player delivered the correct recipes
                    Debug.Log("Player Delivered the Correct Recipe!");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        //No matches found
        //Player did not deliver a correct recipe
        Debug.Log("Player did not deliver the correct recipe");
    }
}
