using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    DONOTHING,
    BOMB
}

public enum States
{
    EMPTY,
    ENEMY
}

public enum StateLoc
{
    //TOWER,
    LANEA,
    LANEB
}

public class AIController : MonoBehaviour
{
    public GameObject Tower;
    public States[] states;
    public int hp = 10;
    public int kills = 0;
    //public float timer = 0;
    public static int reward = 0;

    public float[,,] policy = new float[2, 2, 2];

    // Start is called before the first frame update
    void Start()
    {
        states = new States[2];
        for(int i = 0; i < 2; i++)
        {
            states[i] = States.EMPTY;
        }
    }
     
    // Update is called once per frame
    void Update()
    {
        MoveEnemy(ref states);
        SpawnEnemy(ref states);
        Actions action = Actions.DONOTHING;
        float randAction = Random.Range(0.0f, 1.0f);
        //Debug.Log("Action Random: " + randAction.ToString());
        StateLoc newTarget = StateLoc.LANEA;
        float randTarget = Random.Range(0.0f, 1.0f);
        //Debug.Log("Target Random: " + randTarget.ToString());
        if (randAction <= (2.0f / 3.0f))
        {
            action = Actions.BOMB;
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; i < 2; j++)
            //    {
            //        rand = rand - policy[i, j];
            //        if (rand <= 0)
            //        {
            //            newTarget = (StateLoc)((i * 2) + j);
            //            break;
            //        }
            //    }
            //}
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
        if(action == Actions.BOMB)
        {
            actionIndex = 1;
        }
        if(states[0] == States.ENEMY)
        {
            stateLaneA = 1;
        }
        if(states[1] == States.ENEMY)
        {
            stateLaneB = 1;
        }
        float reward = GetNextReward(states, action, newTarget, kills, hp) * 0.01f;
        Debug.Log("Rewards: " + reward.ToString() + ", Action: " + actionIndex.ToString() + ", LaneA: " + stateLaneA.ToString() + ", LaneB: " + stateLaneB.ToString());
        policy[actionIndex, stateLaneA, stateLaneB] += reward;

        if (action == Actions.BOMB)// && timer <= 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if ((int)newTarget == i)
                {
                    //timer = 5.0f;
                    if (states[i] == States.EMPTY)
                    {
                        --reward;
                    }
                    else if (states[i] == States.ENEMY)
                    {
                        ++reward;
                        ++kills;
                    }
                    states[i] = States.EMPTY;
                }
            }
        }
        else
        {
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
                        if (newTarget == StateLoc.LANEB)
                        {
                            --hp;
                        }
                    }
                }
            }
        }
    }

    public int GetNextReward(States[] currentState, Actions move, StateLoc target, int currentKills, int currentHP)
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
                        --reward;
                    }
                    else if (currentState[i] == States.ENEMY)
                    {
                        ++reward;
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
                        ++reward;
                    }
                    if (currentState[i] == States.ENEMY)
                    {
                        reward -= (i + 1);
                        if (target == StateLoc.LANEB)
                        {
                            --currentHP;
                        }
                    }
                }
            }
        }

        if (currentKills >= 10)
        {
            Debug.Log("Return Win");
            return 50;
        }

        MoveEnemy(ref currentState);
        SpawnEnemy(ref currentState);

        if (currentHP <= 0)
        {
            Debug.Log("Return Lose");
            return -50;
        }

        Actions action = Actions.DONOTHING;
        float randAction = Random.Range(0.0f, 1.0f);
        StateLoc newTarget = StateLoc.LANEA;
        float randTarget = Random.Range(0.0f, 1.0f);
        if (randAction <= (2.0f / 3.0f))
        {
            action = Actions.BOMB;
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; i < 2; j++)
            //    {
            //        rand = rand - policy[i, j];
            //        if (rand <= 0)
            //        {
            //            newTarget = (StateLoc)((i * 2) + j);
            //            break;
            //        }
            //    }
            //}
            if(randTarget <= 0.5f)
            {
                newTarget = StateLoc.LANEA;
            }
            else
            {
                newTarget = StateLoc.LANEB;
            }
        }
        GetNextReward(currentState, action, newTarget, currentKills, currentHP);

        return 0;
    }
    
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

    public void SpawnEnemy(ref States[] state)
    {
        //if (Random.Range(0.0f, 1.0f) >= 0.5f)
        //{
            if (state[(int)StateLoc.LANEA] == States.EMPTY)
            {
                state[(int)StateLoc.LANEA] = States.ENEMY;
            }
        //}
    }
}
