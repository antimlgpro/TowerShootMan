using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIManager : UIManagerBase
{
	[SerializeField] private Button playButton;
	//[SerializeField] private Button fastForwardButton;


    public override bool LoadUIManager()
    {
		playButton.onClick.AddListener(OnClickPlay);

		return true;
    }

	private void OnClickPlay() {
		GameController.Instance.m_OnWaveTrigger.Invoke();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
