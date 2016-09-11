using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public int defaultMoney;
	public static int money;
	public Text moneyText;

	void Start ()
	{
		money = defaultMoney;
	}

	void Update()
	{
		moneyText.text = money.ToString() + "$";
	}
}