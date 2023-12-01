using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    public static bool isSinglePlayer = true;
    public int currentPlayer = 1;
    public Sprite[] spriteArray = new Sprite[6];
    private int[,] mainBoard = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    private bool gameFinished = false;


    public GameObject[] row1 = new GameObject[3];
    public GameObject[] row2 = new GameObject[3];
    public GameObject[] row3 = new GameObject[3];
    private GameObject[,] objectBoard;

    private GameObject[] winningBoxes = new GameObject[3];
    public GameObject endScreen;

    public bool singlePlayer = true;

    void Start()
    {
        // Create board of gameObjects
        objectBoard = new GameObject[3, 3]{
            { row1[0], row1[1], row1[2] },
            { row2[0], row2[1], row2[2] },
            { row3[0], row3[1], row3[2] }};

        // Set boxPositions
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                objectBoard[i, j].GetComponent<Box>().boxPosition = i + "" + j;
            }
        }
    }


    public void restartGame()
    {
        gameFinished = false;
        mainBoard = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        winningBoxes = new GameObject[3];
        currentPlayer = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                objectBoard[i, j].GetComponent<Image>().sprite = spriteArray[0];
                objectBoard[i, j].GetComponent<Animator>().Play("boxIdle");
            }
        }
        endScreen.SetActive(false);
    }


    public int isWinner()
    {
        // Check horizontal
        for (int i = 0; i < 3; i++)
        {
            if (mainBoard[i, 0] == currentPlayer && mainBoard[i, 1] == currentPlayer && mainBoard[i, 2] == currentPlayer)
            {
                winningBoxes[0] = objectBoard[i, 0];
                winningBoxes[1] = objectBoard[i, 1];
                winningBoxes[2] = objectBoard[i, 2];
                return 1;
            }
        }

        // Check Vertical
        for (int i = 0; i < 3; i++)
        {
            if (mainBoard[0, i] == currentPlayer && mainBoard[1, i] == currentPlayer && mainBoard[2, i] == currentPlayer)
            {
                winningBoxes[0] = objectBoard[0, i];
                winningBoxes[1] = objectBoard[1, i];
                winningBoxes[2] = objectBoard[2, i];
                return 1;
            }
        }

        // Check diag
        if (mainBoard[0, 0] == currentPlayer && mainBoard[1, 1] == currentPlayer && mainBoard[2, 2] == currentPlayer)
        {
            winningBoxes[0] = objectBoard[0, 0];
            winningBoxes[1] = objectBoard[1, 1];
            winningBoxes[2] = objectBoard[2, 2];
            return 1;
        }
        if (mainBoard[2, 0] == currentPlayer && mainBoard[1, 1] == currentPlayer && mainBoard[0, 2] == currentPlayer)
        {
            winningBoxes[0] = objectBoard[2, 0];
            winningBoxes[1] = objectBoard[1, 1];
            winningBoxes[2] = objectBoard[0, 2];
            return 1;
        }

        // Check for draw
        int counter = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mainBoard[i, j] != 0)
                {
                    counter++;
                }
            }
        }
        if (counter == 9)
        {
            winningBoxes[0] = null;
            winningBoxes[1] = null;
            winningBoxes[2] = null;
            return 0;
        }

        return 69;
    }

    public void nextPlayer()
    {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else
        {
            currentPlayer = 1;
        }
    }

    public int opponentOf(int player)
    {
        if (player == 1)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    public bool threeInRow(int a, int b, int c)
    {
        return (a == b) && (b == c) && (a != 0);
    }

    public int minimaxIsWinner(int player)
    {
        // Check horizontal
        for (int i = 0; i < 3; i++)
        {
            if (threeInRow(mainBoard[i, 0], mainBoard[i, 1], mainBoard[i, 2]))
            {
                return 1;
            }
        }

        // Check Vertical
        for (int i = 0; i < 3; i++)
        {
            if (threeInRow(mainBoard[0, i], mainBoard[1, i], mainBoard[2, i]))
            {
                return 1;
            }
        }

        // Check diag
        if (threeInRow(mainBoard[0, 0], mainBoard[1, 1], mainBoard[2, 2]))
        {
            return 1;
        }
        if (threeInRow(mainBoard[2, 0], mainBoard[1, 1], mainBoard[0, 2]))
        {
            return 1;
        }

        // Check for draw
        int counter = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mainBoard[i, j] != 0)
                {
                    counter++;
                }
            }
        }
        if (counter == 9)
        {
            return 0;
        }

        return 69;
    }

    public void computerMove()
    {
        if (!gameFinished)
        {
            int x = -1, y = -1;
            int bestScore = -1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (mainBoard[i, j] == 0)
                    {
                        mainBoard[i, j] = currentPlayer;
                        int localScore = minimax(currentPlayer, false);
                        mainBoard[i, j] = 0;
                        if (localScore > bestScore)
                        {
                            bestScore = localScore;
                            x = i;
                            y = j;
                        }
                    }
                }
            }

            // Board is full
            if (x == -1 && y == -1)
            {
                return;
            }

            // Apply best move
            mainBoard[x, y] = currentPlayer;
            GameObject box = objectBoard[x, y];
            makeMove(box);
        }
    }


    private int minimax(int player, bool isMax)
    {
        if (isMax)
        {
            // Check for opponent
            int evaluateValue = minimaxIsWinner(opponentOf(player));
            switch (evaluateValue)
            {
                case 0:
                    return 0;
                case 1:
                    return -100;
            }

            int bestScore = -10000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (mainBoard[i, j] == 0)
                    {
                        mainBoard[i, j] = player;
                        int score = minimax(player, false);
                        mainBoard[i, j] = 0;
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            // Check for bot
            int evaluateValue = minimaxIsWinner(player);
            switch (evaluateValue)
            {
                case 0:
                    return 0;
                case 1:
                    return 100;
            }

            int bestScore = 10000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (mainBoard[i, j] == 0)
                    {
                        mainBoard[i, j] = opponentOf(player);
                        int score = minimax(player, true);
                        mainBoard[i, j] = 0;
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    public void buttonClicked(GameObject box)
    {

        if (isSinglePlayer)
        {
            if (singlePlayer)
            {
                string move = box.GetComponent<Box>().boxPosition;
                int x = move[0] - '0';
                int y = move[1] - '0';
                if (mainBoard[x, y] == 0 && !gameFinished)
                {
                    mainBoard[x, y] = currentPlayer;
                    makeMove(box);
                    singlePlayer = false;
                    StartCoroutine(computerMoveWithDelay());
                }
                else
                {
                    Debug.Log("ERROR: Invalid Move!");
                }
            }
        }
        else
        {
            string move = box.GetComponent<Box>().boxPosition;
            int x = move[0] - '0';
            int y = move[1] - '0';
            if (mainBoard[x, y] == 0 && !gameFinished)
            {
                mainBoard[x, y] = currentPlayer;
                makeMove(box);
            }
            else
            {
                Debug.Log("ERROR: Invalid Move!");
            }
        }


    }

    public void makeMove(GameObject box)
    {
        box.GetComponent<Image>().sprite = spriteArray[currentPlayer];
        box.GetComponent<Animator>().Play("boxClicked");

        int evaluateValue = isWinner();
        switch (evaluateValue)
        {
            case 0:
                gameFinished = true;
                StartCoroutine(displayEndscreen(0));
                Debug.Log("DRAW!");
                return;
            case 1:
                gameFinished = true;
                StartCoroutine(displayEndscreen(currentPlayer));
                Debug.Log("Player " + currentPlayer + " WON!");
                return;
        }
        nextPlayer();
    }

    void makeBoxesVanish()
    {
        StartCoroutine(makeBoxesVanishWithDelay());
    }

    IEnumerator computerMoveWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        computerMove();
        singlePlayer = true;
    }

    IEnumerator makeBoxesVanishWithDelay()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if index is a winner box
                if (Array.IndexOf(winningBoxes, objectBoard[i, j]) == -1)
                {
                    objectBoard[i, j].GetComponent<Animator>().Play("boxVanish");
                    yield return new WaitForSeconds(0.1f);
                }
            }

        }
    }

    IEnumerator displayEndscreen(int winningPlayer)
    {
        makeBoxesVanish();
        yield return new WaitForSeconds(1.2f);
        endScreen.SetActive(true);
        endScreen.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = spriteArray[winningPlayer + 3];
    }

    public void logBoard()
    {
        string s = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                s += mainBoard[i, j] + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

}
