using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    public GameObject inventroyPanel;
    public GameObject craftingPanel;

    public Transform playerRaycastPos;

    public LayerMask interactableLayers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (craftingPanel.activeSelf && inventroyPanel.activeSelf)
            {
                craftingPanel.SetActive(false);
            }
            inventroyPanel.SetActive(!inventroyPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerRaycastPos.position, playerRaycastPos.forward, out RaycastHit hit, interactableLayers))
            {
                if (hit.transform.tag == "Crafting")
                {
                    craftingPanel.SetActive(!craftingPanel.activeSelf);
                    if (!inventroyPanel.activeSelf)
                    {
                        inventroyPanel.SetActive(true);
                    }
                }
            }
        }
    }
}
