using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int startingPoints = 0;
    public TMP_Text textToDisplay;
    private int points;

    void Start() {
        if (!textToDisplay) {
            Debug.LogWarning(name + ": ¡No hay texto asignado para mostrar los puntos!");
        }
        points = startingPoints;
    }

    public void AddPoints(int pointsToAdd) {
        if (pointsToAdd <= 0) {
            Debug.LogWarning(name + ": ¡Se están intentando sumar puntos igual a 0 o negativos!");
        }
        Debug.Log(name + ": Tengo " + points + " puntos, sumando " + pointsToAdd + "...");
        points += pointsToAdd;
        UpdateText();
    }

    public void TakePoints(int pointsToTake) {
        if (pointsToTake <= 0) {
            Debug.LogWarning(name + ": ¡Se están intentando restar puntos igual a 0 o negativos!");
        }
        Debug.Log(name + ": Tengo " + points + " puntos, restando " + pointsToTake + "...");
        points -= pointsToTake;
        UpdateText();
    }

    public int GetPoints() {
        return points;
    }

    void UpdateText() {
        textToDisplay.text = points.ToString();
    }
}
