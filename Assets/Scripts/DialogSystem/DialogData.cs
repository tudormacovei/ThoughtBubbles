[System.Serializable]
public class DialogData
{
    public string question;

    //Map each choice to a corresponding index of damage
    public string[] choices;
    public int[] damageNumber;
}
