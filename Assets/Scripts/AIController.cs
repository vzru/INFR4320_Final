/*
Code written for INFR 4320
Victor Zhang 100421055
Mathooshan Thevakumaran 100553777
Regan Tran 100622360
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any Action that the AI can do
public enum Actions
{
    DONOTHING,
    BOMB
}

// Different states for each lane
public enum States
{
    EMPTY,
    ENEMY
}

// Location of lane sections
public enum StateLoc
{
    LANEA,
    LANEB
}

// AI Controller that handles all the AI
public class AIController : MonoBehaviour
{
    public GameObject Tower;
    public States[] states;
    public int hp = 10;
    public int kills = 0;
    //public float timer = 0;
    public static int reward = 0;
    public bool gameEnd = false;

    public float[,,] policy = new float[2, 2, 2];

    // Start is called before the first frame update
    void Start()
    {
        states = new States[2];
        for (int i = 0; i < 2; i++)
        {
            states[i] = States.EMPTY;
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    policy[i, j, k] = 0.33f;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnUpdate();
    }

    void TurnUpdate()
    {
        if (!gameEnd)
        {
            // Game State Updates
            MoveEnemy(ref states);
            SpawnEnemy(ref states);

            // AI Action Selection
            Actions action = Actions.DONOTHING;
            float randAction = Random.Range(0.0f, 1.0f);
            StateLoc newTarget = StateLoc.LANEA;
            float randTarget = Random.Range(0.0f, 1.0f);
            if (randAction <= policy[(int)action, (int)states[0], (int)states[1]])
            {
                action = Actions.BOMB;
                if (randTarget <= 0.5f)
                {
                    newTarget = StateLoc.LANEA;
                }
                else
                {
                    newTarget = StateLoc.LANEB;
                }
            }
            int actionIndex = 0;
            int stateLaneA = 0;
            int stateLaneB = 0;
            if (action == Actions.BOMB)
            {
                actionIndex = 1;
            }
            if (states[0] == States.ENEMY)
            {
                stateLaneA = 1;
            }
            if (states[1] == States.ENEMY)
            {
                stateLaneB = 1;
            }
            if (action == Actions.BOMB)// && timer <= 0)
            {
                // Looping through each lane and check the states to update rewards
                for (int i = 0; i < 2; i++)
                {
                    if ((int)newTarget == i)
                    {
                        //timer = 5.0f;

                        // State Checker
                        if (states[i] == States.EMPTY)
                        {
                            --reward;
                        }
                        else if (states[i] == States.ENEMY)
                        {
                            ++reward;
                            ++kills;
                        }

                        // Bombed the state and wiped clean
                        states[i] = States.EMPTY;
                    }
                }
            }
            else
            {
                // For doing nothing action
                for (int i = 0; i < 2; i++)
                {
                    if ((int)newTarget == i)
                    {
                        if (states[i] == States.EMPTY)
                        {
                            ++reward;
                        }
                        if (states[i] == States.ENEMY)
                        {
                            reward -= (i + 1);
                            // Enemy is at the last section of the lane and will damage the base/tower since no action was taken
                            if (newTarget == StateLoc.LANEB)
                            {
                                --hp;
                            }
                        }
                    }
                }
            }

            // End Game Condition Checkers
            if (kills >= 10)
            {
                Debug.Log("You Win!");
                gameEnd = true;
            }

            if (hp <= 0)
            {
                Debug.Log("You Lose!");
                gameEnd = true;
            }

            // Apply the discount rate of 1%
            float rewardSum = GetNextReward(states, action, newTarget, kills, hp, reward) * 0.01f;
            Debug.Log("Rewards: " + reward.ToString() + ", Action: " + actionIndex.ToString() + ", LaneA: " + stateLaneA.ToString() + ", LaneB: " + stateLaneB.ToString());
            
            // Update Policy Matrixs based on reward value 
            policy[actionIndex, stateLaneA, stateLaneB] += rewardSum;
        }

    }

    // Recursive Function that returns the sum of all the future reward values given
    // Current state of the game, the next action, target of the next action, current progress towards win goal, current base's hp, current reward value
    public int GetNextReward(States[] currentState, Actions move, StateLoc target, int currentKills, int currentHP, int currentReward)
    {
        if (move == Actions.BOMB)// && timer <= 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if ((int)target == i)
                {
                    //timer = 5.0f;
                    if (currentState[i] == States.EMPTY)
                    {
                        --currentReward;
                    }
                    else if (currentState[i] == States.ENEMY)
                    {
                        ++currentReward;
                        ++currentKills;
                    }
                    currentState[i] = States.EMPTY;
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if ((int)target == i)
                {
                    if (currentState[i] == States.EMPTY)
                    {
                        ++currentReward;
                    }
                    if (currentState[i] == States.ENEMY)
                    {
                        currentReward -= (i + 1);
                        if (target == StateLoc.LANEB)
                        {
                            --currentHP;
                            currentReward -= 5;
                        }
                    }
                }
            }
        }

        if (currentKills >= 10)
        {
            //Debug.Log("Return Win");
            return 50 + currentReward;
        }

        MoveEnemy(ref currentState);
        SpawnEnemy(ref currentState);

        if (currentHP <= 0)
        {
            //Debug.Log("Return Lose");
            return -50 + currentReward;
        }

        Actions action = Actions.DONOTHING;
        float randAction = Random.Range(0.0f, 1.0f);
        StateLoc newTarget = StateLoc.LANEA;
        float randTarget = Random.Range(0.0f, 1.0f);
        if (randAction <= policy[(int)action, (int)states[0], (int)states[1]])
        {
            action = Actions.BOMB;
            if (randTarget <= 0.5f)
            {
                newTarget = StateLoc.LANEA;
            }
            else
            {
                newTarget = StateLoc.LANEB;
            }
        }
        // Recursive Call to explore the branches
        return currentReward + GetNextReward(currentState, action, newTarget, currentKills, currentHP, currentReward);
    }

    // Updates the states of all positions based on enemy movement
    public void MoveEnemy(ref States[] state)
    {
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == States.ENEMY)
            {
                if (i != (state.Length - 1))
                {
                    state[i + 1] = States.ENEMY;
                }
                state[i] = States.EMPTY;
            }
        }
    }

    // Update the state when an enemy is spawned
    public void SpawnEnemy(ref States[] state)
    {
        if (state[(int)StateLoc.LANEA] == States.EMPTY)
        {
            state[(int)StateLoc.LANEA] = States.ENEMY;
        }
    }
}
