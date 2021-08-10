using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

/*
 * Dispatcher自作←QueueにActionを保存してそれを順次実行する
 * 引数つきの場合λ式で書くと良い? 
 */
public class Dispatcher 
{
    private bool m_lock = false;
    private bool m_run = false;
    private Queue<Action> m_queue;

    public Dispatcher()
    {
        m_queue = new Queue<Action>();
    }

    //ActionをQueueに蓄える
    public void Invoke(Action action)
    {
        while (true)
        {
            if (!m_lock)
            {
                m_lock = true;
                m_queue.Enqueue(action);
                m_run = true;
                m_lock = false;
                break;
            }
            Debug.Log("lock");
        }
    }

    //Queueに蓄えられたActionを順に実行する
    public void Execute()
    {
        if (m_run && !m_lock)
        {
            m_lock = true;
            while (m_queue.Count > 0)
            {
                //Debug.Log(m_queue.Count);
                Action action = m_queue.Dequeue();
                if (action != null) action();
            }
            m_run = false;
            m_lock = false;
        }
    }
}
