using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PLAY : MonoBehaviour
{
    new Text name;
    string playername;
    string symbols;
    GameObject btnHolder;
    PhotonView pv;
    bool isPlayerATurn;
    bool gameEnded = false;
    public GameObject plX_panale,plO_panale,Draw_panal;
    public Text scoreX_Text, scoreO_Text;

    // Start is called before the first frame update
    void Start()
    {
        name = GameObject.Find("name").GetComponent<Text>();
        playername = PhotonNetwork.NickName;
        name.text = playername;
        btnHolder = GameObject.Find("btnHolder");
        pv = GetComponent<PhotonView>();

        // Determine the initial turn based on player name.
        isPlayerATurn = (playername == "player A");
        if (PhotonNetwork.IsMasterClient)
        {
            isPlayerATurn = true;
        }
        else
        {
            isPlayerATurn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void btnclick(int no)
    {
      

        if ((isPlayerATurn && playername == "player A") || (!isPlayerATurn && playername == "player B"))
        {
            string str = "";
            if (playername == "player A")
            {
                str = "O";
            }
            if (playername == "player B")
            {
                str = "X";
            }
            btnHolder.transform.GetChild(no).GetComponentInChildren<Text>().text = str;
            btnHolder.transform.GetChild(no).GetComponentInChildren<Button>().interactable = false;

            pv.RPC("visible", RpcTarget.All, no, str, !isPlayerATurn); // Send the new turn state
        }
    }

    [PunRPC]
    void visible(int n, string s, bool newTurn)
    {
        btnHolder.transform.GetChild(n).GetComponentInChildren<Text>().text = s;
        btnHolder.transform.GetChild(n).GetComponentInChildren<Button>().interactable = false;

        if (wincheck("X"))
        {
            print("X is win ");
            btnclose();
            gameEnded = true;
            plX_panale.SetActive(true);
            UpdateScore("X");
            // restartbtn();

            return;
        }
        if (wincheck("O"))
        {
            print("O is win");
            btnclose();
            gameEnded = true;
            plO_panale.SetActive(true);
            UpdateScore("O");
            //restartbtn();

            return;
        }

        if (IsBoardFull())
        {
            print("match draw !....");
            btnclose();
            gameEnded = true;
            Draw_panal.SetActive(true);
           

            //  restartbtn();

            return;
        }

        isPlayerATurn = newTurn; // Update the turn state.
    }
    void UpdateScore(string winner)
    {
        if (winner == "X")
        {
            int scoreX = PlayerPrefs.GetInt("ScoreX", 0) + 1;
            PlayerPrefs.SetInt("ScoreX", scoreX);
        }
        else if (winner == "O")
        {
            int scoreO = PlayerPrefs.GetInt("ScoreO", 0) + 1;
            PlayerPrefs.SetInt("ScoreO", scoreO);
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreX_Text.text = "X: " + PlayerPrefs.GetInt("ScoreX", 0);
        scoreO_Text.text = "O: " + PlayerPrefs.GetInt("ScoreO", 0);
    }
    bool wincheck(string symbols)
    {
        if (btnHolder.transform.GetChild(0).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(1).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(2).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(3).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(4).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(5).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(6).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(7).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(8).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(0).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(3).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(6).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(1).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(4).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(7).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(2).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(5).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(8).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(0).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(4).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(8).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        else if (btnHolder.transform.GetChild(2).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(4).GetComponentInChildren<Text>().text == symbols &&
           btnHolder.transform.GetChild(6).GetComponentInChildren<Text>().text == symbols)
        {
            return true;
        }
        return false;
    }
    bool IsBoardFull()
        {
            for (int i = 0; i < 9; i++)
            {
                if (btnHolder.transform.GetChild(i).GetComponentInChildren<Text>().text == "")
                {
                    return false;
                }
            }
            return true;
        }
  
        void btnclose()
        {
            for (int i = 0; i <= 8; i++)
            {
                btnHolder.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;
            }
        }
    [PunRPC]
    void restartbtn()
    {
        for (int i = 0; i < 9; i++)
        {
            btnHolder.transform.GetChild(i).GetComponentInChildren<Text>().text = "";
            btnHolder.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
        }
        isPlayerATurn = true;
        gameEnded = false;
        plX_panale.SetActive(false);
        plO_panale.SetActive(false);
        Draw_panal.SetActive(false);
    }

    public void retry()
    {
        pv.RPC("restartbtn", RpcTarget.All);
    }
}