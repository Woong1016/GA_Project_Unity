using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stone
{
    public string name;
    public int exp;
    public int price;
    public float efficiency;

    public Stone(string n, int e, int p)
    {
        name = n;
        exp = e;
        price = p;
        efficiency = (float)e / p;
    }
}

public class WeaponShop : MonoBehaviour
{
    public Text resultText;
    public Text infoText;

    public int currentLevel = 1;

    List<Stone> stones = new List<Stone>();

    void Start()
    {
        stones.Add(new Stone("강화석 소", 3, 8));
        stones.Add(new Stone("강화석 중", 5, 12));
        stones.Add(new Stone("강화석 대", 12, 30));
        stones.Add(new Stone("강화석 특대", 20, 45));

        UpdateUI();
    }

    int GetNeedExp()
    {
        return 8 * currentLevel * currentLevel;
    }

    void UpdateUI()
    {
        infoText.text = "+" + currentLevel + " -> +" + (currentLevel + 1) + "\n" +
                        "필요 경험치: " + GetNeedExp();
    }

    public void OnClickBruteForce()
    {
        int needExp = GetNeedExp();
        int minPrice = int.MaxValue;

        int bestSmall = 0, bestMedium = 0, bestLarge = 0, bestHuge = 0;
        int finalExp = 0;

        for (int i = 0; i <= needExp / 20 + 1; i++)
        {
            for (int j = 0; j <= needExp / 12 + 1; j++)
            {
                for (int k = 0; k <= needExp / 5 + 1; k++)
                {
                    for (int l = 0; l <= needExp / 3 + 1; l++)
                    {
                        int currentExp = (i * 20) + (j * 12) + (k * 5) + (l * 3);
                        int currentPrice = (i * 45) + (j * 30) + (k * 12) + (l * 8);

                        if (currentExp >= needExp)
                        {
                            if (currentPrice < minPrice)
                            {
                                minPrice = currentPrice;
                                finalExp = currentExp;
                                bestSmall = l;
                                bestMedium = k;
                                bestLarge = j;
                                bestHuge = i;
                            }
                            break;
                        }
                    }
                }
            }
        }

        PrintResult("Brute Force", bestSmall, bestMedium, bestLarge, bestHuge, minPrice, finalExp);
    }

    void DoGreedy(System.Comparison<Stone> sortMethod)
    {
        int targetExp = GetNeedExp();
        int currentExp = 0;
        int totalPrice = 0;

        int countSmall = 0;
        int countMedium = 0;
        int countLarge = 0;
        int countHuge = 0;

        List<Stone> sortedStones = new List<Stone>(stones);
        sortedStones.Sort(sortMethod);

        foreach (Stone s in sortedStones)
        {
            int remain = targetExp - currentExp;
            if (remain <= 0) break;

            int useCount = remain / s.exp;

            if (useCount > 0)
            {
                currentExp += useCount * s.exp;
                totalPrice += useCount * s.price;

                if (s.name == "강화석 소") countSmall += useCount;
                else if (s.name == "강화석 중") countMedium += useCount;
                else if (s.name == "강화석 대") countLarge += useCount;
                else if (s.name == "강화석 특대") countHuge += useCount;
            }
        }

        if (currentExp < targetExp)
        {
            Stone smallStone = stones[0];
            int gap = targetExp - currentExp;

            int needMore = gap / smallStone.exp;
            if (gap % smallStone.exp != 0)
            {
                needMore++;
            }

            countSmall += needMore;
            currentExp += needMore * smallStone.exp;
            totalPrice += needMore * smallStone.price;
        }

        PrintResult("Greedy", countSmall, countMedium, countLarge, countHuge, totalPrice, currentExp);
    }

    public void OnClickGreedyWasteMin()
    {
        DoGreedy((a, b) => a.exp.CompareTo(b.exp));
    }

    public void OnClickGreedyEfficiency()
    {
        DoGreedy((a, b) => b.efficiency.CompareTo(a.efficiency));
    }

    public void OnClickGreedyExpMax()
    {
        DoGreedy((a, b) => b.exp.CompareTo(a.exp));
    }

    void PrintResult(string type, int s, int m, int l, int xl, int cost, int exp)
    {
        string txt = "[" + type + " 결과]\n";

        if (s > 0) txt += "강화석 소 (exp3) x " + s + "\n";
        if (m > 0) txt += "강화석 중 (exp5) x " + m + "\n";
        if (l > 0) txt += "강화석 대 (exp12) x " + l + "\n";
        if (xl > 0) txt += "강화석 특대 (exp20) x " + xl + "\n";

        txt += "----------------\n";
        txt += "총 가격: " + cost + " gold\n";
        txt += "획득 경험치: " + exp + " / " + GetNeedExp();

        resultText.text = txt;
    }

    public void OnClickLevelUp()
    {
        currentLevel++;
        UpdateUI();
    }
}