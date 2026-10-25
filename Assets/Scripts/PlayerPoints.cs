using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public TMP_Text textToDisplay;
    private int points;

    void Start() {
        if (!textToDisplay) {
            Debug.LogWarning(name + ": ¡No hay texto asignado para mostrar los puntos!");
        }
        points = 0;
    }

    public void AddPoints(int pointsToAdd) {
        if (pointsToAdd <= 0) {
            Debug.LogWarning(name + ": ¡Se están intentando sumar puntos igual a 0 o negativos!");
        }
        Debug.Log(name + ": Tengo " + points + " puntos, sumando " + pointsToAdd + "...");
        points += pointsToAdd;
        UpdateText();
    }

    public int GetPoints() {
        return points;
    }

    void UpdateText() {
        textToDisplay.text = points.ToString();
    }
}
