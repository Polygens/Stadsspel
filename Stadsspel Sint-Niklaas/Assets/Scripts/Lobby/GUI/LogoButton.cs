using UnityEngine;

public class LogoButton : MonoBehaviour
{
	/// <summary>
	/// Opens the website of "800 jaar Sint-Niklaas" in the browser.
	/// </summary>
	public void Open800StNikWebsite()
	{
		Application.OpenURL("http://800jaarsint-niklaas.be/");
	}

	/// <summary>
	/// Opens the website of "KBC" in the browser.
	/// </summary>
	public void OpenJOSWebsite()
	{
		Application.OpenURL("http://www.jos.be/");
	}
}
